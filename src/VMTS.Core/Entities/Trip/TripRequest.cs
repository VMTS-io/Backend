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
    public string Destination { get; set; }
    public DateTime Date { get; set; }
    public TripStatus Status { get; set; }

    public FaultReport FaultReports { get; set; }

    // Foreign Key for Vehicle
    public string VehicleId { get; set; }

    // Navigation Property
    public Vehicle? Vehicle { get; set; }

    public string DriverId { get; set; }
    public BusinessUser? Driver { get; set; }

    public string ManagerId { get; set; }
    public BusinessUser? Manager { get; set; }
}
