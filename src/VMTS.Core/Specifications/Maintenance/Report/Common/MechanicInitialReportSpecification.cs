using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Specifications.Maintenance.Report;

public class MechanicInitialReportSpecification : BaseSpecification<MaintenanceInitialReport>
{
    public MechanicInitialReportSpecification(MaintenanceReportSpecParams spec)
        : base(fr =>
            (string.IsNullOrEmpty(spec.MechanicId) || fr.MechanicId == spec.MechanicId)
            && (string.IsNullOrEmpty(spec.VehicleId) || fr.VehicleId == spec.VehicleId)
            && (
                string.IsNullOrEmpty(spec.MaintenanceRequestId)
                || fr.MaintenanceRequestId == spec.MaintenanceRequestId
            )
            && (!spec.ReportDate.HasValue || fr.Date == spec.ReportDate.Value.Date)
        )
    {
        AddIncludes();
        ApplySorting(spec);
        ApplyPagination(spec);
    }

    private void AddIncludes()
    {
        Includes.Add(mir => mir.Vehicle.VehicleModel.Brand);
        Includes.Add(mir => mir.Vehicle.VehicleModel.Category);
        Includes.Add(mir => mir.Mechanic);
        Includes.Add(mir => mir.MaintenanceCategory);
        Includes.Add(mir => mir.MaintenanceRequest);
        Includes.Add(mir => mir.MissingParts);
        // Includes.Add(mir => mir.ExpectedChangedParts.Single().Part);
        IncludeStrings.Add("ExpectedChangedParts.Part");
    }

    private void ApplySorting(MaintenanceReportSpecParams spec)
    {
        if (string.IsNullOrEmpty(spec.Sort))
            return;

        switch (spec.Sort)
        {
            case "DateAsc":
                AddOrderBy(fr => fr.Date);
                break;
            case "DateDesc":
                AddOrderByDesc(fr => fr.Date);
                break;
        }
    }

    private void ApplyPagination(MaintenanceReportSpecParams spec)
    {
        AddPaginaiton(Math.Max(0, spec.PageIndex - 1), spec.PageSize);
    }
}
