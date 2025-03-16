using VMTS.Core.Entities;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;



public class FaultReport : BaseEntity
{
    public string Details { get; set; }
    public DateTime ReportedAt { get; set; }
    public string Destination { get; set; }
    
    public string FaultAddress { get; set; } 
    public MaintenanceType FaultType { get; set; }
     
    public FaultComponent? FaultComponent { get; set; }
    
    public FaultReportStatus Status { get; set; } = FaultReportStatus.Reported; 

    // FKs
    public string TripId { get; set; }
    public string VehicleId { get; set; }
    public string DriverId { get; set; }  

    // Navigational Properties
    public TripRequest Trip { get; set; } 
    public Vehicle Vehicle { get; set; } 
    public BusinessUser Driver { get; set; }  
}