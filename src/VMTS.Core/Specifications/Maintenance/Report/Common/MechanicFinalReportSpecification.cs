using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Specifications.Maintenance.Report;

public class MechanicFinalReportSpecification : BaseSpecification<MaintenanceFinalReport>
{
    public MechanicFinalReportSpecification(MaintenanceReportSpecParams spec)
        : base(fr =>
            (string.IsNullOrEmpty(spec.MechanicId) || fr.MechanicId == spec.MechanicId)
            && (string.IsNullOrEmpty(spec.VehicleId) || fr.VehicleId == spec.VehicleId)
            && (
                string.IsNullOrEmpty(spec.MaintenanceRequestId)
                || fr.MaintenaceRequestId == spec.MaintenanceRequestId
            )
            && (!spec.ReportDate.HasValue || fr.FinishedDate.Date == spec.ReportDate.Value.Date)
        )
    {
        AddIncludes();
        ApplySorting(spec);
        ApplyPagination(spec);
    }

    private void AddIncludes()
    {
        Includes.Add(r => r.Vehicle.VehicleModel.Category);
        Includes.Add(r => r.Mechanic);
        Includes.Add(r => r.InitialReport);
        Includes.Add(r => r.MaintenaceRequest);
        // Includes.Add(r => r.ChangedParts);
        // Includes.Add(r => r.MaintenanceCategory);
        IncludeStrings.Add("ChangedParts.Part");
    }

    private void ApplySorting(MaintenanceReportSpecParams spec)
    {
        if (string.IsNullOrEmpty(spec.Sort))
            return;

        switch (spec.Sort)
        {
            case "DateAsc":
                AddOrderBy(fr => fr.FinishedDate);
                break;
            case "DateDesc":
                AddOrderByDesc(fr => fr.FinishedDate);
                break;
        }
    }

    private void ApplyPagination(MaintenanceReportSpecParams spec)
    {
        AddPaginaiton(Math.Max(0, spec.PageIndex - 1), spec.PageSize);
    }
}
