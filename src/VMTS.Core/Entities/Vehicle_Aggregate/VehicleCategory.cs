namespace VMTS.Core.Entities.Vehicle_Aggregate;

public class VehicleCategory : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }
    
    
    public ICollection<Vehicle> Vehicle { get; set; }  = new HashSet<Vehicle>();

}