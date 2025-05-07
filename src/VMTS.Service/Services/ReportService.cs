using Microsoft.AspNetCore.Identity;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.ServicesContract;
using VMTS.Core.Specifications.TripRequestSpecification;

namespace VMTS.Service.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public ReportService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<FaultReport> CreateFaultReportAsync(
        string userEmail,
        string details,
        MaintenanceType faultType,
        string address
    )
    {
        // get driver
        var driver = await _userManager.FindByEmailAsync(userEmail);
        if (driver == null)
            throw new UnauthorizedAccessException("Unauthorized: Unable to determine driver.");
        // get driverId
        var driverId = driver.Id;

        // get active trip for driver
        var tripSpec = new TripRequestIncludesSpecification(driverId);
        var tripRequest = await _unitOfWork
            .GetRepo<TripRequest>()
            .GetByIdWithSpecificationAsync(tripSpec);

        if (tripRequest == null)
            throw new Exception("No active trip found for this driver.");
        if (string.IsNullOrEmpty(tripRequest.Vehicle.Id))
            throw new Exception("Trip does not have an assigned vehicle.");

        var faultReport = new FaultReport
        {
            Id = Guid.NewGuid().ToString(),
            Details = details,
            ReportedAt = DateTime.UtcNow,
            FaultType = faultType,
            TripId = tripRequest.Id,
            VehicleId = tripRequest.Vehicle.Id,
            DriverId = tripRequest.DriverId,
            Destination = tripRequest.Destination,
            FaultAddress = address,
        };

        await _unitOfWork.GetRepo<FaultReport>().CreateAsync(faultReport);
        var result = await _unitOfWork.SaveChanges();

        return result > 0 ? faultReport : null;
    }
}
