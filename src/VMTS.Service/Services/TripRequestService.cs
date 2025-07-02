using Microsoft.AspNetCore.Identity;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Helpers;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.ServicesContract;
using VMTS.Core.Specifications.TripRequestSpecification;
using VMTS.Core.Specifications.VehicleSpecification;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class TripRequestService : ITripRequestService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public TripRequestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #region create

    public async Task<TripRequest> CreateTripRequestAsync(
        string managerId,
        string driverId,
        string vehicleId,
        TripType tripType,
        DateTime date,
        string details,
        string pickupLocation,
        string destination
    )
    {
        /// Get manager
        var manager = await _unitOfWork.GetRepo<BusinessUser>().GetByIdAsync(managerId);
        if (manager is null)
            throw new NotFoundException("Unable to find manager");

        // Get driver
        var driver = await _unitOfWork.GetRepo<BusinessUser>().GetByIdAsync(driverId);
        if (driver is null)
            throw new NotFoundException("Unable to find driver");
        if (driver.Role != Roles.Driver)
            throw new InvalidOperationException("You must pick a driver");

        // Get vehicle
        var vehicleSpec = new VehicleIncludesSpecification(vehicleId);
        var vehicle = await _unitOfWork
            .GetRepo<Vehicle>()
            .GetByIdWithSpecificationAsync(vehicleSpec);
        if (vehicle is null)
            throw new NotFoundException("Vehicle not found");

        var vehicleUnderMaintenance = await _unitOfWork.GetRepo<Vehicle>().GetByIdAsync(vehicleId);

        if (vehicleUnderMaintenance.Status == VehicleStatus.UnderMaintenance)
            throw new InvalidOperationException("Vehicle is under maintenance");

        if (vehicleUnderMaintenance.Status == VehicleStatus.OnTrip)
            throw new InvalidOperationException("Vehicle is On a Trip right now");

        if (vehicleUnderMaintenance.Status == VehicleStatus.OutOfService)
            throw new InvalidOperationException("Vehicle is out of serivce right now");

        if (vehicleUnderMaintenance.Status == VehicleStatus.Retired)
            throw new InvalidOperationException("Vehicle is retired xxx");

        // Check for past date
        if (date.Date < DateTime.UtcNow.Date)
            throw new ArgumentException("Trip date cannot be in the past.");

        // Check driver availability
        var driverAvailabilitySpec = new TripRequestIncludesSpecification(
            new TripRequestSpecParams
            {
                DriverId = driverId,
                Date = date,
                Status = new[] { TripStatus.Approved, TripStatus.Pending },
            }
        );
        var driverTrips = await _unitOfWork
            .GetRepo<TripRequest>()
            .GetAllWithSpecificationAsync(driverAvailabilitySpec);
        if (driverTrips.Any())
            throw new InvalidOperationException("Driver already has a trip on this date.");

        // Check vehicle availability
        var vehicleAvailabilitySpec = new TripRequestIncludesSpecification(
            new TripRequestSpecParams
            {
                VehicleId = vehicleId,
                Date = date,
                Status = new[] { TripStatus.Approved, TripStatus.Pending },
            }
        );
        var vehicleTrips = await _unitOfWork
            .GetRepo<TripRequest>()
            .GetAllWithSpecificationAsync(vehicleAvailabilitySpec);

        if (vehicleTrips.Any())
            throw new InvalidOperationException("Vehicle already on a trip on this date");

        var tripRequest = new TripRequest
        {
            Id = Guid.NewGuid().ToString(),
            Type = tripType,
            Details = details,
            Date = date,
            Status = TripStatus.Pending,
            DriverId = driverId,
            ManagerId = managerId,
            VehicleId = vehicleId,
            PickupLocation = pickupLocation,
            PickupLocationNominatimLink = GenerateNominatimLink(pickupLocation),
            Destination = destination,
            DestinationLocationNominatimLink = GenerateNominatimLink(destination),
        };

        await _unitOfWork.GetRepo<TripRequest>().CreateAsync(tripRequest);

        var result = await _unitOfWork.SaveChanges();
        if (result <= 0)
            throw new Exception("Failed to create trip request.");

        // Load with includes
        var specs = new TripRequestIncludesSpecification(
            new TripRequestSpecParams { TripId = tripRequest.Id }
        );
        var fullTrip = await _unitOfWork
            .GetRepo<TripRequest>()
            .GetByIdWithSpecificationAsync(specs);

        return fullTrip!;
    }

    #endregion

    #region Update

    public async Task UpdateTripRequestAsync(
        string tripId,
        string managerId,
        string driverId,
        string vehicleId,
        string details,
        DateTime date,
        TripType tripType,
        TripStatus status,
        string pickupLocation,
        string destination
    )
    {
        var trip = await _unitOfWork.GetRepo<TripRequest>().GetByIdAsync(tripId);
        if (trip is null)
            throw new NotFoundException("Trip Request Not Found");

        if (trip.ManagerId != managerId)
            throw new ForbbidenException("you are not authorized to update this trip request.");

        trip.Date = date;
        trip.Details = details;
        trip.DriverId = driverId;
        trip.VehicleId = vehicleId;
        trip.Type = tripType;
        trip.Status = status;
        if (trip.PickupLocation != pickupLocation)
        {
            trip.PickupLocation = pickupLocation;
            trip.PickupLocationNominatimLink = GenerateNominatimLink(pickupLocation);
        }

        if (trip.Destination != destination)
        {
            trip.Destination = destination;
            trip.DestinationLocationNominatimLink = GenerateNominatimLink(destination);
        }
        trip.Destination = destination;
        _unitOfWork.GetRepo<TripRequest>().Update(trip);
        await _unitOfWork.SaveChanges();
    }

    #endregion

    #region Delete
    public async Task DeleteTripRequestAsync(string tripId, string managerId)
    {
        var trip = await _unitOfWork.GetRepo<TripRequest>().GetByIdAsync(tripId);
        if (trip is null)
            throw new NotFoundException("Trip request not found.");

        if (trip.ManagerId != managerId)
            throw new ForbbidenException("You are not authorized to delete this trip request.");

        _unitOfWork.GetRepo<TripRequest>().Delete(trip);
        await _unitOfWork.SaveChanges();
    }

    #endregion

    #region Get By Id
    public async Task<TripRequest> GetTripRequestByIdAsync(string id)
    {
        var spec = new TripRequestIncludesSpecification(id);
        return await _unitOfWork.GetRepo<TripRequest>().GetByIdWithSpecificationAsync(spec)
            ?? throw new NotFoundException("Trip Request Not Found");
    }

    #endregion

    #region Get All

    public async Task<IReadOnlyList<TripRequest>> GetAllTripRequestsAsync(
        TripRequestSpecParams specParams
    )
    {
        var spec = new TripRequestIncludesSpecification(specParams);
        return await _unitOfWork.GetRepo<TripRequest>().GetAllWithSpecificationAsync(spec);
    }

    #endregion

    #region Get All Trip Requests For User
    public async Task<IReadOnlyList<TripRequest>> GetAllTripsForUserAsync(
        TripRequestSpecParams specParams
    )
    {
        var user = await _unitOfWork.GetRepo<BusinessUser>().GetByIdAsync(specParams.DriverId);
        if (user == null)
            throw new NotFoundException("Driver not found.");

        var spec = new TripRequestIncludesSpecification(specParams);
        var trips = await _unitOfWork.GetRepo<TripRequest>().GetAllWithSpecificationAsync(spec);
        return trips;
    }

    #endregion

    #region update status

    public async Task UpdateTripRequestStatusAsync(string tripId)
    {
        var tripRequest = await _unitOfWork.GetRepo<TripRequest>().GetByIdAsync(tripId);
        if (tripRequest is null)
            throw new NotFoundException("Trip Request Not Found");

        if (tripRequest.Date.Date != DateTime.UtcNow.Date)
            throw new InvalidOperationException("trip has not started yet");

        if (tripRequest.Status == TripStatus.Pending)
            tripRequest.Status = TripStatus.Approved;

        _unitOfWork.GetRepo<TripRequest>().Update(tripRequest);
        await _unitOfWork.SaveChanges();
    }

    #endregion

    #region NominatimLink

    public string GenerateNominatimLink(string address)
    {
        var encoded = address;
        return $"https://nominatim.openstreetmap.org/search?q={encoded}&format=json&polygon_geojson=1&addressdetails=1";
    }

    #endregion
}
