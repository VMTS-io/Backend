using System.Linq.Expressions;
using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Specifications.Maintenance.Report.Initial;

public class MaintenanceIntialReportSpecification : BaseSpecification<MaintenanceInitialReport>
{
    public MaintenanceIntialReportSpecification(string id)
        : base(mir => mir.Id == id)
    {
        ApplyIncludes();
    }

    public MaintenanceIntialReportSpecification(
        Expression<Func<MaintenanceInitialReport, bool>> criteria
    )
        : base(criteria) { }

    public MaintenanceIntialReportSpecification(MaintenanceIntialReportSpecParams specParams)
        : base(mir =>
            (
                string.IsNullOrWhiteSpace(specParams.MechanicId)
                || mir.MechanicId == specParams.MechanicId
            )
            && (
                string.IsNullOrWhiteSpace(specParams.VehicleId)
                || mir.VehicleId == specParams.VehicleId
            )
            && (
                string.IsNullOrWhiteSpace(specParams.MaintenanceRequestId)
                || mir.MaintenanceRequestId == specParams.MaintenanceRequestId
            )
            && (
                string.IsNullOrWhiteSpace(specParams.Search)
                || mir.Vehicle.PalletNumber.Contains(
                    specParams.Search,
                    StringComparison.CurrentCultureIgnoreCase
                )
            )
            && (!specParams.Date.HasValue || mir.Date == specParams.Date)
        )
    {
        ApplyIncludes();

        if (!string.IsNullOrWhiteSpace(specParams.Sort))
        {
            switch (specParams.Sort)
            {
                case "DateAsc":
                    AddOrderBy(mir => mir.Date);
                    break;
                case "DateDes":
                    AddOrderByDesc(mir => mir.Date);
                    break;
            }
        }
        else
            AddOrderBy(mir => mir.Date);
        AddPaginaiton((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
    }

    public MaintenanceIntialReportSpecification(MaintenanceReportSpecParams specParams)
        : base(mir =>
            (
                string.IsNullOrWhiteSpace(specParams.MechanicId)
                || mir.MechanicId == specParams.MechanicId
            )
            && (
                string.IsNullOrWhiteSpace(specParams.VehicleId)
                || mir.VehicleId == specParams.VehicleId
            )
            && (
                string.IsNullOrWhiteSpace(specParams.MaintenanceRequestId)
                || mir.MaintenanceRequestId == specParams.MaintenanceRequestId
            )
            && (
                !specParams.ReportDate.HasValue || mir.Date.Date == specParams.ReportDate.Value.Date
            )
            && (
                string.IsNullOrWhiteSpace(specParams.Search)
                || mir.Vehicle.PalletNumber.Contains(specParams.Search)
            )
        )
    {
        ApplyIncludes();
        ApplaySort(specParams.Sort);
        AddPaginaiton((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
    }

    private void ApplyIncludes()
    {
        Includes.Add(mir => mir.Vehicle.VehicleModel.Category);
        Includes.Add(mir => mir.Mechanic);
        // Includes.Add(mir => mir.MaintenanceCategory);
        Includes.Add(mir => mir.MaintenanceRequest);
        Includes.Add(mir => mir.MissingParts);
        IncludeStrings.Add("ExpectedChangedParts.Part");
    }

    private void ApplaySort(string? sort)
    {
        if (!string.IsNullOrWhiteSpace(sort))
        {
            switch (sort)
            {
                case "DateAsc":
                    AddOrderBy(mir => mir.Date);
                    break;
                case "DateDesc":
                    AddOrderByDesc(mir => mir.Date);
                    break;
            }
        }
        else
        {
            AddOrderByDesc(mir => mir.Date);
        }
    }
}
