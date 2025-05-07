namespace VMTS.Core.Entities.Vehicle_Aggregate;

public class VehicleModel : BaseEntity
{
    public string Name { get; set; } = default!;

    public string Manufacturer { get; set; } = default!;

    public string FuelEfficiency { get; set; } = default!;

    public string CategoryId { get; set; } = default!;

    public VehicleCategory Category { get; set; } = default!;

    public ICollection<Vehicle> Vehicle { get; set; } = [];
}
