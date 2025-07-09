using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Interfaces.Services;

public interface IVehicleModelServices
{
    Task CreateVehicleModelAsync(VehicleModel entity);
    Task UpdateVehicleModelAsync(VehicleModel entity);
    Task DeleteVehicleModelAsync(string id);
    Task<IReadOnlyList<VehicleModel>> GetAllVehicleModelsAsync(string? categoryId);
}
