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
    [Authorize(Roles = Roles.Manager)]
    [HttpGet("reports")]
    public async Task<ActionResult<DriverReportsResponse>> GetAll(
        [FromQuery] DriverReportsSpecParams spec
    )
    {
        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _driverReportsService.GetDriverReportsAsync(managerId, spec);

        var response = new DriverReportsResponse
        {
            TripReports = _mapper.Map<IReadOnlyList<TripReportResponse>>(result.TripReports),
            FaultReports = _mapper.Map<IReadOnlyList<FaultReportResponse>>(result.FaultReports),
        };

        return Ok(response);
    }

    #endregion
}
