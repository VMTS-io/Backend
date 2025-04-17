using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Specifications.VehicleSpecification;

namespace VMTS.Core.Interfaces.Services;

public interface IVehicleSerivces
{
    Task<Vehicle> GetVehicleByIdAsync(string id);
    Task<IReadOnlyList<Vehicle>> GetAllVehiclesAsync(VehicleSpecParams specParams);
    Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
    Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle);
    Task<bool> DeleteVehicleAsync(string id);
}
