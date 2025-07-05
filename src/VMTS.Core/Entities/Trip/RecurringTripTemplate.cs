using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Entities.Trip
{
    public class RecurringTripTemplate : BaseEntity
    {
        public TripType Type { get; set; }
        public string Details { get; set; }

        public string PickupLocation { get; set; }
        public string PickupLocationNominatimLink { get; set; }
        public double PickupLocationLatitude { get; set; }
        public double PickupLocationLongitude { get; set; }

        public string Destination { get; set; }
        public string DestinationLocationNominatimLink { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }

        public string VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public string DriverId { get; set; }
        public BusinessUser? Driver { get; set; }

        public string ManagerId { get; set; }
        public BusinessUser? Manager { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
