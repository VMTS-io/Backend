using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Vehicles;

public class ManagerVehicleDto
{
    public string Id { get; set; } = default!;
    public string Model { get; set; } = default!;
    public string PalletNumber { get; set; } = default!;
    public VehicleStatus Status { get; set; }
    public DateTime LastAssignedDate { get; set; }
    public string Category { get; set; } = default!;
}
