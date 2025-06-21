using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Specifications.Maintenance.Report.Initial;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceInitialReportServices
{
    Task CreateInitialReportAsync(MaintenanceInitialReport report);
    Task UpdateInitialReportAsync(MaintenanceInitialReport report);
    Task<IReadOnlyList<MaintenanceInitialReport>> GetAllInitialReportsAsync(
        MaintenanceIntialReportSpecParams specParams
    );
    Task<MaintenanceInitialReport> GetInitialReportByIdAsync(string id);
    Task DeleteInitialReportAsync(string id);
}
