using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceTrackingServices
{
    public Task Create(MaintenanceTracking entity);
    public Task UpdateAll(MaintenanceTracking entity);
}
