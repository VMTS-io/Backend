﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos;
using VMTS.API.Errors;
using VMTS.Core.Entities.Identity;
using VMTS.Core.ServicesContract;

namespace VMTS.API.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IAuthService _authService;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IAuthService authService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
    }

    #region Login
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MustChangePasswordDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return NotFound(new ApiErrorResponse(404));

        if (user.MustChangePassword)
        {
            return Unauthorized(new MustChangePasswordDto
            {
                MustChangePassword = true,
                StatusCode = 401,
                Message = "Password reset required"
            });
        }


        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new ApiErrorResponse(401, "Invalid credentials"));

        var tokenString = await _authService.CreateTokenAsync(user, _userManager);

        return Ok(new UserDto
        {
            Email = user.Email,
            Token = tokenString,
        });
    }
    #endregion

    #region Reset Password
    [HttpPost("resetpassword")]
    [ProducesResponseType(typeof(ResetPasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResetPasswordResponse>> ResetPasswordAsync(Reset_PasswordRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
            return Unauthorized(new ApiErrorResponse(401));

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (!result.Succeeded)
            return BadRequest(new ApiErrorResponse(400));

        user.MustChangePassword = false;
        await _userManager.UpdateAsync(user);

        return Ok(new ResetPasswordResponse
        {
            Message = "Password reset successful. Please log in again.",
        });
    }
    #endregion

    #region Forgot Password
    [HttpPost("forgotpassword")]
    [ProducesResponseType(typeof(ForgetPasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ForgetPasswordResponse>> ForgotPasswordAsync(Forgot_PasswordRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
            return BadRequest(new ApiErrorResponse(400, "User not found"));

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return Ok(new ForgetPasswordResponse
        {
            Message = "Password reset token generated",
            Token = token
        });
    }
    }


    #endregion
