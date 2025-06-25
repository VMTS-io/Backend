using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Specifications.Maintenance.Report.Initial;

namespace VMTS.Core.Entities.Reports;

public class MechanicReportsResult
{
    public IReadOnlyList<MaintenanceInitialReport> InitialReports { get; set; } =
        new List<MaintenanceInitialReport>();
    public IReadOnlyList<MaintenanceFinalReport> FinalReports { get; set; } =
        new List<MaintenanceFinalReport>();
}
