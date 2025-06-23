using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos;

public class TripRequestResponse
{
    public int StatusCode { get; set; }

    public TripRequestObj TripRequest { get; set; }
}
