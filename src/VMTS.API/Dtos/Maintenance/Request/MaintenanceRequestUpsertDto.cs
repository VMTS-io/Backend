using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Request;

public class MaintenanceRequestUpsertDto
{
    public string MechanicId { get; set; } = default!;

    public string VehicleId { get; set; } = default!;

    public MaintenanceCategory MaintenanceCategory { get; set; } = default!;

    public List<string> Parts { get; set; } = [];

    public string Description { get; set; } = default!;
    public string? falutReportId { get; set; } = default!;
}
