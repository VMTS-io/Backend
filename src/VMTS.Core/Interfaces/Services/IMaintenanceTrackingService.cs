using VMTS.API.Dtos.MaintenanceTrackingForGetVehicleInDue;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Non_Entities_Class;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceTrackingService
{
    Task<IReadOnlyList<VehicleWithDueParts>> GetVehiclesWithDuePartsAsync(
        VehicleWithDuePartsSpecParams specParams
    );
}
