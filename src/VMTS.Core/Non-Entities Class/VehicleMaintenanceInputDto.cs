using System.Text.Json.Serialization;

namespace VMTS.Core.Non_Entities_Class;

public class VehicleMaintenanceInputDto
{
    [JsonPropertyName("Vehicle_Model")]
    public string Vehicle_Model { get; set; } = default!;

    [JsonPropertyName("Reported_Issues")]
    public int Reported_Issues { get; set; }

    [JsonPropertyName("Vehicle_Age")]
    public int Vehicle_Age { get; set; }

    [JsonPropertyName("Fuel_Type")]
    public string Fuel_Type { get; set; } = default!;

    [JsonPropertyName("Transmission_Type")]
    public string Transmission_Type { get; set; } = default!;

    [JsonPropertyName("Engine_Size")]
    public double Engine_Size { get; set; }

    [JsonPropertyName("Odometer_Reading")]
    public int Odometer_Reading { get; set; }

    [JsonPropertyName("Last_Service_Date")]
    public string Last_Service_Date { get; set; } = default!;

    [JsonPropertyName("Tire_Condition")]
    public string Tire_Condition { get; set; } = default!;

    [JsonPropertyName("Brake_Condition")]
    public string Brake_Condition { get; set; } = default!;

    [JsonPropertyName("Battery_Status")]
    public string Battery_Status { get; set; } = default!;
}
