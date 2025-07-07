using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleUpsertDto
{
    public string PalletNumber { get; set; } = default!;

    public short JoinedYear { get; set; }

    public FuelType FuelType { get; set; }

    public int KMDriven { get; set; }

    public VehicleStatus Status { get; set; }

    public string ModelId { get; set; } = default!;

    public short ModelYear { get; set; }
    public string? TransmissionType { get; set; } = default!;
    public string? EngineSize { get; set; } = default!;
    public string? TireCondition { get; set; } = default!;
    public string? BrakeCondition { get; set; } = default!;
    public string? BatteryStatus { get; set; } = default!;
    public DrivingCondition? DrivingCondition { get; set; } = default!;
    public DateTime? LastAssignedDate { get; set; }

    public DateTime? LastMaintenanceDate { get; set; }
    // public string CategoryId { get; set; }=default!;
}
