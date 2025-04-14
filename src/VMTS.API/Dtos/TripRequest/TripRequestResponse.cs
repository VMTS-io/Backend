using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos;

public class TripRequestResponse
{
    public string ManagerId { get; set; }
    public string Details { get; set; }

    public string Destination { get; set; }

    public TripType TripType { get; set; }
    public DateTime Date { get; set; }
    public string DriverId { get; set; }
    public string VehicleId { get; set; }

    public TripStatus TripStatus { get; set; }
    
    
}