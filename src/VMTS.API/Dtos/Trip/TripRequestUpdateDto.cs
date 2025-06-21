using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos.Trip;

public class TripRequestUpdateDto
{
    public string DriverId { get; set; }
    public string VehicleId { get; set; }
    public TripType TripType { get; set; }
    public DateTime Date { get; set; }
    public string Details { get; set; }
    public string Destination { get; set; }
    public TripStatus Status { get; set; }
}
