namespace VMTS.Core.Entities.Vehicle_Aggregate;

public class Vehicle : BaseEntity
{
    public string Name { get; set; }

    public string PalletNumber { get; set; }

    public DateTime JoindYear { get; set; }

    public FuelType FuelType { get; set; }

    public int KMDriven { get; set; }

    public VehicleStatus Status { get; set; }
    
}