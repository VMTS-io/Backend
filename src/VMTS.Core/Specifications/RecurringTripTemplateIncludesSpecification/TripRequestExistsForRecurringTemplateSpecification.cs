using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Specifications.RecurringTripTemplateIncludesSpecification;

public class TripRequestExistsForRecurringTemplateSpecification : BaseSpecification<TripRequest>
{
    public TripRequestExistsForRecurringTemplateSpecification(
        string driverId,
        string vehicleId,
        DateTime date
    )
        : base(t =>
            t.DriverId == driverId
            && t.VehicleId == vehicleId
            && t.Date == date
            && t.IsDaily
            && t.Status != TripStatus.Canceled
        ) { }
}
