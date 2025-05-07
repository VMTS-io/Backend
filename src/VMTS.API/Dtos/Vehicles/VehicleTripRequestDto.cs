using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos.Vehicles;

public class VehicleTripRequestDto
{
    public TripType Type { get; set; }
    public string Details { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public DateTime Date { get; set; }
    public TripStatus Status { get; set; }
    public DateTime ModelYear { get; set; }
}
