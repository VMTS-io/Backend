using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Specifications.TripRequestSpecification;

namespace VMTS.Core.ServicesContract;

public interface ITripRequestService
{
    public Task<TripRequest> CreateTripRequestAsync(
        string managerId,
        string driverId,
        string vehicleId,
        TripType tripType,
        DateTime date,
        string details,
        string destination
    );

    Task UpdateTripRequestAsync(
        string tripId,
        string managerId,
        string driverId,
        string vehicleId,
        string destination,
        string details,
        DateTime date,
        TripType tripType,
        TripStatus status
    );

    Task DeleteTripRequestAsync(string tripId, string managerId);

    Task<TripRequest> GetTripRequestByIdAsync(string id);
    Task<IReadOnlyList<TripRequest>> GetAllTripRequestsAsync(TripRequestSpecParams specParams);

    Task<IReadOnlyList<TripRequest>> GetAllTripsForUserAsync(TripRequestSpecParams specParams);
}
