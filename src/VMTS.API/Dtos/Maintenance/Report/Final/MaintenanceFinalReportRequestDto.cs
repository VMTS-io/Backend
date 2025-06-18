using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Report.Final;

public class MaintenanceFinalReportRequestDto
{
    public string Notes { get; set; } = default!;
    public decimal TotalCost { get; set; }
    public string InitialReportId { get; set; } = default!;
    public List<string> CategoryIds { get; set; } = [];
    public List<string> PartIds { get; set; } = [];

    // public DateTime FinishedDate { get; set; } = DateTime.UtcNow;
    // public Status Status { get; set; } = Status.Completed;
    // public string MechanicId { get; set; } = default!;
    // public string VehicleId { get; set; } = default!;
    // public string MaintenaceRequestId { get; set; } = default!;
}
