using VMTS.API.Dtos.Maintenance.Request;
using VMTS.API.Dtos.Part;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Report.Initial;

public class MaintenanceInitialReportDetailsDto
{
    public string Id { get; set; } = default!;

    public string Notes { get; set; } = default!;

    public decimal ExpectedCost { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public DateOnly ExpectedFinishDate { get; set; }

    public string MaintenanceCategory { get; set; } = default!;

    public BussinessUserDto Mechanic { get; set; } = default!;

    public VehicleForMaintenanceReportDto Vehicle { get; set; } = default!;

    public MaintenanceRequestForReportDto MaintenanceRequest { get; set; } = default!;

    public ICollection<PartForMaintenanceReportDto> MissingParts { get; set; } = [];

    public ICollection<MaintenanceReportPartResponseDto> ExpectedChangedParts { get; set; } = [];
}
