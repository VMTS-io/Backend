namespace VMTS.Core.Non_Entities_Class;

public class VehicleMaintenancePredictionDto
{
    public string VehicleCategory { get; set; } = default!;
    public int VehicleAge { get; set; }
    public string[] ReportedIssues { get; set; } = [];
    public string FuelType { get; set; } = default!;
    public string TransmissionType { get; set; } = default!;
    public string EngineSize { get; set; } = default!;
    public int OdometerReading { get; set; }
    public DateTime? LastServiceDate { get; set; }
    public string TireCondition { get; set; } = default!;
    public string BrakeCondition { get; set; } = default!;
    public string BatteryStatus { get; set; } = default!;
}
