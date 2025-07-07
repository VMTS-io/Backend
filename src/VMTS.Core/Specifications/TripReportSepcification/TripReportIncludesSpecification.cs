using System.Linq.Expressions;
using VMTS.Core.Entities.Trip;

namespace VMTS.Core.Specifications;

public class TripReportIncludesSpecification : BaseSpecification<TripReport>
{
    public TripReportIncludesSpecification(Expression<Func<TripReport, bool>> criteria) { }

    public TripReportIncludesSpecification(TripReportSpecParams specParams)
        : base(tr =>
            (string.IsNullOrEmpty(specParams.TripId) || tr.TripId == specParams.TripId)
            && (string.IsNullOrEmpty(specParams.DriverId) || tr.DriverId == specParams.DriverId)
            && (string.IsNullOrEmpty(specParams.VehicleId) || tr.VehicleId == specParams.VehicleId)
            && (
                !specParams.ReportDate.HasValue
                || tr.ReportedAt.Date == specParams.ReportDate.Value.Date
            )
        )
    {
        ApplyIncludes();
        ApplySorting(specParams);
        ApplyPagination(specParams);
    }

    public TripReportIncludesSpecification(string id)
        : base(tr => tr.Id == id)
    {
        ApplyIncludes();
    }

    private void ApplyIncludes()
    {
        Includes.Add(tr => tr.Driver);
        Includes.Add(tr => tr.Vehicle.VehicleModel.Category);
        Includes.Add(tr => tr.Vehicle.VehicleModel.Brand);
        Includes.Add(tr => tr.Trip);
    }

    private void ApplySorting(TripReportSpecParams specParams)
    {
        if (string.IsNullOrEmpty(specParams.Sort))
            return;

        switch (specParams.Sort)
        {
            case "DateAsc":
                AddOrderBy(tr => tr.ReportedAt);
                break;
            case "DateDesc":
                AddOrderByDesc(tr => tr.ReportedAt);
                break;
            // Add more sorting logic if needed
        }
    }

    private void ApplyPagination(TripReportSpecParams specParams)
    {
        AddPaginaiton(Math.Max(0, specParams.PageIndex - 1), specParams.PageSize);
    }
}
