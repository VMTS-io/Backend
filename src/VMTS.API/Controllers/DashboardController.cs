using Microsoft.AspNetCore.Mvc;
using VMTS.Core.Interfaces.Services;

namespace VMTS.API.Controllers;

public class DashboardController : BaseApiController
{
    private readonly IDashboardServices _dashboardServices;

    public DashboardController(IDashboardServices dashboardServices)
    {
        _dashboardServices = dashboardServices;
    }

    [HttpGet("Trips-With-Faults")]
    public async Task<ActionResult<int>> GetTripsWithFaults()
    {
        var trips = await _dashboardServices.GetCountTripsWithFaultsAsync();
        return Ok(trips);
    }

    [HttpGet("Trips-Without-Faults")]
    public async Task<ActionResult<int>> GetTripsWithoutFaults()
    {
        var trips = await _dashboardServices.GetCountTripsWithoutFaultsAsync();
        return Ok(trips);
    }

    [HttpGet("Total-Maintenance-Cost")]
    public async Task<ActionResult<decimal>> GetTotalMaintenanceCost()
    {
        var cost = await _dashboardServices.GetTotalMaintenanceCostAsync();
        return Ok(cost);
    }

    [HttpGet("Total-Fuel-Cost")]
    public async Task<ActionResult<decimal>> GetTotalFuelCost()
    {
        var cost = await _dashboardServices.GetTotalFuelCostAsync();
        return Ok(cost);
    }

    [HttpGet("Total-Vehicle")]
    public async Task<ActionResult<decimal>> GetTotalVehicle()
    {
        var cost = await _dashboardServices.GetTotalVehicleCount();
        return Ok(cost);
    }

    [HttpGet("Total-Vehicle-UnderMaintencance")]
    public async Task<ActionResult<decimal>> GetTotalVehicleUnderMaintenance()
    {
        var cost = await _dashboardServices.GetVehicleUnderMaintenanceCount();
        return Ok(cost);
    }

    [HttpGet("Total-Vehicle-Available")]
    public async Task<ActionResult<decimal>> GetAvailable()
    {
        var cost = await _dashboardServices.GetVehicleAvailableCount();
        return Ok(cost);
    }
}
