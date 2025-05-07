using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Specifications.VehicleSpecification.VehicleModelSpecification;

public class VehicleModelSpecification : BaseSpecification<VehicleModel>
{
    public VehicleModelSpecification()
    {
        Includes.Add(vm => vm.Category);
    }
}
