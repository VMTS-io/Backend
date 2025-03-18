using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Entities.User_Business;

public class BusinessUser : BaseEntity
{
    public string DisplayName { get; set; }

    public string Email { get; set; }

    public string NormalizedEmail { get; set; }

    public string PhoneNumber { get; set; }

    public ICollection<TripRequest?> ManagerTripRequest { get; set; } = new HashSet<TripRequest?>();

    public ICollection<TripRequest?> DriverTripRequest { get; set; } = new HashSet<TripRequest?>();

    public ICollection<TripReport?> DriverTripReport { get; set; } = new HashSet<TripReport>();

    public ICollection<FaultReport?> DriverFaultReport { get; set; } = new HashSet<FaultReport>();

    public ICollection<MaintenaceReport?> MechanicMaintenaceReports { get; set; } =
        new HashSet<MaintenaceReport>();

    public ICollection<MaintenaceRequest?> ManagerMaintenaceRequests { get; set; } =
        new HashSet<MaintenaceRequest>();
    public ICollection<MaintenaceRequest?> MechanicMaintenaceRequests { get; set; } =
        new HashSet<MaintenaceRequest>();
}

