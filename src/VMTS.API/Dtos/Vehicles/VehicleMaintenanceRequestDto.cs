using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleMaintenanceRequestDto
{
    public string Id { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public Status Status { get; set; } = Status.Pending;

    public DateTime? ModelYear { get; set; }
}
