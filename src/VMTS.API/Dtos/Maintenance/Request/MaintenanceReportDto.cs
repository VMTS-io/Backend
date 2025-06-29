using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Request;

public class MaintenanceReportDto
{
    public string Id { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Cost { get; set; }

    public DateTime Date { get; set; }

    public MaintenanceStatus Status { get; set; }
}
