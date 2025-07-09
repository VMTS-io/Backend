using System.Linq.Expressions;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Specifications.Maintenance.Report.Final;

public class MaintenanceFinalReportSpecification : BaseSpecification<MaintenanceFinalReport>
{
    public MaintenanceFinalReportSpecification(string id)
        : base(r => r.Id == id)
    {
        ApplyIncludes();
    }

    public MaintenanceFinalReportSpecification(
        Expression<Func<MaintenanceFinalReport, bool>> criteria
    )
        : base(criteria) { }

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

        ApplySort(specParams.Sort);

        AddPaginaiton((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
    }

    public MaintenanceFinalReportSpecification(MaintenanceReportSpecParams specParams)
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
                string.IsNullOrWhiteSpace(specParams.MaintenanceRequestId)
                || r.MaintenaceRequestId == specParams.MaintenanceRequestId
            )
            && (
                !specParams.ReportDate.HasValue
                || r.FinishedDate.Date == specParams.ReportDate.Value.Date
            )
            && (
                string.IsNullOrWhiteSpace(specParams.Search)
                || r.Notes.Contains(specParams.Search)
                || r.Vehicle.PalletNumber.Contains(specParams.Search)
            )
        )
    {
        ApplyIncludes();

        ApplySort(specParams.Sort);

        AddPaginaiton((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
    }

    private void ApplySort(string? sort)
    {
        if (!string.IsNullOrWhiteSpace(sort))
        {
            switch (sort)
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
    }

    private void ApplyIncludes()
    {
        Includes.Add(r => r.Vehicle.VehicleModel.Category);
        Includes.Add(r => r.Mechanic);
        Includes.Add(r => r.InitialReport);
        Includes.Add(r => r.MaintenaceRequest);
        // Includes.Add(r => r.ChangedParts);
        // Includes.Add(r => r.MaintenanceCategory);
        IncludeStrings.Add("ChangedParts.Part");
    }
}
