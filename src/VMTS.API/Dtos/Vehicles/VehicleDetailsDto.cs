using VMTS.API.Dtos.Maintenance.Report.Final;
using VMTS.API.Dtos.Trip;
using VMTS.API.Dtos.Vehicles.Model;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleDetailsDto
{
    public string PalletNumber { get; set; } = default!;

    public DateOnly JoinedYear { get; set; }

    public short ModelYear { get; set; }

    public decimal TotalMaintenanceCost { get; set; }

    public decimal ToatalFuleCost { get; set; }

    public FuelType FuelType { get; set; }

    public int KMDriven { get; set; }

    public VehicleStatus Status { get; set; }

    public VehicleModelDto VehicleModel { get; set; } = default!;

    public TransmissionType? TransmissionType { get; set; } = default!;
    public string? EngineSize { get; set; } = default!;
    public PartCondition? TireCondition { get; set; } = default!;
    public PartCondition? BrakeCondition { get; set; } = default!;
    public PartCondition? BatteryStatus { get; set; } = default!;
    public DrivingCondition? DrivingCondition { get; set; } = default!;
    public DateTime? LastAssignedDate { get; set; }
    public string? FuelEfficiency { get; set; } = default!;

    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? ExpectedNextMaintenanceDate { get; set; }
    public string ModelId { get; set; } = default!;

    // public VehicleCategoryDto VehicleCategory { get; set; }

    public ICollection<TripRequestForVehicles> TripRequests { get; set; } = [];

    public ICollection<TripReportDto> TripReports { get; set; } = [];

    // public ICollection<MaintenanceReportDto> MaintenaceReports { get; set; } = [];
    // public ICollection<MaintenanceFinalReportSummaryDto> MaintenaceFinalReport { get; set; } = [];
    // public ICollection<MaintenanceInitialReportSummaryDto> MaintenaceInitialReports { get; set; } =
    //     [];

    public ICollection<VehicleMaintenanceRequestDto> MaintenaceRequests { get; set; } = [];
}
