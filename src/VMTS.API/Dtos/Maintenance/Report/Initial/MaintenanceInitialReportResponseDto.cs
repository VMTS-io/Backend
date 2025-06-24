using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Report.Initial;

public class MaintenanceInitialReportResponseDto
{
    public string Id { get; set; } = default!;
    public string Notes { get; set; } = default!;
    public decimal ExpectedCost { get; set; }
    public DateTime Date { get; set; }
    public DateOnly ExpectedFinishDate { get; set; }
    public string RequestStatus { get; set; } = default!;
    public string MechanicName { get; set; } = default!;
    public string VehicleName { get; set; } = default!;
    public string RequestTitle { get; set; } = default!;

    public string MaintenanceCategory { get; set; } = default!;
    public List<string> MissingParts { get; set; } = [];
    public List<string> ExpectedChangedParts { get; set; } = [];
}
