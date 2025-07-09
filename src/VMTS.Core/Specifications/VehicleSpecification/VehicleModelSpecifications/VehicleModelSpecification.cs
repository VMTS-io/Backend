using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Specifications.VehicleSpecification.VehicleModelSpecifications;

public class VehicleModelSpecification : BaseSpecification<VehicleModel>
{
    public VehicleModelSpecification(string? categoryId)
        : base(vm => string.IsNullOrEmpty(categoryId) || vm.CategoryId == categoryId)
    {
        Includes.Add(vm => vm.Category);
        // Includes.Add(vm => vm.Brand);
    }
}
