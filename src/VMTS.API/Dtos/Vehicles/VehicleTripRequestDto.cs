using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleTripRequestDto
{
    public TripType Type { get; set; }
    public string Details { get; set; }
    public string Destination { get; set; }
    public DateTime Date { get; set; }
    public TripStatus Status { get; set; }
}
