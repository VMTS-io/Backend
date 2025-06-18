using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Specifications.Maintenance.Report.Final;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceFinalReportServices
{
    Task CreateFinalReportAsync(
        MaintenanceFinalReport report,
        List<string> categoryIds,
        List<string> partIds
    );
    Task UpdateFinalReportAsync(
        MaintenanceFinalReport report,
        List<string> categoryIds,
        List<string> partIds
    );
    Task<IReadOnlyList<MaintenanceFinalReport>> GetAllFinalReportsAsync(
        MaintenanceFinalReportSpecParams specParams
    );
    Task<MaintenanceFinalReport> GetFinalReportByIdAsync(string id);
    Task DeleteFinalReportAsync(string id);
}
