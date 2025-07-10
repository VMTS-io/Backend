namespace VMTS.Core.Non_Entities_Class;

public class VehicleMaintenancePredictionItem
{
    public string Label { get; set; }
    public string Vehicle_Model { get; set; } = default!;
    public bool NeedsMaintenance { get; set; } // true for "Yes", false for "No"
}
