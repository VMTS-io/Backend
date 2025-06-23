using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Entities.Reports;

public class DriverReportsResult
{
    public IReadOnlyList<TripReport> TripReports { get; set; } = new List<TripReport>();
    public IReadOnlyList<FaultReport> FaultReports { get; set; } = new List<FaultReport>();
}
