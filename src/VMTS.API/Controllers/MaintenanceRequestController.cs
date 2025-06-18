using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Maintenance.Request;
using VMTS.API.Errors;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Specifications.Maintenance;

namespace VMTS.API.Controllers;

[Tags("Maintenance/Request")]
[Route("api/Maintenance/Request")]
public class MaintenanceRequestController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IMaintenanceRequestServices _services;

    public MaintenanceRequestController(IMapper mapper, IMaintenanceRequestServices services)
    {
        _mapper = mapper;
        _services = services;
    }

    #region Create
    [Authorize(
        Roles = Roles.Manager,
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme
    )]
    [HttpPost]
    public async Task<ActionResult<MaintenanceRequestDto>> Create(MaintenanceRequestDto model)
    {
        var mappedModel = _mapper.Map<MaintenanceRequestDto, MaintenaceRequest>(model);
        await _services.CreateAsync(mappedModel, User);
        return Ok(model);
    }
    #endregion

    #region Edit
    [Authorize(
        Roles = Roles.Manager,
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme
    )]
    [HttpPut]
    public async Task<ActionResult<MaintenanceRequestDto>> Edit(MaintenanceRequestEdit model)
    {
        var mappedModel = _mapper.Map<MaintenanceRequestEdit, MaintenaceRequest>(model);
        await _services.UpdateAsync(mappedModel, User);
        return Ok(model);
    }
    #endregion

    #region Get By Id
    [Authorize(
        Roles = $"{Roles.Manager},{Roles.Mechanic}",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme
    )]
    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceRequestDto>> GetById([FromRoute] string id)
    {
        /*var role = User.FindFirstValue(ClaimTypes.Role);*/
        /*var claimEmail = User.FindFirstValue(ClaimTypes.Email);*/
        var result = await _services.GetByIdAsync(id);
        if (result is null)
            return NotFound(new ApiErrorResponse(404));
        /*if (role == "Mechanic" && claimEmail != result.MechanicEmail)*/
        /*    return Unauthorized(new ApiResponse(401));*/
        var mappedModel = _mapper.Map<MaintenaceRequest, MaintenanceRequestResponse>(result);
        return Ok(mappedModel);
    }
    #endregion

    #region Get All
    [Authorize(
        Roles = Roles.Manager,
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme
    )]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaintenanceRequestResponse>>> GetAll(
        [FromQuery] MaintenanceRequestSpecParams specParams
    )
    {
        var result = await _services.GetAllAsync(specParams);
        var mappedModel = _mapper.Map<
            IReadOnlyList<MaintenaceRequest>,
            IReadOnlyList<MaintenanceRequestResponse>
        >(result);
        return Ok(mappedModel);
    }
    #endregion

    #region Get All For User
    [Authorize(
        Roles = Roles.Manager,
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme
    )]
    [Route("mechanic")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaintenanceRequestResponse>>> GetAllForUser(
        [FromQuery] MaintenanceRequestSpecParamsForMechanic specParams
    )
    {
        var result = await _services.GetAllForUserAsync(specParams, User);
        var mappedResult = _mapper.Map<
            IReadOnlyList<MaintenaceRequest>,
            IReadOnlyList<MaintenanceRequestResponse>
        >(result);
        return Ok(mappedResult);
    }
    #endregion
}
