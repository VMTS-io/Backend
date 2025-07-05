using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.ActionFilters;
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
    [ServiceFilter<ValidateModelActionFilter<MaintenanceRequestUpsertDto>>]
    [Authorize(Roles = Roles.Manager)]
    [HttpPost]
    public async Task<ActionResult> Create(MaintenanceRequestUpsertDto model)
    {
        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var mappedModel = _mapper.Map<MaintenanceRequestUpsertDto, MaintenaceRequest>(model);
        mappedModel.ManagerId = managerId!;
        await _services.CreateAsync(mappedModel, model.Parts);
        return NoContent();
    }
    #endregion

    #region Edit
    [ServiceFilter<ValidateModelActionFilter<MaintenanceRequestUpsertDto>>]
    [Authorize(Roles = Roles.Manager)]
    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(
        [FromBody] MaintenanceRequestUpsertDto model,
        [FromRoute] string id
    )
    {
        var mappedModel = _mapper.Map<MaintenanceRequestUpsertDto, MaintenaceRequest>(model);
        mappedModel.Id = id;
        await _services.UpdateAsync(mappedModel, model.Parts);
        return NoContent();
    }
    #endregion

    #region Delete
    [Authorize(Roles = Roles.Manager)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] string id)
    {
        await _services.DeleteAsync(id);
        return NoContent();
    }

    #endregion

    #region Get By Id
    [Authorize(Roles = $"{Roles.Manager},{Roles.Mechanic}")]
    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceRequestUpsertDto>> GetById([FromRoute] string id)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _services.GetByIdAsync(id);
        if (
            role!.Equals(Roles.Mechanic, StringComparison.CurrentCultureIgnoreCase)
            && userId != result.MechanicId
        )
            return Unauthorized(new ApiErrorResponse(401));
        var mappedModel = _mapper.Map<MaintenaceRequest, MaintenanceRequestResponseDto>(result);
        return Ok(new { data = mappedModel, StatusCode = StatusCodes.Status200OK });
    }
    #endregion

    #region Get All
    [Authorize(Roles = Roles.Manager)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaintenanceRequestResponseDto>>> GetAll(
        [FromQuery] MaintenanceRequestSpecParams specParams
    )
    {
        var result = await _services.GetAllAsync(specParams);
        var mappedModel = _mapper.Map<
            IReadOnlyList<MaintenaceRequest>,
            IReadOnlyList<MaintenanceRequestResponseDto>
        >(result);
        return Ok(new { data = mappedModel, StatusCode = StatusCodes.Status200OK });
    }
    #endregion

    #region Get All For User
    [Authorize(Roles = Roles.Mechanic)]
    [Route("Me")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MaintenanceRequestResponseDto>>> GetAllForUser(
        [FromQuery] MaintenanceRequestSpecParamsForMechanic specParams
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _services.GetAllForUserAsync(specParams, userId!);
        var mappedResult = _mapper.Map<
            IReadOnlyList<MaintenaceRequest>,
            IReadOnlyList<MaintenanceRequestResponseDto>
        >(result);
        return Ok(new { data = mappedResult, StatusCode = StatusCodes.Status200OK });
    }
    #endregion
}
