﻿using System.Security.Claims;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;
using VMTS.Core.Non_Entities_Class;
using VMTS.Core.Specifications.FaultReportSepcification;

namespace VMTS.Core.ServicesContract;

public interface IFaultReportService
{
    Task<FaultReport> CreateFaultReportAsync(
        string userId,
        string details,
        decimal cost,
        int fuelRefile,
        string address
    );

    Task<IReadOnlyList<FaultReport>> GetAllFaultReportsAsync(FaultReportSpecParams specParams);

    Task<IReadOnlyList<FaultReport>> GetAllFaultReportsForUserAsync(
        FaultReportSpecParams specParams
    );

    Task<FaultReport> GetFaultReportByIdAsync(string id);

    // Task<IReadOnlyList<FaultReport>> GetAllFaultReportsForVehicleAsync(
    //     string vehicleId,
    //     FaultReportSpecParams specParams
    // );

    Task UpdateFaultReportAsync(
        string reportId,
        string driverId,
        string details,
        string faultAddress,
        decimal cost,
        int fuelRefile
    );
    Task DeleteFaultReportAsync(string reportId, string managerId);

    Task UpdateMarkAsSeen(string faultReportId);

    Task<ChartDto> GetPriorityChartAsync();
}
