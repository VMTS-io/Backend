using System.Globalization;
using VMTS.Core.Entities.Trip;

namespace VMTS.Core.ServicesContract;

public interface ITripRequestService
{
    public Task<TripRequest> CreateTripRequestAsync(string managerEmail,
                                                    string driverEmail,
                                                    string vehicleId,
                                                    TripType tripType,
                                                    string details,
                                                    string destination);
}