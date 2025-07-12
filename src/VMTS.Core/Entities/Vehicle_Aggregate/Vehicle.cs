using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Entities.Vehicle_Aggregate;

public class Vehicle : BaseEntity
{
    public string PalletNumber { get; set; } = default!;

    public DateOnly JoinedYear { get; set; }

    public FuelType FuelType { get; set; } //

    public int CurrentOdometerKM { get; set; } //

    public DateTime CreatedAt { get; set; }

    public VehicleStatus Status { get; set; } = VehicleStatus.Available;
    public DateOnly ModelYear { get; set; }
    public TransmissionType? TransmissionType { get; set; } = default!;
    public string? EngineSize { get; set; } = default!;
    public PartCondition? TireCondition { get; set; } = default!;
    public PartCondition? BrakeCondition { get; set; } = default!;
    public PartCondition? BatteryStatus { get; set; } = default!;
    public DrivingCondition? DrivingCondition { get; set; } = default!;
    public DateTime? LastAssignedDate { get; set; }
    public string? FuelEfficiency { get; set; } = default!;

    public bool NeedMaintenancePrediction { get; set; } = false;

    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? ExpectedNextMaintenanceDate { get; set; }
    public string ModelId { get; set; } = default!;
    public VehicleModel VehicleModel { get; set; } = default!;
    public ICollection<TripRequest> TripRequests { get; set; } = [];
    public ICollection<TripReport> TripReports { get; set; } = [];
    public ICollection<FaultReport> FaultReports { get; set; } = [];
    public ICollection<MaintenanceInitialReport> MaintenaceInitialReports { get; set; } = [];
    public ICollection<MaintenanceFinalReport> MaintenaceFinalReports { get; set; } = [];
    public ICollection<MaintenaceRequest> MaintenaceRequests { get; set; } = [];
    public ICollection<MaintenanceTracking> MaintenanceTrackings { get; set; } = default!;
    public ICollection<MaintenanceFinalReportParts> MaintenanceFinalReportParts { get; set; } =
        default!;

    public int? NeedMaintenanceInDays { get; set; }
}
