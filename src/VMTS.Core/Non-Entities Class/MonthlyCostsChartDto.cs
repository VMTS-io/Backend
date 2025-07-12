using System.Text.Json.Serialization;

namespace VMTS.Core.Non_Entities_Class;

public class MonthlyCostsChartDto
{
    [JsonPropertyName("months")]
    public string[] Months { get; set; } = default!;

    [JsonPropertyName("fuel_costs")]
    public decimal[] FuelCosts { get; set; } = default!;

    [JsonPropertyName("maintenance_costs")]
    public decimal[] MaintenanceCosts { get; set; } = default!;
}
