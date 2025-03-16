using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Trip;

public class TripRequest : BaseEntity
{
    public TripType Type { get; set; }
    public string Details { get; set; }
    public string Destination { get; set; }
    public DateTime Date { get; set; }
    public TripStatus Status { get; set; }
    
    
    public ICollection<FaultReport> FaultReports { get; set; } = new HashSet<FaultReport>();


  

    public Vehicle Vehicle { get; set; }
    
    public string DriverId { get; set; } 
    public BusinessUser? Driver { get; set; }
    
    public string ManagerId { get; set; } 
    public BusinessUser? Manager { get; set; }
    
}