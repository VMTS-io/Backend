using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Non_Entities_Class;

public class MaintenanceTrackingAi
{
    public string VehicleType { get; set; } = default!;
    public string Make { get; set; } = default!;
    public DrivingCondition DrivingCondition { get; set; } = default!;
    public double AvgDailyKm { get; set; }
    public int VehicleAge { get; set; }
    public int TotalKm { get; set; }

    public Dictionary<string, double> KmSince { get; set; } = [];
    public Dictionary<string, double> DaysSince { get; set; } = [];
}
