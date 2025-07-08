using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos;
using VMTS.API.Dtos.DriverReportsResponse;
using VMTS.API.Dtos.TripReport;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.ServicesContract;
using VMTS.Core.Specifications;
using VMTS.Core.Specifications.DriverReports;
using VMTS.Core.Specifications.FaultReportSepcification;

namespace VMTS.API.Controllers;

public class DriverReportController : BaseApiController
{
    private readonly IDriverReportsService _driverReportsService;
    private readonly IMapper _mapper;

    public DriverReportController(IDriverReportsService driverReportsService, IMapper mapper)
    {
        _driverReportsService = driverReportsService;
        _mapper = mapper;
    }

    #region GetAll
    [Authorize(Roles = $"{Roles.Manager},{Roles.Driver}")]
    [ProducesResponseType(typeof(DriverReportsResponse), StatusCodes.Status200OK)]
    [HttpGet("reports")]
    public async Task<ActionResult<DriverReportsResponse>> GetAll(
        [FromQuery] DriverReportsSpecParams spec
    )
    {
        var role = User.FindFirstValue(ClaimTypes.Role);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (role!.Equals(Roles.Driver, StringComparison.CurrentCultureIgnoreCase))
            spec.DriverId = userId;

        var result = await _driverReportsService.GetDriverReportsAsync(spec);

        var mappedtrips = result.TripReports.Select(tr => new DriverReportItemDto
        {
            Id = tr.Id,
            ReportType = "Trip",
            Driver = _mapper.Map<DriverDto>(tr.Driver),
            Vehicle = _mapper.Map<VehicleDto>(tr.Vehicle),
            ReportedAt = tr.ReportedAt,
            Destination = tr.Destination,
            FuelCost = tr.FuelCost,
            Status = tr.Trip.Status,
            Seen = tr.Seen,
        });
        var mappedfaults = result.FaultReports.Select(fr => new DriverReportItemDto
        {
            Id = fr.Id,
            ReportType = "Fault",
            Driver = _mapper.Map<DriverDto>(fr.Driver),
            Vehicle = _mapper.Map<VehicleDto>(fr.Vehicle),
            ReportedAt = fr.ReportedAt,
            Destination = fr.Trip.Destination,
            FaultAddress = fr.FaultAddress,
            FaultDetails = fr.Details,
            FaultType = fr.AiPredictedFaultType,
            Cost = fr.Cost,
            Status = fr.Trip.Status,
            Seen = fr.Seen,
        });

        var response = mappedtrips
            .Concat(mappedfaults)
            .OrderByDescending(r => r.ReportedAt)
            .ToList();

        return Ok(response);
    }

    #endregion
}
