namespace VMTS.Core.Specifications.DriverReports;

public class DriverFaultReportsIncludeSpecifications : BaseSpecification<FaultReport>
{
    public DriverFaultReportsIncludeSpecifications(DriverReportsSpecParams spec)
        : base(fr =>
            (string.IsNullOrEmpty(spec.TripId) || fr.TripId == spec.TripId)
            && (string.IsNullOrEmpty(spec.VehicleId) || fr.VehicleId == spec.VehicleId)
            && (string.IsNullOrEmpty(spec.DriverId) || fr.DriverId == spec.DriverId)
            && (!spec.FaultType.HasValue || fr.FaultType == spec.FaultType)
            && (!spec.From.HasValue || fr.ReportedAt >= spec.From)
            && (!spec.To.HasValue || fr.ReportedAt <= spec.To)
        )
    {
        AddIncludes();
        ApplySorting(spec);
        ApplyPagination(spec);
    }

    private void AddIncludes()
    {
        Includes.Add(fr => fr.Driver);
        Includes.Add(fr => fr.Vehicle.VehicleModel.Category);
        Includes.Add(fr => fr.Vehicle.VehicleModel.Brand);
        Includes.Add(fr => fr.Trip);
    }

    private void ApplySorting(DriverReportsSpecParams spec)
    {
        if (string.IsNullOrEmpty(spec.Sort))
            return;

        switch (spec.Sort)
        {
            case "DateAsc":
                AddOrderBy(fr => fr.ReportedAt);
                break;
            case "DateDesc":
                AddOrderByDesc(fr => fr.ReportedAt);
                break;
            // Optional extension
            // case "FaultTypeAsc":
            //     AddOrderBy(fr => fr.FaultType);
            //     break;
            // case "FaultTypeDesc":
            //     AddOrderByDesc(fr => fr.FaultType);
            //     break;
        }
    }

    private void ApplyPagination(DriverReportsSpecParams spec)
    {
        AddPaginaiton(Math.Max(0, spec.PageIndex - 1), spec.PageSize);
    }
}
