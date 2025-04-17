using System.Globalization;
using VMTS.Core.Entities.Trip;

namespace VMTS.Core.ServicesContract;

public interface ITripRequestService
{
    public Task<TripRequest> CreateTripRequestAsync(string managerEmail,
                                                    string driverId,
                                                    string vehicleId,
                                                    TripType tripType,
                                                    string details,
                                                    string destination);
}