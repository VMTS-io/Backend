using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Entities.Vehicle_Aggregate;

public class Vehicle : BaseEntity
{
    public string PalletNumber { get; set; } = default!;

    public DateOnly JoinedYear { get; set; }

    public FuelType FuelType { get; set; } //

    public int CurrentOdometerKM { get; set; } //

    public VehicleStatus Status { get; set; } = VehicleStatus.Available;
    public DateOnly ModelYear { get; set; }
    public string? TransmissionType { get; set; } = default!;
    public string? EngineSize { get; set; } = default!;
    public string? TireCondition { get; set; } = default!;
    public string? BrakeCondition { get; set; } = default!;
    public string? BatteryStatus { get; set; } = default!;
    public DrivingCondition? DrivingCondition { get; set; } = default!;
    public DateTime? LastAssignedDate { get; set; }

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
