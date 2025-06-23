namespace VMTS.API.Dtos;

public class TripRequestListResponse
{
    public int StatusCode { get; set; }
    public List<TripRequestObj> TripRequests { get; set; }
}
