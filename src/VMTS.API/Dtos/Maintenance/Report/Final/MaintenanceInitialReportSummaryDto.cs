using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Report.Final;

public class MaintenanceInitialReportSummaryDto
{
    public string Id { get; set; } = default!;
    public string Notes { get; set; } = default!;
    public decimal ExpectedCost { get; set; }
    public DateTime Date { get; set; }
    public DateOnly ExpectedFinishDate { get; set; }
    public MaintenanceCategory MaintenanceCategory { get; set; } = default!;
}
