namespace VMTS.API.Dtos;

public class TripRequestListResponse
{
    public int StatusCode { get; set; }
    public List<TripRequestResponse> TripRequests { get; set; }
}
