using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Entities.Vehicle_Aggregate;

public class Vehicle : BaseEntity
{
    public string Name { get; set; }

    public string PalletNumber { get; set; }

    public DateTime JoindYear { get; set; }

    public FuelType FuelType { get; set; }

    public int KMDriven { get; set; }

    public VehicleStatus Status { get; set; }


    public string ModelId { get; set; }
    public string CategoryId { get; set; }

    
    public VehicleModel VehicleModel { get; set; }
    public VehicleCategory VehicleCategory { get; set; }
    
    public ICollection<TripRequest> TripRequests { get; set; } = new HashSet<TripRequest>();
    public ICollection<TripReport> TripReports { get; set; } = new HashSet<TripReport>();
    public ICollection<MaintenaceReport> MaintenaceReports { get; set; }
    public ICollection<MaintenaceRequest> MaintenaceRequests { get; set; }
    
}