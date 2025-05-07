using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Interfaces.Services;

public interface IVehicleCategoryServices
{
    Task<VehicleCategory> CreateVehicleCategoryAsync(VehicleCategory entity);
    Task<IReadOnlyList<VehicleCategory>> GetAllVehicleCategoryAsync();
    Task<VehicleCategory> UpdateVehicleCategory(VehicleCategory model);
    Task DeleteVehicleCategory(string id);
}
