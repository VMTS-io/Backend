using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VMTS.API.Dtos;
using VMTS.API.Errors;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.ServicesContract;
using VMTS.Core.Specifications;

namespace VMTS.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _iunitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UserController(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork iunitOfWork,
        IMapper mapper,
        IUserService userService,
        IAuthService authService
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _iunitOfWork = iunitOfWork;
        _mapper = mapper;
        _userService = userService;
        _authService = authService;
    }

    #region Create User
    [HttpPost("add")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegisterResponse>> Create(RegisterRequest model)
    {
        var email = await _authService.GenerateUniqueEmailAsync(model.FirstName, model.LastName);
        var password = "Pa$$w0rd";

        if (await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber))
            return BadRequest(new ApiErrorResponse(400, "Phone number is already exists."));

        if (await _userManager.Users.AnyAsync(u => u.NationalId == model.NationalId))
            return BadRequest(new ApiErrorResponse(400, "National ID is already exists."));

        var displayName = $"{model.FirstName?.Trim()} {model.LastName?.Trim()}";
        var address = _mapper.Map<Address>(model.Address);

        var user = new AppUser
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = email,
            UserName = email.Split('@')[0],
            PhoneNumber = model.PhoneNumber,
            DateOfBirth = model.DateOfBirth,
            NationalId = model.NationalId,
            DisplayName = displayName,
            PictureUrl = "default-profile.png",
            Address = address,
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse(400));
        }

        if (!await _roleManager.RoleExistsAsync(model.Role))
        {
            return BadRequest(new ApiErrorResponse(400, "Invalid role"));
        }

        var userRole = await _userManager.AddToRoleAsync(user, model.Role);
        if (!userRole.Succeeded)
        {
            return BadRequest(new ApiErrorResponse(400));
        }

        var businessUser = new BusinessUser
        {
            Id = user.Id,
            DisplayName = displayName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            NormalizedEmail = user.NormalizedEmail,
        };

        await _iunitOfWork.GetRepo<BusinessUser>().CreateAsync(businessUser);
        await _iunitOfWork.SaveChanges();

        return Ok(new RegisterResponse { Email = email });
    }

    #endregion


    #region Get All Users

    [HttpGet]
    [Authorize(Roles = Roles.Manager)]
    [ProducesResponseType(
        typeof(IReadOnlyList<BusinessUserGetAllResponse>),
        StatusCodes.Status200OK
    )]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<BusinessUserGetAllResponse>>> GetAll(
        [FromQuery] BusinessUserSpecParams specParams
    )
    {
        var users = await _userService.GetAllUsersAsync(specParams);

        var userResponses = _mapper.Map<IReadOnlyList<BusinessUserGetAllResponse>>(users);

        return Ok(userResponses);
    }

    #endregion

    #region GetAllManagers

    [HttpGet("managers")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetManagers()
    {
        var users = await _userManager.Users.Include(u => u.Address).ToListAsync();
        var managers = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Manager"))
            {
                var mappedUser = _mapper.Map<UserResponse>(user);
                mappedUser.Role = roles.FirstOrDefault();
                managers.Add(mappedUser);
            }
        }

        return Ok(managers);
    }

    #endregion

    #region Get All Drivers

    [HttpGet("drivers")]
    [Authorize(Roles = $"{Roles.Driver},{Roles.Manager}")]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetDrivers()
    {
        var users = await _userManager.Users.Include(u => u.Address).ToListAsync();
        var drivers = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Driver"))
            {
                var mappedUser = _mapper.Map<UserResponse>(user);
                mappedUser.Role = roles.FirstOrDefault();
                drivers.Add(mappedUser);
            }
        }

        return Ok(drivers);
    }

    #endregion

    #region Get All Mechanics

    [HttpGet("mechanics")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = $"{Roles.Driver},{Roles.Manager}"
    )]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetMechanics()
    {
        var users = await _userManager.Users.Include(u => u.Address).ToListAsync();
        var mechanics = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Mechanic"))
            {
                var mappedUser = _mapper.Map<UserResponse>(user);
                mappedUser.Role = roles.FirstOrDefault();
                mechanics.Add(mappedUser);
            }
        }

        return Ok(mechanics);
    }

    #endregion

    #region Get User By Id
    [HttpGet("{userId}")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = $"{Roles.Driver},{Roles.Manager}"
    )]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> GetById([FromRoute] string userId)
    {
        var user = await _userManager
            .Users.Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return BadRequest(new ApiErrorResponse(400, "User not found"));

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault();

        return Ok(
            new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                NationalId = user.NationalId,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Role = role,
                Address = new AddressDto
                {
                    Street = user.Address?.Street,
                    Area = user.Address?.Area,
                    Governorate = user.Address?.Governorate,
                    Country = user.Address?.Country,
                },
            }
        );
    }

    #endregion


    #region Delete User

    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteUser([FromRoute] string Id)
    {
        await _userService.DeleteUserAsync(Id);
        return NoContent();
    }

    #endregion

    #region Edit User
    [HttpPut("{Id}")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Edit(EditUserRequest request, [FromRoute] string Id)
    {
        await _userService.EditUserAsync(
            Id,
            request.FirstName,
            request.LastName,
            request.NationalId,
            request.DateOfBirth,
            request.PhoneNumber,
            request.Address.Street,
            request.Address.Area,
            request.Address.Governorate,
            request.Address.Country,
            request.Role
        );

        return NoContent();
    }
    #endregion
}
