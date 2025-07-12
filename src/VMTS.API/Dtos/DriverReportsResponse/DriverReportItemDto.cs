using System.Text.Json.Serialization;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Trip;

namespace VMTS.API.Dtos.DriverReportsResponse;

public class DriverReportItemDto
{
    public string Id { get; set; } = default!;
    public string ReportType { get; set; } = default!; // "Trip" or "Fault"

    public DriverDto Driver { get; set; } = default!;
    public VehicleDto Vehicle { get; set; } = default!;
    public DateTime ReportedAt { get; set; }
    public bool Seen { get; set; }

    public TripStatus? Status { get; set; }

    // Trip-specific
    public string? Destination { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? FuelCost { get; set; }

    // Fault-specific

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FaultAddress { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FaultDetails { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FaultType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Priority { get; set; } // enum like Low, Medium, High (based on AI)

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? Cost { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? SentToMechanic { get; set; } = false;
}
