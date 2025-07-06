using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleMaintenanceRequestDto
{
    public string Id { get; set; } = default!;

    public string MechanicName { get; set; }

    public string Description { get; set; } = default!;

    public DateTime Date { get; set; }

    public MaintenanceStatus Status { get; set; }

    public DateTime ModelYear { get; set; }
}
