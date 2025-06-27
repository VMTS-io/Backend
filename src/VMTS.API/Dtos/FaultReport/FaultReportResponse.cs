using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;

namespace VMTS.API.Dtos;

public class FaultReportResponse
{
    public string Id { get; set; }
    public DriverDto Driver { get; set; }
    public VehicleDto Vehicle { get; set; }
    public TripDto Trip { get; set; }

    public string FaultAddress { get; set; }
    public string Details { get; set; }
    public MaintenanceType FaultType { get; set; }
    public FaultReportStatus Status { get; set; }
    public decimal Cost { get; set; }
    public int FuelRefile { get; set; }
    public DateTime CreatedAt { get; set; }

    public bool Seen { get; set; }
}
