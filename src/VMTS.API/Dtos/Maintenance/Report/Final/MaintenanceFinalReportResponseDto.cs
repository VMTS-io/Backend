using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Report.Final;

public class MaintenanceFinalReportResponseDto
{
    public string Id { get; set; } = default!;
    public string Notes { get; set; } = default!;
    public decimal ExpectedCost { get; set; }
    public DateTime FinishedDate { get; set; }
    public Status Status { get; set; }

    public string MechanicName { get; set; } = default!;
    public string ManagerName { get; set; } = default!;
    public string VehicleName { get; set; } = default!;
    public string InitialReportSummary { get; set; } = default!;
    public string RequestTitle { get; set; } = default!;

    public List<string> CategoryNames { get; set; } = [];
    public List<string> ChangedPartNames { get; set; } = [];
}
