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
    public FaultReportStatus Status { get; set; }
    public decimal FuelCost { get; set; }
    public int FuelRefile { get; set; }

    public string? Priority { get; set; } // enum like Low, Medium, High (based on AI)
    public string? AiPredictedFaultType { get; set; } // Store AI-predicted label if different from driver input

    public DateTime ReportedAt { get; set; }

    public bool Seen { get; set; }
}
