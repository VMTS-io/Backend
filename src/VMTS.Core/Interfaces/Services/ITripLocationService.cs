using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Interfaces.Services;

public interface ITripLocationService
{
    Task SetLocationAsync(TripLocation model);

    Task<TripLocation?> GetLocationAsync(string tripId);
}
