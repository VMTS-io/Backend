using Microsoft.AspNetCore.Mvc;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.ServicesContract;

namespace VMTS.API.Controllers;

public class DashboardController : BaseApiController
{
    private readonly IDashboardServices _dashboardServices;
    private readonly IFaultReportService _faultReportService;

    public DashboardController(
        IDashboardServices dashboardServices,
        IFaultReportService faultReportService
    )
    {
        _dashboardServices = dashboardServices;
        _faultReportService = faultReportService;
    }

    [HttpGet("Trips-With-Faults")]
    public async Task<ActionResult<int>> GetTripsWithFaults(int? month)
    {
        var trips = await _dashboardServices.GetCountTripsWithFaultsAsync(month);
        return Ok(trips);
    }

    [HttpGet("Trips-Without-Faults")]
    public async Task<ActionResult<int>> GetTripsWithoutFaults(int? month)
    {
        var trips = await _dashboardServices.GetCountTripsWithoutFaultsAsync(month);
        return Ok(trips);
    }

    [HttpGet("Toatl-Trips")]
    public async Task<ActionResult<int>> GetTotalTrips(int? month)
    {
        var trips = await _dashboardServices.GetTotalTripsAsync(month);
        return Ok(trips);
    }

    [HttpGet("Total-Maintenance-Cost")]
    public async Task<ActionResult<decimal>> GetTotalMaintenanceCost([FromQuery] int? month)
    {
        var cost = await _dashboardServices.GetTotalMaintenanceCostAsync(month);
        return Ok(cost);
    }

    [HttpGet("Total-Fuel-Cost")]
    public async Task<ActionResult<decimal>> GetTotalFuelCost(int? month)
    {
        var cost = await _dashboardServices.GetTotalFuelCostAsync(month);
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

    [HttpGet("priority-chart")]
    public async Task<IActionResult> GetPriorityChart()
    {
        var chartDto = await _faultReportService.GetPriorityChartAsync();

        return File(chartDto.ChartBase64, "image/jpeg", "priority-chart.jpg");
    }

    [HttpGet("time-series-costs-chart")]
    public async Task<IActionResult> GetTimeSeriesCostsChart()
    {
        var chartDto = await _dashboardServices.GetTimeSeriesCostChartAsync();
        return File(chartDto.ChartBytes, "image/jpeg", "monthly-costs-chart.jpg");
    }
}
