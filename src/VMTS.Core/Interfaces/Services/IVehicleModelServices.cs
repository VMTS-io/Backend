using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Interfaces.Services;

public interface IVehicleModelServices
{
    Task<VehicleModel> CreateVehicleModelAsync(VehicleModel entity);
    Task<VehicleModel> UpdateVehicleModelAsync(VehicleModel entity);
    Task DeleteVehicleModelAsync(string id);
    Task<IReadOnlyList<VehicleModel>> GetAllVehicleModelsAsync(string? categoryId, string? brandId);
}
