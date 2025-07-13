using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleListDto
{
    public DateTime CreatedAt { get; set; }
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string PalletNumber { get; set; } = default!;

    public DateOnly JoinedYear { get; set; }

    public short ModelYear { get; set; }

    public string? FuelEfficiency { get; set; } = default!;

    public int CurrentOdometerKM { get; set; } //

    public VehicleStatus Status { get; set; }
    public DateTime LastAssignedDate { get; set; }
    public string Category { get; set; } = default!;
}
