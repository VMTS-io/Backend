using Microsoft.AspNetCore.Http.HttpResults;
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
    private readonly IAuthService _authService;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(
        UserManager<AppUser> userManager,
        IAuthService authService,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _authService = authService;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest model)
    {
        var email = await _authService.GenerateUniqueEmailAsync(model.FirstName, model.LastName);

        var password = await _authService.GenerateSecurePasswordAsync(12);

        var user = new AppUser()
        {
            Email = email,
            UserName = email.Split('@')[0],
            PhoneNumber = model.PhoneNumber,
            DisplayName = $"{model.FirstName?.Trim()} {model.LastName?.Trim()}"
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return BadRequest(new ApiResponse(400));


        return Ok(new RegisterResponse()
        {
            Email = email,
            Password = password
        });
}

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user is null) return Unauthorized(new ApiResponse(401));
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        
        if(!result.Succeeded) return  Unauthorized(new ApiResponse(401));

        return Ok(new UserDto()
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = await _authService.CreateTokenAsync(user , _userManager)
        });
    }
    
}