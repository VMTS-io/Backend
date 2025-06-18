using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Specifications.Maintenance.Report.Initial;

public class MaintenanceIntialReportSpecification : BaseSpecification<MaintenanceInitialReport>
{
    private void ApplyIncludes()
    {
        Includes.Add(mir => mir.Vehicle);
        Includes.Add(mir => mir.MaintenanceCategories);
        Includes.Add(mir => mir.MaintenanceRequest);
        Includes.Add(mir => mir.MissingParts);
    }

    public MaintenanceIntialReportSpecification(string id)
        : base(mir => mir.Id == id)
    {
        ApplyIncludes();
    }

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
    }
}
