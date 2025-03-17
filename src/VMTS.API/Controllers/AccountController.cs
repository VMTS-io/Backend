using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos;
using VMTS.API.Errors;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.ServicesContract;

namespace VMTS.API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAuthService _authService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _iunitOfWork;
    private readonly IMapper _mapper;

    public AccountController(
        UserManager<AppUser> userManager,
        IAuthService authService,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork iunitOfWork,
        IMapper mapper
    )
    {
        _userManager = userManager;
        _authService = authService;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _iunitOfWork = iunitOfWork;
        _mapper = mapper;
    }

    #region register

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest model)
    {
        var email = await _authService.GenerateUniqueEmailAsync(model.FirstName, model.LastName);

        var password = "Pa$$w0rd";

        var address = _mapper.Map<Address>(model.Address);

        var user = new AppUser()
        {
            Email = email,
            UserName = email.Split('@')[0],
            PhoneNumber = model.PhoneNumber,
            Address = address,
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return BadRequest(new ApiResponse(400));

        var role = await _roleManager.RoleExistsAsync(model.Role);
        if (!role)
            return NotFound(new ApiResponse(404));

        var userRole = await _userManager.AddToRoleAsync(user, model.Role);
        if (!userRole.Succeeded)
            return BadRequest(new ApiResponse(400));

        var businessUser = new BusinessUser()
        {
            Id = user.Id,
            DisplayName = $"{model.FirstName?.Trim()} {model.LastName?.Trim()}",
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            NormalizedEmail = user.NormalizedEmail,
        };

        await _iunitOfWork.GetRepo<BusinessUser>().CreateAsync(businessUser);
        await _iunitOfWork.SaveChanges();

        return Ok(new RegisterResponse() { Email = email });
    }

    #endregion




    #region login

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Unauthorized(new ApiResponse(401));

        // Force password reset check BEFORE checking password
        if (user.MustChangePassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Unauthorized(new ApiResponse(401, "Password reset required"));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new ApiResponse(401, "Invalid credentials"));

        var tokenString = await _authService.CreateTokenAsync(user, _userManager);

        return Ok(
            new UserDto()
            {
                Email = user.Email,
                // DisplayName = user.DisplayName,
                Token = tokenString,
            }
        );
    }

    #endregion




    #region reset password endpoints

    [HttpPost("resetpassword")]
    public async Task<ActionResult<ResetPasswordResponse>> ResetPasswordAsync(
        Reset_PasswordRequest model
    )
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
            return Unauthorized(new ApiResponse(401));

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

        if (!result.Succeeded)
            return Unauthorized(new ApiResponse(401));

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        user.MustChangePassword = false;

        await _userManager.UpdateAsync(user);

        return Ok(
            new ResetPasswordResponse
            {
                Message = "Password reset successful. Please log in again.",
            }
        );
    }

    [HttpPost("forgotpassword")]
    public async Task<ActionResult<ForgetPasswordResponse>> ForgotPasswordAsync(
        Forgot_PasswordRequest model
    )
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
            return BadRequest(new { Message = "User not found" });

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return Ok(new { Message = "Password reset token generated", Token = token });
    }

    #endregion
}

