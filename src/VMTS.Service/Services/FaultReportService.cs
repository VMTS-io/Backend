using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using VMTS.Core.Entities.Ai;
using VMTS.Core.Entities.Identity;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Entities.User_Business;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Non_Entities_Class;
using VMTS.Core.ServicesContract;
using VMTS.Core.Specifications;
using VMTS.Core.Specifications.FaultReportSepcification;
using VMTS.Core.Specifications.TripRequestSpecification;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class FaultReportService : IFaultReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFaultPredictionService _faultPredictionService;
    private readonly IAiClient _aiClient;

    public FaultReportService(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IFaultPredictionService faultPredictionService,
        IAiClient aiClient
    )
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _faultPredictionService = faultPredictionService;
        _aiClient = aiClient;
    }

    #region Create

    public async Task<FaultReport> CreateFaultReportAsync(
        string userId,
        string details,
        decimal cost,
        int fuelRefile,
        string address
    )
    {
        // Get active trip
        var tripSpec = new TripRequestIncludesSpecification(
            new TripRequestSpecParams()
            {
                DriverId = userId,
                Status = new[] { TripStatus.Approved },
            }
        );
        var tripRequest = await _unitOfWork
            .GetRepo<TripRequest>()
            .GetByIdWithSpecificationAsync(tripSpec);

        if (tripRequest == null)
            throw new InvalidOperationException("No active trip found for this driver.");

        if (tripRequest.DriverId != userId)
            throw new ForbbidenException(
                "you are not allowed to create fault report for this trip"
            );

        // Fetch business user
        var businessUserSpec = new BusinessUserSpecification(userId);
        var businessUser = await _unitOfWork
            .GetRepo<BusinessUser>()
            .GetByIdWithSpecificationAsync(businessUserSpec);
        if (businessUser == null)
            throw new InvalidOperationException("User not found in the business user registry.");

        if (businessUser.DriverTripRequest == null || !businessUser.DriverTripRequest.Any())
            throw new UnauthorizedAccessException(
                "User is not authorized to create fault reports. Must be a registered driver."
            );

        if (tripRequest.FaultReports is not null)
            throw new InvalidOperationException(
                "A fault report has already been submitted for this trip."
            );

        if (tripRequest.Vehicle == null || string.IsNullOrEmpty(tripRequest.Vehicle.Id))
            throw new InvalidOperationException("Trip does not have an assigned vehicle.");

        var faultReport = new FaultReport
        {
            Details = details,
            ReportedAt = DateTime.UtcNow,
            TripId = tripRequest.Id,
            VehicleId = tripRequest.Vehicle.Id,
            DriverId = userId,
            Destination = tripRequest.Destination,
            FaultAddress = address,
            FuelRefile = fuelRefile,
            Cost = cost,
        };

        // Call AI model
        var aiEndpointConfigId = (await _unitOfWork.GetRepo<AiEndpointConfig>().GetAllAsync())
            .First(ac => ac.Name == "FaultPriviledge")
            .Id;

        var aiPrediction = await _faultPredictionService.PredictAsync(details, aiEndpointConfigId);

        if (aiPrediction.IsSuccess)
        {
            try
            {
                // Extract urgency value using string manipulation
                string urgencyValue = aiPrediction.predicted_urgency;
                if (urgencyValue.Contains("\"predicted_urgency\":"))
                {
                    var startIndex = urgencyValue.IndexOf("\":\"") + 3;
                    var endIndex = urgencyValue.LastIndexOf("\"");
                    if (startIndex < endIndex)
                    {
                        faultReport.Priority = urgencyValue.Substring(
                            startIndex,
                            endIndex - startIndex
                        );
                    }
                }

                // Extract issue value using string manipulation
                string issueValue = aiPrediction.predicted_issue;
                if (issueValue.Contains("\"predicted_issue\":"))
                {
                    var startIndex = issueValue.IndexOf("\":\"") + 3;
                    var endIndex = issueValue.LastIndexOf("\"");
                    if (startIndex < endIndex)
                    {
                        faultReport.AiPredictedFaultType = issueValue.Substring(
                            startIndex,
                            endIndex - startIndex
                        );
                    }
                }

                faultReport.IsAiPredictionSuccessful = true;
            }
            catch (Exception)
            {
                faultReport.IsAiPredictionSuccessful = false;
                faultReport.Priority = "Unknown";
                faultReport.AiPredictedFaultType = "Unknown";
            }
        }
        else
        {
            faultReport.IsAiPredictionSuccessful = false;
        }

        // Add to repository
        await _unitOfWork.GetRepo<FaultReport>().CreateAsync(faultReport);

        // Update related entities
        businessUser.DriverFaultReport ??= new List<FaultReport>();
        businessUser.DriverFaultReport.Add(faultReport);

        tripRequest.Status = TripStatus.Completed;

        var result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Failed to save the fault report.");

        return faultReport;
    }

    #endregion

    #region Get All Fault Reports

    public async Task<IReadOnlyList<FaultReport>> GetAllFaultReportsAsync(
        FaultReportSpecParams specParams
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

        var specs = new FaultReportIncludesSpecification(specParams);
        var faultReports = await _unitOfWork
            .GetRepo<FaultReport>()
            .GetAllWithSpecificationAsync(specs);

        return faultReports;
    }

    #endregion

    #region Get By Id

    public async Task<FaultReport> GetFaultReportByIdAsync(string id)
    {
        var specs = new FaultReportIncludesSpecification(id);
        return await _unitOfWork.GetRepo<FaultReport>().GetByIdWithSpecificationAsync(specs)
            ?? throw new NotFoundException("Fault Report Not Found");
    }

    #endregion

    #region Update Fault Report

    public async Task UpdateFaultReportAsync(
        string reportId,
        string driverId,
        string details,
        string faultAddress,
        decimal cost,
        int fuelRefile
    )
    {
        var faultReport = await _unitOfWork.GetRepo<FaultReport>().GetByIdAsync(reportId);
        if (faultReport is null)
            throw new NotFoundException("Fault Report Not Found");

        if (faultReport.DriverId != driverId)
            throw new ForbbidenException("you are not authorized to update this fault report.");
        faultReport.Details = details;
        faultReport.FaultAddress = faultAddress;
        faultReport.Cost = cost;
        faultReport.FuelRefile = fuelRefile;

        _unitOfWork.GetRepo<FaultReport>().Update(faultReport);
        await _unitOfWork.SaveChangesAsync();
    }

    #endregion

    #region Delete Fault Report

    public async Task DeleteFaultReportAsync(string reportId, string managerId)
    {
        var faultReport = await _unitOfWork.GetRepo<FaultReport>().GetByIdAsync(reportId);
        if (faultReport is null)
            throw new NotFoundException("Report not found");

        var trip = await _unitOfWork.GetRepo<TripRequest>().GetByIdAsync(faultReport.TripId);
        if (trip is null)
            throw new NotFoundException("Associated trip not found");

        if (trip.ManagerId != managerId)
            throw new ForbbidenException("You are not allowed to delete this report.");

        _unitOfWork.GetRepo<FaultReport>().Delete(faultReport);
        await _unitOfWork.SaveChangesAsync();
    }

    #endregion


    #region Get All Fault Reports For User
    public async Task<IReadOnlyList<FaultReport>> GetAllFaultReportsForUserAsync(
        FaultReportSpecParams specParams
    )
    {
        var user = await _unitOfWork.GetRepo<BusinessUser>().GetByIdAsync(specParams.DriverId);
        if (user == null)
            throw new NotFoundException("User not found.");

        var specs = new FaultReportIncludesSpecification(specParams);

        var userReports = await _unitOfWork
            .GetRepo<FaultReport>()
            .GetAllWithSpecificationAsync(specs);
        return userReports;
    }

    #endregion

    #region mark as seen

    public async Task UpdateMarkAsSeen(string faultReportId)
    {
        var faultReport = await _unitOfWork.GetRepo<FaultReport>().GetByIdAsync(faultReportId);
        if (faultReport is null)
            throw new NotFoundException("Fault Report Not Found");
        if (faultReport.Seen == false)
        {
            faultReport.Seen = true;
            _unitOfWork.GetRepo<FaultReport>().Update(faultReport);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<ChartDto> GetPriorityChartAsync()
    {
        var faultReports = await _unitOfWork.GetRepo<FaultReport>().GetAllAsync();

        var priorityList = faultReports
            .Where(fr => !string.IsNullOrWhiteSpace(fr.Priority))
            .Select(fr => fr.Priority.Trim())
            .ToArray();

        var chartRequest = new ChartRequestDto { Predictions = priorityList };

        var chartBase64 = await _aiClient.SendPrioritiesAndGetChartAsync(chartRequest);

        // Return ChartDto with both Priority list and base64 chart image
        return new ChartDto { Priority = priorityList, ChartBase64 = chartBase64 };
    }

    #endregion
}
