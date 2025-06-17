using System.Security.Claims;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;
using VMTS.Core.Specifications.FaultReportSepcification;

namespace VMTS.Core.ServicesContract;

public interface IReportService
{
    Task<FaultReport> CreateFaultReportAsync(
        string userEmail,
        string details,
        MaintenanceType faultType,
        decimal cost,
        int fuelRefile,
        string address);

    Task<IReadOnlyList<FaultReport>> GetAllFaultReportsAsync(FaultReportSpecParams specParams);
    
    Task<IReadOnlyList<FaultReport>>GetAllFaultReportsForUserAsync(string UserId, FaultReportSpecParams specParams);

    Task<FaultReport> GetFaultReportByIdAsync(string id);
    
    Task<IReadOnlyList<FaultReport>> GetAllFaultReportsForVehicleAsync(string vehicleId , FaultReportSpecParams specParams);

    Task UpdateFaultReportAsync (string reportId,string driverId,string details,string faultAddress,decimal cost,int fuelRefile);
    Task DeleteFaultReportAsync(string reportId ,string managerId);
    
}