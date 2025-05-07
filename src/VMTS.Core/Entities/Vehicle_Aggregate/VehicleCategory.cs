namespace VMTS.Core.Entities.Vehicle_Aggregate;

public class VehicleCategory : BaseEntity
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public ICollection<VehicleModel> VehicleModels { get; set; } = [];
}
