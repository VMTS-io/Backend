using VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Non_Entities_Class;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceTrackingService
{
    Task<IReadOnlyList<VehicleWithDueParts>> GetVehiclesWithDuePartsAsync(
        VehicleWithDuePartsSpecParams specParams
    );

    Task RecalculateAllAsync(VehicleWithDuePartsSpecParams specParams);
    Task RecalculateForVehicleAsync(VehicleWithDuePartsSpecParams specParams);
    Task RecalculateRowAsync(MaintenanceTracking tracking);
}
