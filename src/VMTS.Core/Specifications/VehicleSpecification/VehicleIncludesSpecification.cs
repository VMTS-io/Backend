using System.Linq.Expressions;
using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Specifications.VehicleSpecification;

public class VehicleIncludesSpecification : BaseSpecification<Vehicle>
{
    public VehicleIncludesSpecification(string id)
        : base(v => v.Id == id)
    {
        ApplyAllIncludes();
    }

    public VehicleIncludesSpecification(Expression<Func<Vehicle, bool>> criteria)
        : base(criteria) { }

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
            && (!specParams.MaxKMDriven.HasValue || v.CurrentOdometerKM <= specParams.MaxKMDriven)
            && (!specParams.MaxJoindYear.HasValue || v.JoinedYear <= specParams.MaxJoindYear)
            && (
                string.IsNullOrEmpty(specParams.Search)
                || v.PalletNumber.ToLower().Contains(specParams.Search.ToLower())
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
    }

    private void ApplyAllIncludes()
    {
        IncludeStrings.Add("TripRequests.Driver");
        Includes.Add(v => v.TripReports);
        Includes.Add(v => v.MaintenaceInitialReports);
        Includes.Add(v => v.MaintenaceFinalReports);
        IncludeStrings.Add("MaintenaceRequests.Mechanic");
        IncludeStrings.Add("MaintenaceRequests.FinalReport.ChangedParts.Part");
        Includes.Add(v => v.VehicleModel.Category);
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
                    AddOrderBy(v => v.CurrentOdometerKM);
                    break;
                case "KMDrivenDes":
                    AddOrderByDesc(v => v.CurrentOdometerKM);
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
