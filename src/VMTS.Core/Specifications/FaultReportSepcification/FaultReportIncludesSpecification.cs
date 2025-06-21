using VMTS.Core.Entities.Report;

namespace VMTS.Core.Specifications.FaultReportSepcification;

public class FaultReportIncludesSpecification : BaseSpecification<FaultReport>
{
    public FaultReportIncludesSpecification(string id)
        : base(f => f.Id == id)
    {
        ApplyIncludes();
    }

    private void ApplyIncludes()
    {
        Includes.Add(fr => fr.Driver);
        Includes.Add(fr => fr.Vehicle.VehicleModel.Category);
        Includes.Add(fr => fr.Vehicle.VehicleModel.Brand);
        Includes.Add(fr => fr.Trip);
    }

    #region basic

    public FaultReportIncludesSpecification(FaultReportSpecParams specParams)
        : base(fr =>
            (string.IsNullOrEmpty(specParams.Search) || fr.Id == specParams.Search)
            && (string.IsNullOrEmpty(specParams.VehicleId) || fr.VehicleId == specParams.VehicleId)
            && (string.IsNullOrEmpty(specParams.TripId) || fr.TripId == specParams.TripId)
            && (specParams.FaultType == null || fr.FaultType == specParams.FaultType)
            && (!specParams.ReportDate.HasValue || fr.ReportedAt == specParams.ReportDate)
        )
    {
        ApplyIncludes();

        if (!string.IsNullOrEmpty(specParams.Sort))
        {
            switch (specParams.Sort)
            {
                case "DateAsc":
                    AddOrderBy(fr => fr.ReportedAt);
                    break;
                case "DateDesc":
                    AddOrderByDesc(fr => fr.ReportedAt);
                    break;
                case "FaultTypeAsc":
                    AddOrderBy(fr => fr.FaultType);
                    break;
                case "FaultTypeDesc":
                    AddOrderByDesc(fr => fr.FaultType);
                    break;
            }
        }

        AddPaginaiton(Math.Max(0, specParams.PageIndex - 1), specParams.PageSize);
    }

    #endregion

    #region with driver

    public FaultReportIncludesSpecification(FaultReportSpecParams specParams, string driverId)
        : base(fr =>
            fr.DriverId == driverId
            && (string.IsNullOrEmpty(specParams.Search) || fr.Id == specParams.Search)
            && (string.IsNullOrEmpty(specParams.VehicleId) || fr.VehicleId == specParams.VehicleId)
            && (string.IsNullOrEmpty(specParams.TripId) || fr.TripId == specParams.TripId)
            && (specParams.FaultType == null || fr.FaultType == specParams.FaultType)
            && (!specParams.ReportDate.HasValue || fr.ReportedAt == specParams.ReportDate)
        )
    {
        ApplyIncludes();

        if (!string.IsNullOrEmpty(specParams.Sort))
        {
            switch (specParams.Sort)
            {
                case "DateAsc":
                    AddOrderBy(fr => fr.ReportedAt);
                    break;
                case "DateDesc":
                    AddOrderByDesc(fr => fr.ReportedAt);
                    break;
                case "FaultTypeAsc":
                    AddOrderBy(fr => fr.FaultType);
                    break;
                case "FaultTypeDesc":
                    AddOrderByDesc(fr => fr.FaultType);
                    break;
            }
        }

        AddPaginaiton(Math.Max(0, specParams.PageIndex - 1), specParams.PageSize);
    }

    #endregion

    #region with vehicle

    public FaultReportIncludesSpecification(string vehicleId, FaultReportSpecParams specParams)
        : base(fr =>
            fr.VehicleId == vehicleId
            && (string.IsNullOrEmpty(specParams.Search) || fr.Id == specParams.Search)
            && (string.IsNullOrEmpty(specParams.VehicleId) || fr.VehicleId == specParams.VehicleId)
            && (string.IsNullOrEmpty(specParams.TripId) || fr.TripId == specParams.TripId)
            && (specParams.FaultType == null || fr.FaultType == specParams.FaultType)
            && (!specParams.ReportDate.HasValue || fr.ReportedAt == specParams.ReportDate)
        )
    {
        ApplyIncludes();

        if (!string.IsNullOrEmpty(specParams.Sort))
        {
            switch (specParams.Sort)
            {
                case "DateAsc":
                    AddOrderBy(fr => fr.ReportedAt);
                    break;
                case "DateDesc":
                    AddOrderByDesc(fr => fr.ReportedAt);
                    break;
                case "FaultTypeAsc":
                    AddOrderBy(fr => fr.FaultType);
                    break;
                case "FaultTypeDesc":
                    AddOrderByDesc(fr => fr.FaultType);
                    break;
            }
        }

        AddPaginaiton(Math.Max(0, specParams.PageIndex - 1), specParams.PageSize);
    }

    #endregion
}
