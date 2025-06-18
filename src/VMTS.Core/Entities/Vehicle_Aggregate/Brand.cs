namespace VMTS.Core.Entities.Vehicle_Aggregate;

public class Brand : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Country { get; set; } = default!;
    public ICollection<VehicleModel> VehicleModels { get; set; } = [];
}
