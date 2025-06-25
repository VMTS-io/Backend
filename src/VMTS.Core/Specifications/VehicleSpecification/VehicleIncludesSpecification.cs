using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Specifications.VehicleSpecification;

public class VehicleIncludesSpecification : BaseSpecification<Vehicle>
{
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
            && (
                string.IsNullOrEmpty(specParams.CategoryId)
                || v.VehicleModel.CategoryId == specParams.CategoryId
            )
            && (!specParams.Status.HasValue || v.Status == specParams.Status)
            && (!specParams.MaxKMDriven.HasValue || v.KMDriven <= specParams.MaxKMDriven)
            && (!specParams.MaxJoindYear.HasValue || v.JoinedYear <= specParams.MaxJoindYear)
            && (
                string.IsNullOrEmpty(specParams.Search)
                || v.PalletNumber.Contains(
                    specParams.Search,
                    StringComparison.CurrentCultureIgnoreCase
                )
            )
            && (string.IsNullOrEmpty(specParams.ModelId) || v.ModelId == specParams.ModelId)
            && (
                !specParams.TripDate.HasValue
                || !v.TripRequests.Any(tr =>
                    tr.Date >= specParams.TripDate.Value.Date
                    && tr.Date < specParams.TripDate.Value.Date.AddDays(1)
                )
            )
        )
    {
        ApplyIncludes();

        // if (specParams.TripDate.HasValue)
        //     Includes.Add(v =>
        //         v.TripRequests.Where(tr => tr.Date.Date >= specParams.TripDate.Value.Date)
        //     );

        ApplaySort(specParams.Sort);
        AddPaginaiton(specParams.PageIndex - 1, specParams.PageSize);
    }

    private void ApplyIncludes()
    {
        Includes.Add(v => v.VehicleModel.Category);
        Includes.Add(v => v.VehicleModel.Brand);
    }

    private void ApplyAllIncludes()
    {
        Includes.Add(v => v.TripRequests);
        Includes.Add(v => v.TripReports);
        Includes.Add(v => v.MaintenaceInitialReports);
        Includes.Add(v => v.MaintenaceFinalReports);
        Includes.Add(v => v.MaintenaceRequests);
        Includes.Add(v => v.VehicleModel.Category);
        Includes.Add(v => v.VehicleModel.Brand);
    }

    private void ApplaySort(string? sort)
    {
        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort)
            {
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
                    AddOrderBy(v => v.JoinedYear);
                    break;
                case "DateDes":
                    AddOrderByDesc(v => v.JoinedYear);
                    break;
            }
        }
        else
        {
            AddOrderBy(v => v.Status);
        }
    }
}
