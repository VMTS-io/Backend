using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMTS.API.Dtos.Maintenance.Report;
using VMTS.API.Dtos.MaintenanceReportResponseDto;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Specifications.Maintenance.Report;

namespace VMTS.API.Controllers;

[Route("api/Maintenance/Report/")]
[Tags("Maintenance/Report/")]
public class MaintenanceReportController : BaseApiController
{
    private readonly IMechanicReportsServices _mechanicReportsServices;

    public MaintenanceReportController(IMechanicReportsServices mechanicReportsServices)
    {
        _mechanicReportsServices = mechanicReportsServices;
    }

    [ProducesResponseType(typeof(MaintenanceReportsDto), StatusCodes.Status200OK)]
    [Authorize(Roles = Roles.Manager)]
    [HttpGet("reports")]
    public async Task<ActionResult<MaintenanceReportsDto>> GetAllReports(
        [FromQuery] MaintenanceReportSpecParams specParams
    )
    {
        var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mechanicReportsServices.GetMechanicReportsAsync(managerId!, specParams);

        var initialReports = await _mechanicReportsServices.GetMechanicReportsAsync(
            managerId!,
            specParams
        );

        var mappedInitialReports = result
            .InitialReports.Select(ir => new MaintenanceReportResponseDto
            {
                Id = ir.Id,
                ReportType = "Initial",
                Notes = ir.Notes,
                ReportDate = ir.Date,
                ExpectedFinishDate = ir.ExpectedFinishDate,
                ExpectedCost = ir.ExpectedCost,
                RequestStatus = ir.MaintenanceRequest.Status,
                MechanicName = ir.Mechanic.DisplayName,
                VehicleName = ir.Vehicle.VehicleModel.Name,
                RequestTitle = ir.MaintenanceRequest.Description,
                MaintenanceCategory = ir.MaintenanceCategory,
                MissingParts = ir.MissingParts.Select(p => p.Name).ToList(),
                ExpectedChangedParts = ir
                    .ExpectedChangedParts.Select(ecp => ecp.Part.Name)
                    .ToList(),
                Seen = ir.Seen,
            })
            .ToList();

        var finalReports = await _mechanicReportsServices.GetMechanicReportsAsync(
            managerId!,
            specParams
        );
        var mappedFinalReports = result
            .FinalReports.Select(fr => new MaintenanceReportResponseDto
            {
                Id = fr.Id,
                ReportType = "Final",
                Notes = fr.Notes,
                ReportDate = fr.FinishedDate,
                TotalCost = fr.TotalCost,
                RequestStatus = fr.MaintenaceRequest.Status,
                MechanicName = fr.Mechanic.DisplayName,
                VehicleName = fr.Vehicle.VehicleModel.Name,
                RequestTitle = fr.MaintenaceRequest.Description,
                MaintenanceCategory = fr.MaintenanceCategory,

                ChangedParts = fr.ChangedParts.Select(cp => cp.Part.Name).ToList(),

                // Optional: include summary from Initial Report if available
                InitialReportSummary = fr.InitialReport?.Notes,

                Seen = fr.Seen,
            })
            .ToList();

        var respons = mappedInitialReports
            .Concat(mappedFinalReports)
            .OrderByDescending(r => r.ReportDate)
            .ToList();
        return Ok(respons);
    }
}
