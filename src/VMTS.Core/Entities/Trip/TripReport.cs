using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Trip;

public class TripReport : BaseEntity
{
    public TripReport(
        string driverId,
        string vehicleId,
        string tripId,
        int fuelRefile,
        decimal cost,
        string destination,
        string details
    )
    {
        DriverId = driverId;
        VehicleId = vehicleId;
        TripId = tripId;
        FuelRefile = fuelRefile;
        FuelCost = cost;
        Destination = destination;
        Details = details;
        ReportedAt = DateTime.UtcNow;
    }

    public TripReport() { }

    public string Destination { get; set; }

    public string Details { get; set; }

    public DateTime ReportedAt { get; set; }
    public decimal FuelCost { get; set; }

    public int FuelRefile { get; set; }

    public TripReportStatus Status { get; set; } = TripReportStatus.Reported;

    public bool Seen { get; set; } = false;

    // FKs
    public string TripId { get; set; }
    public string VehicleId { get; set; }
    public string DriverId { get; set; }

    // Navigational Properties
    public TripRequest Trip { get; set; }
    public Vehicle Vehicle { get; set; }
    public BusinessUser? Driver { get; set; }
}
