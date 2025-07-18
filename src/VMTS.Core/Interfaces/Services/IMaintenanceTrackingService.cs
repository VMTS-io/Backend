using VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Non_Entities_Class;
using VMTS.Core.Specifications.MaintenanceTracking;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceTrackingService
{
    Task<IReadOnlyList<VehicleWithDueParts>> GetVehiclesWithDuePartsAsync(
        VehicleWithDuePartsSpecParams specParams
    );

    Task<IReadOnlyList<VehicleWithDueParts>> GetVehiclesPartsAsync(
        VehiclePartsSpecParams specParams
    );

    Task RecalculateAllAsync();
    Task RecalculateForVehicleAsync(VehicleWithDuePartsSpecParams specParams);
    Task RecalculateRowAsync(MaintenanceTracking tracking);
}
