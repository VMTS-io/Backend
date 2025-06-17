using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos;
using VMTS.API.Errors;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.ServicesContract;

namespace VMTS.API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IAuthService authService,
        IEmailService emailService,
        IConfiguration configuration,
        IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _emailService = emailService;
        _configuration = configuration;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    #region Login

    [HttpPost("login")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MustChangePasswordDto), StatusCodes.Status200OK)] 
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Unauthorized(new ApiErrorResponse(401, "Invalid credentials"));

        if (user.MustChangePassword)
        {
            return Ok( // Returning 200 OK, since it's a valid response for password change
                new MustChangePasswordDto
                {
                    MustChangePassword = true,
                    StatusCode = 200, // Changed to 200
                    Message = "Password reset required",
                }
            );
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new ApiErrorResponse(401, "Invalid credentials"));
        var businessUser = await _unitOfWork.GetRepo<BusinessUser>().GetByIdAsync(user.Id);
        var businessUserDto = _mapper.Map<BussinessUserDto>(businessUser);
        var tokenString = await _authService.CreateTokenAsync(user, _userManager);

        return Ok(new UserDto { Email = user.Email, Token = tokenString , BussinessUserDto = businessUserDto });
    }

    #endregion

    #region Reset Password

    [HttpPost("resetpassword")]
    [ProducesResponseType(typeof(ResetPasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResetPasswordResponse>> ResetPasswordAsync(
        Reset_PasswordRequest model
    )
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
            return Unauthorized(new ApiErrorResponse(401));

        var decodedToken = WebUtility.UrlDecode(model.Token);

        var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

        user.MustChangePassword = false;
        await _userManager.UpdateAsync(user);

        return Ok(
            new ResetPasswordResponse
            {
                Message = "Password reset successful. Please log in again.",
            }
        );
    }

    #endregion

    #region Forgot Password

    [HttpPost("forgotpassword")]
    [ProducesResponseType(typeof(ForgetPasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ForgetPasswordResponse>> ForgotPasswordAsync(
        Forgot_PasswordRequest model
    )
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
            return BadRequest(new ApiErrorResponse(400, "User not found"));

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var encodedToken = WebUtility.UrlEncode(token);

        var resetLink =
            $"{_configuration["ClientApp:BaseUrl"]}/reset-password?email={user.Email}&token={encodedToken}";

        var body = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";

        await _emailService.SendEmailAsync(user.Email, "Reset Your Password", body, isHtml: true);

        return Ok("Reset link sent.");
    }
    #endregion

    [HttpPost("send-email")]
    public async Task<IActionResult> SendTestEmail(SendEmailRequest request)
    {
        await _emailService.SendEmailAsync(request.toEmail, request.subject, request.body);
        return Ok("Email sent , please check your inbox");
    }
}
