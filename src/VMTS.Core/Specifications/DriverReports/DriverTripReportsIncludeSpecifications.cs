using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Specifications.DriverReports;

public class DriverTripReportsIncludeSpecifications : BaseSpecification<TripReport>
{
    public DriverTripReportsIncludeSpecifications(DriverReportsSpecParams spec)
        : base(tr =>
            (string.IsNullOrEmpty(spec.DriverId) || tr.DriverId == spec.DriverId)
            && (string.IsNullOrEmpty(spec.TripId) || tr.TripId == spec.TripId)
            && (string.IsNullOrEmpty(spec.VehicleId) || tr.VehicleId == spec.VehicleId)
            && (!spec.From.HasValue || tr.ReportedAt >= spec.From)
            && (!spec.To.HasValue || tr.ReportedAt <= spec.To)
        )
    {
        AddIncludes();
        ApplySorting(spec);
        ApplyPagination(spec);
    }

    private void AddIncludes()
    {
        Includes.Add(tr => tr.Driver);
        Includes.Add(tr => tr.Vehicle.VehicleModel.Category);
        Includes.Add(tr => tr.Vehicle.VehicleModel.Brand);
        Includes.Add(tr => tr.Trip);
    }

    private void ApplySorting(DriverReportsSpecParams spec)
    {
        if (string.IsNullOrWhiteSpace(spec.Sort))
            return;

        switch (spec.Sort)
        {
            case "DateAsc":
                AddOrderBy(tr => tr.ReportedAt);
                break;
            case "DateDesc":
                AddOrderByDesc(tr => tr.ReportedAt);
                break;
            // You can extend this as needed
        }
    }

    private void ApplyPagination(DriverReportsSpecParams spec)
    {
        AddPaginaiton(Math.Max(0, spec.PageIndex - 1), spec.PageSize);
    }
}
