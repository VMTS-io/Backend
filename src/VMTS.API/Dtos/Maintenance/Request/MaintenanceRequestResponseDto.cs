using System.Text.Json.Serialization;
using VMTS.API.Dtos.Maintenance.Report.Final;
using VMTS.API.Dtos.Part;
using VMTS.API.Dtos.Vehicles;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.Maintenance.Request;

public class MaintenanceRequestResponseDto
{
    public string Id { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime Date { get; set; }

    public MaintenanceCategory MaintenaceCategory { get; set; } = default!;

    public MaintenanceStatus Status { get; set; }

    public BussinessUserDto Manager { get; set; } = default!;

    public BussinessUserDto Mechanic { get; set; } = default!;

    public VehicleListDto Vehicle { get; set; } = default!;

    public List<PartDto> Parts { get; set; } = [];

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MaintenanceInitialReportSummaryDto? InitialReport { get; set; } = default!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MaintenanceFinalReportSummaryDto FinalReport { get; set; } = default!;
}
