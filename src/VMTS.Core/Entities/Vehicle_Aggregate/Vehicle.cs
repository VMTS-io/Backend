using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Entities.Vehicle_Aggregate;

public class Vehicle : BaseEntity
{
    public string PalletNumber { get; set; } = default!;

    public DateOnly JoindYear { get; set; }

    public FuelType FuelType { get; set; }

    public int KMDriven { get; set; }

    public VehicleStatus Status { get; set; } = VehicleStatus.Active;

    public DateOnly? ModelYear { get; set; }

    public DateTime? LastAssignedDate { get; set; } = DateTime.UtcNow;

    public string ModelId { get; set; } = default!;
    public VehicleModel VehicleModel { get; set; } = default!;

    public ICollection<TripRequest> TripRequests { get; set; } = [];
    public ICollection<TripReport> TripReports { get; set; } = [];
    public ICollection<MaintenaceReport> MaintenaceReports { get; set; } = [];
    public ICollection<MaintenaceRequest> MaintenaceRequests { get; set; } = [];
}
