using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos.Trip;

public class TripRequestForVehicles
{
    public string DriverId { get; set; }

    public string VehicleId { get; set; }

    public string DriverName { get; set; }
    public TripType TripType { get; set; }

    public DateTime Date { get; set; }

    public string Details { get; set; }

    public string PickupLocation { get; set; }

    public string Destination { get; set; }

    public bool IsDaily { get; set; }
}
