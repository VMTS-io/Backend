using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos;

public class TripRequestDto
{
    public string Id { get; set; }

    public string DriverEmail { get; set; }

    public string VehicleId { get; set; }

    public TripType TripType { get; set; }

    public string Details { get; set; }

    public string Destination { get; set; }
}

