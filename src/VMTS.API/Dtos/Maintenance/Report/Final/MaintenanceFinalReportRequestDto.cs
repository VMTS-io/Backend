using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Report.Final;

public class MaintenanceFinalReportRequestDto
{
    public string Notes { get; set; } = default!;
    public string InitialReportId { get; set; } = default!;
    public List<MaintenanceInitialReportParts> ChangedParts { get; set; } = [];

    // public bool IsPartsChanged { get; set; }
    // public string CategoryIds { get; set; } = default!;
    // public decimal TotalCost { get; set; }
    // public DateTime FinishedDate { get; set; } = DateTime.UtcNow;
    // public Status Status { get; set; } = Status.Completed;
    // public string MechanicId { get; set; } = default!;
    // public string VehicleId { get; set; } = default!;
    // public string MaintenaceRequestId { get; set; } = default!;
}
