using Microsoft.AspNetCore.Identity;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.ServicesContract;
using VMTS.Core.Specifications;
using VMTS.Core.Specifications.FaultReportSepcification;
using VMTS.Core.Specifications.TripRequestSpecification;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;



    public ReportService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager )
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        
    }

    #region Create

    public async Task<FaultReport> CreateFaultReportAsync(
    string userId,
    string details,
    MaintenanceType faultType,
    decimal cost,
    int fuelRefile,
    string address
)
{
    // Basic user validations
    if (string.IsNullOrWhiteSpace(userId))
        throw new UnauthorizedAccessException("User ID is required.");

    if (string.IsNullOrWhiteSpace(details))
        throw new ArgumentNullException(nameof(details), "Fault details are required.");

    if (string.IsNullOrWhiteSpace(address))
        throw new ArgumentNullException(nameof(address), "Fault address is required.");

    // Fetch business user
    var businessUserSpec = new BusinessUserSpecification(userId);
    var businessUser = await _unitOfWork.GetRepo<BusinessUser>().GetByIdWithSpecificationAsync(businessUserSpec);
    if (businessUser == null)
        throw new InvalidOperationException("User not found in the business user registry.");

    if (businessUser.DriverTripRequest == null || !businessUser.DriverTripRequest.Any())
        throw new UnauthorizedAccessException("User is not authorized to create fault reports. Must be a registered driver.");

    // Get active trip
    var tripSpec = new TripRequestIncludesSpecification(userId);
    var tripRequest = await _unitOfWork
        .GetRepo<TripRequest>()
        .GetByIdWithSpecificationAsync(tripSpec); 

    if (tripRequest == null)
        throw new InvalidOperationException("No active trip found for this driver.");

    if (tripRequest.Vehicle == null || string.IsNullOrEmpty(tripRequest.Vehicle.Id))
        throw new InvalidOperationException("Trip does not have an assigned vehicle.");

    if (tripRequest.DriverId != userId)
        throw new UnauthorizedAccessException("You can only create fault reports for trips assigned to you.");
    
    // Create the fault report
    var faultReport = new FaultReport
    {
        Details = details,
        ReportedAt = DateTime.UtcNow,
        FaultType = faultType,
        TripId = tripRequest.Id,
        VehicleId = tripRequest.Vehicle.Id,
        DriverId = userId,
        Destination = tripRequest.Destination,
        FaultAddress = address,
        FuelRefile = fuelRefile,
        Cost = cost
    };
    
    

    // Save fault report
    businessUser.DriverFaultReport.Add(faultReport);
    _unitOfWork.GetRepo<BusinessUser>().Update(businessUser);

    await _unitOfWork.GetRepo<FaultReport>().CreateAsync(faultReport);
    var result = await _unitOfWork.SaveChanges();

    if (result <= 0)
        throw new InvalidOperationException("Failed to save the fault report.");

    return faultReport;
}


#endregion

    #region Get All Fault Reports
    
    public async Task<IReadOnlyList<FaultReport>> GetAllFaultReportsAsync(FaultReportSpecParams specParams)
    {
        var user = _userManager.FindByIdAsync(specParams.DriverId);
        if (user == null) throw new NotFoundException("Driver Id Not Found.");
        
        var vehicle = await _unitOfWork.GetRepo<Vehicle>().GetByIdAsync(specParams.VehicleId);
        if (vehicle == null) throw new NotFoundException("Vehicle Id Not Found.");

        var trip = await _unitOfWork.GetRepo<TripRequest>().GetByIdAsync(specParams.TripId);
        if (trip == null) throw new NotFoundException("Trip Id Not Found.");
        
        var specs = new FaultReportIncludesSpecification(specParams);
        var faultReports = await _unitOfWork.GetRepo<FaultReport>().GetAllWithSpecificationAsync(specs);
        return faultReports;
    }

   

    #endregion
    
    #region Get By Id
    
    public async Task<FaultReport> GetFaultReportByIdAsync(string id )
    {
        var specs = new FaultReportIncludesSpecification(id);
        return  await _unitOfWork.GetRepo<FaultReport>().GetByIdWithSpecificationAsync(specs)?? throw new NotFoundException("Fault Report Not Found");
    }

    

    #endregion
    
    #region Update Fault Report
    
    public async Task UpdateFaultReportAsync(string reportId,string driverId,string details,string faultAddress,decimal cost,int fuelRefile)
    {
        var faultReport = await _unitOfWork.GetRepo<FaultReport>().GetByIdAsync(reportId);
        if (faultReport is null) throw new NotFoundException("Fault Report Not Found");

        if (faultReport.DriverId != driverId) throw new UnauthorizedAccessException("you are not authorized to update this fault report.");
        faultReport.Details = details;
        faultReport.FaultAddress = faultAddress;
        faultReport.Cost = cost;
        faultReport.FuelRefile = fuelRefile;
        
        _unitOfWork.GetRepo<FaultReport>().Update(faultReport);
        await _unitOfWork.SaveChanges();
        
    }

    

    #endregion

    #region Delete Fault Report

    public async Task DeleteFaultReportAsync(string reportId, string managerId)
    {
        var specs = new FaultReportIncludesSpecification(reportId);
        var faultReport = await _unitOfWork.GetRepo<FaultReport>().GetByIdWithSpecificationAsync(specs);
        if (faultReport is null)
            throw new NotFoundException("Report not found");
        
        if (faultReport.Trip.ManagerId != managerId)
            throw new UnauthorizedAccessException("You are not allowed to delete this report.");

        _unitOfWork.GetRepo<FaultReport>().Delete(faultReport);
        await _unitOfWork.SaveChanges();
    }


    #endregion
    
    
    
    #region Get All Fault Reports For User
    public async Task<IReadOnlyList<FaultReport>> GetAllFaultReportsForUserAsync(string userId, FaultReportSpecParams specParams)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new ArgumentException("User not found.");
        
        var specs = new FaultReportIncludesSpecification(specParams,userId);
        var userReports = await _unitOfWork.GetRepo<FaultReport>().GetAllWithSpecificationAsync(specs);
        return userReports;
    }

    #endregion
    
    #region Get All For Vehicle 

    public async Task<IReadOnlyList<FaultReport>> GetAllFaultReportsForVehicleAsync(string vehicleId, FaultReportSpecParams specParams)
    {
        var vehicle = _unitOfWork.GetRepo<Vehicle>().GetByIdAsync(vehicleId);
        if (vehicle == null) throw new ArgumentException("Vehicle not found.");
        var specs = new FaultReportIncludesSpecification( vehicleId ,specParams);
        var vehicleReports = await _unitOfWork.GetRepo<FaultReport>().GetAllWithSpecificationAsync(specs);
        return vehicleReports;
    }

    #endregion
    
    
}
