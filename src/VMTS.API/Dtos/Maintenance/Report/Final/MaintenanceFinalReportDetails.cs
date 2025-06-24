using VMTS.API.Dtos.Maintenance.Report.Initial;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Report.Final;

public class MaintenanceFinalReportDetailsDto
{
    public string Notes { get; set; } = default!;

    public decimal TotalCost { get; set; }

    public DateTime FinishedDate { get; set; }

    public string MaintenanceCategory { get; set; } = default!;

    public BussinessUserDto Mechanic { get; set; } = default!;

    public VehicleForMaintenanceReportDto Vehicle { get; set; } = default!;

    public MaintenanceInitialReportSummaryDto InitialReport { get; set; } = default!;

    public MaintenanceRequestForReportDto MaintenaceRequest { get; set; } = default!;

    public ICollection<MaintenanceReportPartResponseDto> ChangedParts { get; set; } = [];
}
