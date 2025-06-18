using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Specifications.Maintenance.Report.Final;

public class MaintenanceFinalReportSpecification : BaseSpecification<MaintenanceFinalReport>
{
    private void ApplyIncludes()
    {
        Includes.Add(r => r.Vehicle);
        Includes.Add(r => r.InitialReport);
        Includes.Add(r => r.MaintenaceRequest);
        Includes.Add(r => r.ChangedParts);
        Includes.Add(r => r.MaintenanceCategories);
    }

    public MaintenanceFinalReportSpecification(string id)
        : base(r => r.Id == id)
    {
        ApplyIncludes();
    }

    public MaintenanceFinalReportSpecification(MaintenanceFinalReportSpecParams specParams)
        : base(r =>
            (
                string.IsNullOrWhiteSpace(specParams.MechanicId)
                || r.MechanicId == specParams.MechanicId
            )
            && (
                string.IsNullOrWhiteSpace(specParams.VehicleId)
                || r.VehicleId == specParams.VehicleId
            )
            && (
                string.IsNullOrWhiteSpace(specParams.MaintenaceRequestId)
                || r.MaintenaceRequestId == specParams.MaintenaceRequestId
            )
            && (
                string.IsNullOrWhiteSpace(specParams.InitialReportId)
                || r.InitialReportId == specParams.InitialReportId
            )
            && (
                !specParams.FinishedDate.HasValue
                || r.FinishedDate.Date == specParams.FinishedDate.Value.Date
            )
            && (
                string.IsNullOrWhiteSpace(specParams.Search)
                || r.Notes.Contains(specParams.Search, StringComparison.CurrentCultureIgnoreCase)
                || r.Vehicle.PalletNumber.Contains(
                    specParams.Search,
                    StringComparison.CurrentCultureIgnoreCase
                )
            )
        )
    {
        ApplyIncludes();

        // Sorting
        if (!string.IsNullOrWhiteSpace(specParams.Sort))
        {
            switch (specParams.Sort)
            {
                case "DateAsc":
                    AddOrderBy(r => r.FinishedDate);
                    break;
                case "DateDesc":
                    AddOrderByDesc(r => r.FinishedDate);
                    break;
                case "CostAsc":
                    AddOrderBy(r => r.TotalCost);
                    break;
                case "CostDesc":
                    AddOrderByDesc(r => r.TotalCost);
                    break;
                default:
                    AddOrderByDesc(r => r.FinishedDate);
                    break;
            }
        }
        else
        {
            AddOrderByDesc(r => r.FinishedDate);
        }

        // Pagination
        // ApplyPaging((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
    }
}
