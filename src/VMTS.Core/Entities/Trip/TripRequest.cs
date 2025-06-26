using VMTS.Core.Entities;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

public class TripRequest : BaseEntity
{
    public TripRequest() { }

    public TripRequest(
        TripType type,
        string details,
        string destination,
        TripStatus status,
        string driverId,
        string managerId,
        string vehicleId
    )
    {
        Type = type;
        Details = details;
        Destination = destination;
        Status = status;
        DriverId = driverId;
        ManagerId = managerId;
        VehicleId = vehicleId;
    }

    public TripType Type { get; set; }
    public string Details { get; set; }

    public string PickupLocation { get; set; }

    public string PickupLocationNominatimLink { get; set; }
    public double PickupLocationLatitude { get; set; }
    public double PickupLocationLongitude { get; set; }

    public string Destination { get; set; }

    public string DestinationLocationNominatimLink { get; set; }
    public double DestinationLatitude { get; set; }
    public double DestinationLongitude { get; set; }
    public DateTime Date { get; set; }
    public TripStatus Status { get; set; }

    public FaultReport FaultReports { get; set; }

    public TripReport TripReports { get; set; }

    // Foreign Key for Vehicle
    public string VehicleId { get; set; }

    // Navigation Property
    public Vehicle? Vehicle { get; set; }

    public string DriverId { get; set; }
    public BusinessUser? Driver { get; set; }

    public string ManagerId { get; set; }
    public BusinessUser? Manager { get; set; }
}
