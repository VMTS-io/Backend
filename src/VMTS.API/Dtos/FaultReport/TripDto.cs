using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos;

public class TripDto
{
    public string Id { get; set; }
    public string Destination { get; set; }
    public TripType Type { get; set; }
}
