using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Entities.User_Business;

public class BusinessUser : BaseEntity
{
    public string DisplayName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string NormalizedEmail { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string? Role { get; set; } = default!;

    public ICollection<TripRequest> ManagerTripRequest { get; set; } = [];

    public ICollection<TripRequest> DriverTripRequest { get; set; } = [];

    public ICollection<TripReport> DriverTripReport { get; set; } = [];

    public ICollection<FaultReport> DriverFaultReport { get; set; } = [];

    public ICollection<MaintenanceInitialReport> MechanicMaintenaceInitialReports { get; set; } =
        [];
    public ICollection<MaintenanceFinalReport> MechanicMaintenaceFinalReports { get; set; } = [];

    // public ICollection<MaintenaceReport> MechanicMaintenaceReports { get; set; } = [];

    public ICollection<MaintenaceRequest> ManagerMaintenaceRequests { get; set; } = [];
    public ICollection<MaintenaceRequest> MechanicMaintenaceRequests { get; set; } = [];
}
