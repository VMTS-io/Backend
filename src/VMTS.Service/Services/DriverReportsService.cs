using VMTS.Core.Entities.Reports;
using VMTS.Core.Entities.Trip;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Specifications;
using VMTS.Core.Specifications.DriverReports;

namespace VMTS.Service.Services;

public class DriverReportsService : IDriverReportsService
{
    private readonly IGenericRepository<TripReport> _tripReportRepo;
    private readonly IGenericRepository<FaultReport> _faultReportRepo;

    public DriverReportsService(
        IGenericRepository<TripReport> tripReportRepo,
        IGenericRepository<FaultReport> faultReportRepo
    )
    {
        _tripReportRepo = tripReportRepo;
        _faultReportRepo = faultReportRepo;
    }

    public async Task<DriverReportsResult> GetDriverReportsAsync(
        string managerId,
        DriverReportsSpecParams specParams
    )
    {
        if (string.IsNullOrWhiteSpace(managerId))
            throw new UnauthorizedAccessException("You are not authorized.");

        var result = new DriverReportsResult();

        if (string.IsNullOrEmpty(specParams.Filter) || specParams.Filter == "Trip")
        {
            var tripSpec = new DriverTripReportsIncludeSpecifications(specParams);
            result.TripReports = await _tripReportRepo.GetAllWithSpecificationAsync(tripSpec);
        }

        if (string.IsNullOrEmpty(specParams.Filter) || specParams.Filter == "Fault")
        {
            var faultSpec = new DriverFaultReportsIncludeSpecifications(specParams);
            result.FaultReports = await _faultReportRepo.GetAllWithSpecificationAsync(faultSpec);
        }

        return result;
    }
}
