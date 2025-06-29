using System.Runtime.InteropServices.Marshalling;
using Microsoft.AspNetCore.Identity;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Specifications;
using VMTS.Core.Specifications.TripRequestSpecification;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class TripReportService : ITripReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public TripReportService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    #region create

    public async Task<TripReport> CreateTripReportAsync(
        string driverId,
        int fuelRefile,
        decimal cost,
        string details
    )
    {
        // 1. Fetch active approved trip for the driver
        var tripSpec = new TripRequestIncludesSpecification(
            new TripRequestSpecParams
            {
                DriverId = driverId,
                Status = new[] { TripStatus.Approved },
            }
        );
        var tripRequest = await _unitOfWork
            .GetRepo<TripRequest>()
            .GetByIdWithSpecificationAsync(tripSpec);

        if (tripRequest == null)
            throw new InvalidOperationException("No active trip found for this driver.");

        if (tripRequest.DriverId != driverId)
            throw new ForbbidenException(
                "You are not allowed to create a trip report for this trip."
            );

        if (tripRequest.Status != TripStatus.Approved)
            throw new InvalidOperationException("This trip is not in an approvable state.");

        if (tripRequest.Vehicle == null || string.IsNullOrEmpty(tripRequest.Vehicle.Id))
            throw new InvalidOperationException("Trip does not have an assigned vehicle.");

        if (tripRequest.TripReports is not null)
            throw new InvalidOperationException(
                "A trip report has already been submitted for this trip."
            );

        // 2. Fetch business user
        var businessUserSpec = new BusinessUserSpecification(driverId);
        var businessUser = await _unitOfWork
            .GetRepo<BusinessUser>()
            .GetByIdWithSpecificationAsync(businessUserSpec);

        if (businessUser == null)
            throw new InvalidOperationException("User not found in the business user registry.");

        if (businessUser.DriverTripRequest == null || !businessUser.DriverTripRequest.Any())
            throw new UnauthorizedAccessException(
                "User is not authorized to create trip reports. Must be a registered driver."
            );

        // 3. Create and attach trip report
        var tripReport = new TripReport
        {
            DriverId = driverId,
            VehicleId = tripRequest.Vehicle.Id,
            TripId = tripRequest.Id,
            Destination = tripRequest.Destination,
            Details = details,
            FuelCost = cost,
            FuelRefile = fuelRefile,
            ReportedAt = DateTime.UtcNow,
        };

        businessUser.DriverTripReport ??= new List<TripReport>();
        businessUser.DriverTripReport.Add(tripReport);

        tripRequest.Status = TripStatus.Completed;

        await _unitOfWork.GetRepo<TripReport>().CreateAsync(tripReport);
        var result = await _unitOfWork.SaveChanges();

        if (result <= 0)
            throw new InvalidOperationException("Failed to save the trip report.");

        return tripReport;
    }

    #endregion

    #region update

    public async Task UpdateTripReportAsync(
        string reportId,
        string driverId,
        string details,
        int fuelRefile,
        decimal cost
    )
    {
        var tripReport = await _unitOfWork.GetRepo<TripReport>().GetByIdAsync(reportId);
        if (tripReport is null)
            throw new NotFoundException("Trip Report Not Found");
        if (tripReport.DriverId != driverId)
            throw new ForbbidenException("you are not authorized to update this trip report.");

        tripReport.Details = details;
        tripReport.FuelRefile = fuelRefile;
        tripReport.FuelCost = cost;

        _unitOfWork.GetRepo<TripReport>().Update(tripReport);
        await _unitOfWork.SaveChanges();
    }

    #endregion

    #region delete

    public async Task DeleteTripReportAsync(string reportId, string managerId)
    {
        var tripReport = await _unitOfWork.GetRepo<TripReport>().GetByIdAsync(reportId);
        if (tripReport is null)
            throw new NotFoundException("Report not found");

        var trip = await _unitOfWork.GetRepo<TripRequest>().GetByIdAsync(tripReport.TripId);
        if (trip is null)
            throw new NotFoundException("Associated trip not found");
        if (trip.ManagerId != managerId)
            throw new ForbbidenException("You are not allowed to delete this report.");

        _unitOfWork.GetRepo<TripReport>().Delete(tripReport);
        await _unitOfWork.SaveChanges();
    }

    #endregion

    #region GetAll

    public async Task<IReadOnlyList<TripReport>> GetAllTripReportsAsync(
        TripReportSpecParams specParams
    )
    {
        if (!string.IsNullOrWhiteSpace(specParams.DriverId))
        {
            var user = await _userManager.FindByIdAsync(specParams.DriverId);
            if (user == null)
                throw new NotFoundException("Driver Id Not Found.");
        }

        if (!string.IsNullOrWhiteSpace(specParams.VehicleId))
        {
            var vehicle = await _unitOfWork.GetRepo<Vehicle>().GetByIdAsync(specParams.VehicleId);
            if (vehicle == null)
                throw new NotFoundException("Vehicle Id Not Found.");
        }

        if (!string.IsNullOrWhiteSpace(specParams.TripId))
        {
            var trip = await _unitOfWork.GetRepo<TripRequest>().GetByIdAsync(specParams.TripId);
            if (trip == null)
                throw new NotFoundException("Trip Id Not Found.");
        }

        var specs = new TripReportIncludesSpecification(specParams);
        var tripReports = await _unitOfWork
            .GetRepo<TripReport>()
            .GetAllWithSpecificationAsync(specs);

        return tripReports;
    }

    #endregion

    #region Get By Id

    public async Task<TripReport> GetTripReportByIdAsync(string id)
    {
        var specs = new TripReportIncludesSpecification(id);
        return await _unitOfWork.GetRepo<TripReport>().GetByIdWithSpecificationAsync(specs)
            ?? throw new NotFoundException("Trip Report Not Found");
    }

    #endregion

    #region get trip reports for user


    public async Task<IReadOnlyList<TripReport>> GetAllTripReportsForUserAsync(
        TripReportSpecParams specParams
    )
    {
        var user = await _unitOfWork.GetRepo<BusinessUser>().GetByIdAsync(specParams.DriverId);
        if (user == null)
            throw new NotFoundException("User not found.");

        var specs = new TripReportIncludesSpecification(specParams);
        var userReports = await _unitOfWork
            .GetRepo<TripReport>()
            .GetAllWithSpecificationAsync(specs);
        return userReports;
    }

    #endregion

    #region mark as seen

    public async Task UpdateMarkAsSeen(string tripReportId)
    {
        var tripReport = await _unitOfWork.GetRepo<TripReport>().GetByIdAsync(tripReportId);
        if (tripReport is null)
            throw new NotFoundException("Trip Report Not Found");
        if (tripReport.Seen == false)
        {
            tripReport.Seen = true;
            _unitOfWork.GetRepo<TripReport>().Update(tripReport);
            await _unitOfWork.SaveChanges();
        }
    }

    #endregion
}
