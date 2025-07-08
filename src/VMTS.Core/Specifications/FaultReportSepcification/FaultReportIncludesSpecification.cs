using System.Linq.Expressions;
using VMTS.Core.Entities.Report;
using VMTS.Core.Specifications;
using VMTS.Core.Specifications.FaultReportSepcification;

public class FaultReportIncludesSpecification : BaseSpecification<FaultReport>
{
    public FaultReportIncludesSpecification(Expression<Func<FaultReport, bool>> criteria)
        : base(criteria) { }

    public FaultReportIncludesSpecification(FaultReportSpecParams specParams)
        : base(fr =>
            (string.IsNullOrEmpty(specParams.TripId) || fr.TripId == specParams.TripId)
            && (string.IsNullOrEmpty(specParams.VehicleId) || fr.VehicleId == specParams.VehicleId)
            && (string.IsNullOrEmpty(specParams.DriverId) || fr.DriverId == specParams.DriverId)
            && (
                !specParams.ReportDate.HasValue
                || fr.ReportedAt.Date == specParams.ReportDate.Value.Date
            )
            && (
                string.IsNullOrEmpty(specParams.FaultType)
                || fr.AiPredictedFaultType == specParams.FaultType
            )
        )
    {
        ApplyIncludes();
        ApplySorting(specParams);
        ApplyPagination(specParams);
    }

    public FaultReportIncludesSpecification(string id)
        : base(f => f.Id == id)
    {
        ApplyIncludes();
    }

    private void ApplyIncludes()
    {
        Includes.Add(fr => fr.Driver);
        Includes.Add(fr => fr.Vehicle.VehicleModel.Category);
        Includes.Add(fr => fr.Trip);
    }

    private void ApplySorting(FaultReportSpecParams specParams)
    {
        if (string.IsNullOrEmpty(specParams.Sort))
            return;

        switch (specParams.Sort)
        {
            case "DateAsc":
                AddOrderBy(fr => fr.ReportedAt);
                break;
            case "DateDesc":
                AddOrderByDesc(fr => fr.ReportedAt);
                break;
            case "FaultTypeAsc":
                AddOrderBy(fr => fr.AiPredictedFaultType);
                break;
            case "FaultTypeDesc":
                AddOrderByDesc(fr => fr.AiPredictedFaultType);
                break;
        }
    }

    private void ApplyPagination(FaultReportSpecParams specParams)
    {
        AddPaginaiton(Math.Max(0, specParams.PageIndex - 1), specParams.PageSize);
    }
}
