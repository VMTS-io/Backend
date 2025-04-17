using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Specifications.VehicleSpecification;

public class VehicleIncludesSpecification : BaseSpecification<Vehicle>
{
    private void ApplyIncludes()
    {
        Includes.Add(v => v.VehicleCategory);
        Includes.Add(v => v.VehicleModel);
    }

    private void ApplyAllIncludes()
    {
        Includes.Add(v => v.VehicleCategory);
        Includes.Add(v => v.VehicleModel);
        Includes.Add(v => v.TripRequests);
        Includes.Add(v => v.TripReports);
        Includes.Add(v => v.MaintenaceReports);
        Includes.Add(v => v.MaintenaceRequests);
    }

    public VehicleIncludesSpecification(string id)
        : base(v => v.Id == id)
    {
        ApplyAllIncludes();
    }

    public VehicleIncludesSpecification(VehicleSpecParams specParams)
        : base(v =>
            (
                string.IsNullOrEmpty(specParams.PalletNumber)
                || v.PalletNumber == specParams.PalletNumber
            )
            && (!specParams.Status.HasValue || v.Status == specParams.Status)
            && (!specParams.MaxKMDriven.HasValue || v.KMDriven <= specParams.MaxKMDriven)
            && (specParams.MaxJoindYear.HasValue || v.JoindYear <= specParams.MaxJoindYear)
            && (
                string.IsNullOrEmpty(specParams.Search)
                || v.PalletNumber.Contains(
                    specParams.Search,
                    StringComparison.CurrentCultureIgnoreCase
                )
            )
            && (
                string.IsNullOrEmpty(specParams.CategoryId) || v.CategoryId == specParams.CategoryId
            )
            && (string.IsNullOrEmpty(specParams.ModelId) || v.ModelId == specParams.ModelId)
        )
    {
        ApplyIncludes();

        if (!string.IsNullOrEmpty(specParams.Sort))
        {
            switch (specParams.Sort)
            {
                // case "StatusAsc":
                //     AddOrderBy(v => v.Status);
                //     break;
                case "StatusDes":
                    AddOrderByDesc(v => v.Status);
                    break;
                case "KMDrivenAsc":
                    AddOrderBy(v => v.KMDriven);
                    break;
                case "KMDrivenDes":
                    AddOrderByDesc(v => v.KMDriven);
                    break;
                case "DateAsc":
                    AddOrderBy(v => v.JoindYear);
                    break;
                case "DateDes":
                    AddOrderByDesc(v => v.JoindYear);
                    break;
            }
        }
        else
        {
            AddOrderBy(v => v.Status);
        }

        // AddPaginaiton((specParams.PageIndex - 1), (specParams.PageSize));
    }
}
