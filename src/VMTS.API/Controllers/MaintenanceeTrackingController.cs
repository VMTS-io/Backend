using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.ActionFilters;
using VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Controllers;

public class MaintenanceeTrackingController : BaseApiController
{
    private readonly IMaintenanceTrackingService _maintenanceTrackingService;
    private readonly IMapper _mapper;

    public MaintenanceeTrackingController(
        IMaintenanceTrackingService maintenanceTrackingService,
        IMapper mapper
    )
    {
        _maintenanceTrackingService = maintenanceTrackingService;
        _mapper = mapper;
        ;
    }

    [HttpGet("vehicle-with-due-parts")]
    public async Task<ActionResult<IReadOnlyList<VehicleWithDuePartsDto>>> GetDueVehicles(
        [FromQuery] VehicleWithDuePartsSpecParams specParams
    )
    {
        var result = await _maintenanceTrackingService.GetVehiclesWithDuePartsAsync(specParams);
        var mappedResult = _mapper.Map<IReadOnlyList<VehicleWithDuePartsDto>>(result);
        return Ok(mappedResult);
    }

    [ServiceFilter<ValidateModelActionFilter<VehicleWithDuePartsSpecParams>>]
    [HttpGet("MaintenanceTrackings")]
    public async Task<VehicleTrackingDto> GetVehicleTraching(
        [FromQuery] VehicleWithDuePartsSpecParams specParams
    )
    {
        var result = await _maintenanceTrackingService.GetVehiclesPartsAsync(specParams);
        var mappedResult = _mapper.Map<IReadOnlyList<VehicleTrackingDto>>(result).FirstOrDefault();
        return mappedResult;
    }

    [HttpPost("recalculate")]
    public async Task<IActionResult> RecalculateAll()
    {
        await _maintenanceTrackingService.RecalculateAllAsync();
        return Ok(new { Message = "Recalculation completed successfully." });
    }
}
