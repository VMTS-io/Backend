using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Specifications.Maintenance.Report;
using VMTS.Core.Specifications.Maintenance.Report.Final;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceFinalReportServices
{
    Task CreateFinalReportAsync(MaintenanceFinalReport report);
    Task UpdateFinalReportAsync(MaintenanceFinalReport report);
    Task<IReadOnlyList<MaintenanceFinalReport>> GetAllFinalReportsAsync(
        MaintenanceFinalReportSpecParams specParams
    );
    Task<IReadOnlyList<MaintenanceFinalReport>> GetAllFinalReportsAsync(
        MaintenanceReportSpecParams specParams
    );
    Task<MaintenanceFinalReport> GetFinalReportByIdAsync(string id);
    Task DeleteFinalReportAsync(string id);

    Task UpdateMarkAsSeen(string finalReportId);
}
