using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Maintenance.Report;
using VMTS.API.Dtos.Maintenance.Report.Final;
using VMTS.API.Dtos.Maintenance.Report.Initial;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Specifications.Maintenance.Report;

namespace VMTS.API.Controllers;

[Route("api/Maintenance/Report/")]
[Tags("Maintenance/Report/")]
public class MaintenanceReportController : BaseApiController
{
    private readonly IMaintenanceInitialReportServices _initialService;
    private readonly IMaintenanceFinalReportServices _finalService;
    private readonly IMapper _mapper;

    public MaintenanceReportController(
        IMapper mapper,
        IMaintenanceInitialReportServices initialService,
        IMaintenanceFinalReportServices finalService
    )
    {
        _mapper = mapper;
        _initialService = initialService;
        _finalService = finalService;
    }

    [ProducesResponseType(typeof(MaintenanceReportsDto), StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<MaintenanceReportsDto>> GetAllReports(
        [FromQuery] MaintenanceReportSpecParams specParams
    )
    {
        var initialReports = await _initialService.GetAllInitialReportsAsync(specParams);
        var mappedInitialReprots = _mapper.Map<IReadOnlyList<MaintenanceInitialReportResponseDto>>(
            initialReports
        );
        var finalReports = await _finalService.GetAllFinalReportsAsync(specParams);
        var mappedFinalReprots = _mapper.Map<IReadOnlyList<MaintenanceFinalReportResponseDto>>(
            finalReports
        );
        var respons = new MaintenanceReportsDto()
        {
            InitialReports = mappedInitialReprots,
            FinalReports = mappedFinalReprots,
        };
        return Ok(respons);
    }
}
