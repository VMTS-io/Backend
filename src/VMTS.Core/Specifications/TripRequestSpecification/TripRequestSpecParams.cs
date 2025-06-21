using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Specifications.TripRequestSpecification;

public class TripRequestSpecParams
{
    public string? ManagerId { get; set; }
    public string? TripId { get; set; }
    public string? DriverId { get; set; }
    public string? VehicleId { get; set; }
    public DateTime? Date { get; set; }
    public TripStatus? Status { get; set; }
}
