using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;

namespace VMTS.API.Dtos;

public class FaultReportResponse
{
    public string DriverId { get; set; } 
    public string TripId { get; set; }
    public string Destination { get; set; }
    public string FaultAddress { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public MaintenanceType FaultType { get; set; }  
    public FaultComponent? FaultComponent { get; set; }
    public FaultReportStatus Status { get; set; } = FaultReportStatus.Reported;
    public DateTime CreatedAt { get; set; }
    
    
    
}