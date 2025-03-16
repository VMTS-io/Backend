using VMTS.Core.Entities.Report;

namespace VMTS.Core.Specifications.FaultReportSepcification;

public class FaultReportIncludesSpecification : BaseSpecification<FaultReport>
{
    private void ApplyIncludes()
    {
        Includes.Add(fr => fr.TripId); 
    }
    
    
    public FaultReportIncludesSpecification(FaultReportSpecParams specParams) : base(fr =>
            (string.IsNullOrEmpty(specParams.Search) || fr.Id == specParams.Search)&&
            (string.IsNullOrEmpty(specParams.VehicleId) || fr.VehicleId == specParams.VehicleId)&&
            (string.IsNullOrEmpty(specParams.TripId)|| fr.TripId == specParams.TripId)&&
            (specParams.FaultType == null || fr.FaultType == specParams.FaultType)&&
            (!specParams.ReportDate.HasValue || fr.ReportedAt == specParams.ReportDate)
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
        
        AddPaginaiton((specParams.PageIndex -1),(specParams.PageSize));
    }
    
}