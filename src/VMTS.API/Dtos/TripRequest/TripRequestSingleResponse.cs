using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos;

public class TripRequestSingleResponse
{
    public int StatusCode { get; set; }

    public TripRequestResponse TripRequest { get; set; }
}
