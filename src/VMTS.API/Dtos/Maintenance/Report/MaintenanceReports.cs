using VMTS.API.Dtos.Maintenance.Report.Final;
using VMTS.API.Dtos.Maintenance.Report.Initial;

namespace VMTS.API.Dtos.Maintenance.Report;

public class MaintenanceReportsDto
{
    public IReadOnlyList<MaintenanceInitialReportResponseDto> InitialReports { get; set; } =
        default!;
    public IReadOnlyList<MaintenanceFinalReportResponseDto> FinalReports { get; set; } = default!;
}
