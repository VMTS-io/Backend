using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos.Trip;

public class TripRequestUpdateDto
{
    public string DriverId { get; set; }
    public string VehicleId { get; set; }
    public TripType TripType { get; set; }
    public DateTime Date { get; set; }
    public string Details { get; set; }
    public TripStatus Status { get; set; }

    public string PickupLocation { get; set; }
    public double PickupLocationLatitude { get; set; }
    public double PickupLocationLongitude { get; set; }
    public string Destination { get; set; }
    public double DestinationLatitude { get; set; }
    public double DestinationLongitude { get; set; }
}
