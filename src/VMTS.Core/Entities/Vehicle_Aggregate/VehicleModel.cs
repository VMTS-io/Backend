namespace VMTS.Core.Entities.Vehicle_Aggregate;

public class VehicleModel : BaseEntity
{
    public string Name { get; set; }

    public DateTime Year { get; set; }

    public string Manufacturer { get; set; }

    public string FuelEfficiency { get; set; }

    public ICollection<Vehicle> Vehicle { get; set; } = new HashSet<Vehicle>();
}

