using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos;

public class TripRequestResponse
{
    public int StatusCode { get; set; }

    public string Id { get; set; }
    public string ManagerId { get; set; }

    public string Details { get; set; }

    public string Destination { get; set; }

    public TripType TripType { get; set; }

    public DateTime Date { get; set; }

    public DriverDto Driver { get; set; }

    public VehicleDto Vehicle { get; set; }

    public TripStatus Status { get; set; }
}
