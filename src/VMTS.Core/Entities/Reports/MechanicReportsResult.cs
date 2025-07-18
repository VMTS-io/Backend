using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Entities.Reports;

public class MechanicReportsResult
{
    public IReadOnlyList<MaintenanceInitialReport> InitialReports { get; set; } =
        new List<MaintenanceInitialReport>();
    public IReadOnlyList<MaintenanceFinalReport> FinalReports { get; set; } =
        new List<MaintenanceFinalReport>();
}
