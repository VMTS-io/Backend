using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Report.Final;

public class MaintenanceFinalReportResponseDto
{
    public string Id { get; set; } = default!;
    public string Notes { get; set; } = default!;
    public decimal TotalCost { get; set; }
    public DateTime FinishedDate { get; set; }
    public string RequestStatus { get; set; } = default!;

    public string MechanicName { get; set; } = default!;
    public string VehicleName { get; set; } = default!;
    public string InitialReportSummary { get; set; } = default!;
    public string RequestTitle { get; set; } = default!;

    public string MaintenanceCategory { get; set; } = default!;
    public List<string> ChangedParts { get; set; } = [];
}
