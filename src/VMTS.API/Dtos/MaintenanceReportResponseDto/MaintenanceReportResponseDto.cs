using System.Text.Json.Serialization;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.API.Dtos.MaintenanceReportResponseDto;

public class MaintenanceReportResponseDto
{
    public string Id { get; set; } = default!;

    public string ReportType { get; set; } = default!;
    public string Notes { get; set; } = default!;
    public MaintenanceStatus RequestStatus { get; set; }
    public string MechanicName { get; set; } = default!;
    public string VehicleName { get; set; } = default!;
    public string RequestTitle { get; set; } = default!;
    public MaintenanceCategory MaintenanceCategory { get; set; } = default!;

    // Common Dates
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? ReportDate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateOnly? ExpectedFinishDate { get; set; }

    // Initial Report Fields
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? ExpectedCost { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? MissingParts { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? ExpectedChangedParts { get; set; }

    // Final Report Fields
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? TotalCost { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? ChangedParts { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InitialReportSummary { get; set; }

    public bool Seen { get; set; }
}
