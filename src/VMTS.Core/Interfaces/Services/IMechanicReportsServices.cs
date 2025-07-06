using VMTS.Core.Entities.Reports;
using VMTS.Core.Specifications.Maintenance.Report;

namespace VMTS.Core.Interfaces.Services;

public interface IMechanicReportsServices
{
    Task<MechanicReportsResult> GetMechanicReportsAsync(MaintenanceReportSpecParams specParams);
}
