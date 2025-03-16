using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Specifications.VehicleSpecification;

public class VehicleIncludesSpecification : BaseSpecification<Vehicle>
{
    private void ApplyIncludes()
    {
        Includes.Add(v => v.CategoryId);
        Includes.Add(v => v.ModelId);
    }

    public VehicleIncludesSpecification(string id) : base(v => v.Id == id)
    {
        ApplyIncludes();
    }
    
    public VehicleIncludesSpecification(VehicleSpecParams specParams)
        : base(
            v =>
            string.IsNullOrEmpty(specParams.Search) || v.Name.ToLower().Contains(specParams.Search) &&
            (string.IsNullOrEmpty(specParams.CategoryId) || v.CategoryId == specParams.CategoryId) &&
            (string.IsNullOrEmpty(specParams.ModelId) || v.ModelId == specParams.ModelId))
    {
        ApplyIncludes();

        if (!string.IsNullOrEmpty(specParams.Sort))
        {
            switch (specParams.Sort)
            {
                case "NameAsc":
                    AddOrderBy(v => v.Name);
                    break;
                case "NameDesc":
                    AddOrderByDesc(v => v.Name);
                    break;
            }
        }

        AddPaginaiton((specParams.PageIndex - 1),(specParams.PageSize));
    }







}