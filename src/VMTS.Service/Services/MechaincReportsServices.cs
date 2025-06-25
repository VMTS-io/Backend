using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Reports;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Specifications.Maintenance.Report;

namespace VMTS.Service.Services;

public class MechaincReportsServices : IMechanicReportsServices
{
    private readonly IGenericRepository<MaintenanceInitialReport> _initialReportRepo;
    private readonly IGenericRepository<MaintenanceFinalReport> _finalReportRepo;

    public MechaincReportsServices(
        IGenericRepository<MaintenanceInitialReport> initialReportRepo,
        IGenericRepository<MaintenanceFinalReport> finalReportRepo
    )
    {
        _initialReportRepo = initialReportRepo;
        _finalReportRepo = finalReportRepo;
    }

    public async Task<MechanicReportsResult> GetMechanicReportsAsync(
        string managerId,
        MaintenanceReportSpecParams specParams
    )
    {
        if (string.IsNullOrWhiteSpace(managerId))
            throw new UnauthorizedAccessException("You are not authorized.");

        var result = new MechanicReportsResult();

        if (string.IsNullOrEmpty(specParams.Filter) || specParams.Filter == "Initial")
        {
            var initialSpecs = new MechanicInitialReportSpecification(specParams);
            result.InitialReports = await _initialReportRepo.GetAllWithSpecificationAsync(
                initialSpecs
            );
        }

        if (string.IsNullOrEmpty(specParams.Filter) || specParams.Filter == "Final")
        {
            var finalSpecs = new MechanicFinalReportSpecification(specParams);
            result.FinalReports = await _finalReportRepo.GetAllWithSpecificationAsync(finalSpecs);
        }

        return result;
    }
}
