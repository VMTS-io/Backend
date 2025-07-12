using VMTS.Core.Entities;
using VMTS.Core.Entities.Report;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

public class FaultReport : BaseEntity
{
    public string Details { get; set; } = default!;
    public DateTime ReportedAt { get; set; }
    public string Destination { get; set; } = default!;

    public decimal Cost { get; set; }

    public int FuelRefile { get; set; }
    public string FaultAddress { get; set; } = default!;

    public FaultReportStatus Status { get; set; } = FaultReportStatus.Reported;

    public bool Seen { get; set; } = false;

    public string? Priority { get; set; }
    public string? AiPredictedFaultType { get; set; } // Store AI-predicted label if different from driver input

    public bool? IsAiPredictionSuccessful { get; set; }

    // FKs
    public string TripId { get; set; } = default!;
    public string VehicleId { get; set; } = default!;
    public string DriverId { get; set; } = default!;
    public bool SentToMechanic { get; set; } = false;

    // Navigational Properties
    public TripRequest Trip { get; set; } = default!;
    public Vehicle Vehicle { get; set; } = default!;
    public BusinessUser? Driver { get; set; }
}
