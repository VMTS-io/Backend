using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Request;

public class MaintenanceReportDto
{
    public string Id { get; set; }

    public string Description { get; set; }

    public decimal Cost { get; set; }

    public DateTime Date { get; set; }

    public Status Status { get; set; }
}
