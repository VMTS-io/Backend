using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Report;

public class MaintenanceRequestForReportDto
{
    public string Id { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime Date { get; set; }

    public MaintenanceStatus Status { get; set; }
}
