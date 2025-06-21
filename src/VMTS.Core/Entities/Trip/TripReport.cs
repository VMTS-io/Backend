using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Trip;

public class TripReport : BaseEntity
{
    public string Duration { get; set; }

    public string Details { get; set; }

    public DateTime ReportedAt { get; set; }
    public decimal FuelCost { get; set; }

    public int FuelRefile { get; set; }

    public TripReportStatus Status { get; set; } = TripReportStatus.Reported;

    // FKs
    public string TripId { get; set; }
    public string VehicleId { get; set; }
    public string DriverId { get; set; }

    // Navigational Properties
    public TripRequest Trip { get; set; }
    public Vehicle Vehicle { get; set; }
    public BusinessUser? Driver { get; set; }
}
