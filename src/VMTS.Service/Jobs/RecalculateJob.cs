using VMTS.Core.Interfaces.Services;

namespace VMTS.Repository.Data.Jobs;

public class RecalculateJob
{
    private readonly IMaintenanceTrackingService _maintenanceService;

    public RecalculateJob(IMaintenanceTrackingService maintenanceService)
    {
        _maintenanceService = maintenanceService;
    }

    public async Task RunRecalculateAll()
    {
        await _maintenanceService.RecalculateAllAsync();
    }
}
