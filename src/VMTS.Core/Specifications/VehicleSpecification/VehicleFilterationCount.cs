using VMTS.Core.Entities;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Specifications.VehicleSpecification;

public class VehicleFilterationCount : BaseSpecification<Vehicle>
{
    public VehicleFilterationCount(VehicleSpecParams specParams) : base(v =>
    string.IsNullOrEmpty(specParams.Search) || v.Name.ToLower().Contains(specParams.Search) &&
    (string.IsNullOrEmpty(specParams.CategoryId) || v.CategoryId == specParams.CategoryId) &&
    (string.IsNullOrEmpty(specParams.ModelId) || v.ModelId == specParams.ModelId))
    {
        
    }



}