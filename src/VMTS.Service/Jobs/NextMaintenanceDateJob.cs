using VMTS.Core.Interfaces.Services;

namespace VMTS.Service.Jobs;

public class UpdateNextMaintenanceDateJob
{
    private readonly INextMaintenanceDateServices _nextMaintenanceDateServices;

    public UpdateNextMaintenanceDateJob(INextMaintenanceDateServices maintenanceTrackingServices)
    {
        _nextMaintenanceDateServices = maintenanceTrackingServices;
    }

    public async Task SetNextMaintenanceDate()
    {
        await _nextMaintenanceDateServices.UpdateNextVehicelMaintenanceDate();
    }
}
