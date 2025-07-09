using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Specifications.VehicleSpecification;

namespace VMTS.Core.Interfaces.Services;

public interface IVehicleSerivces
{
    Task<Vehicle> GetVehicleByIdAsync(string id);
    Task<IReadOnlyList<Vehicle>> GetAllVehiclesAsync(VehicleSpecParams specParams);
    Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
    public Task CreateVehicleWithHistoryAsync(
        Vehicle vehicle,
        List<MaintenanceTracking> maintenanceTracking
    );
    Task<Vehicle> UpdateVehicleAsync(Vehicle vehicle);
    Task<bool> DeleteVehicleAsync(string id);

    Task<decimal> GetTotalFuelCostAsync(string vehicleId);
    Task<decimal> GetTotalMaintenanceCostAsync(string vehicleId);
    Task AddHistoryToVehicleAsync(List<MaintenanceTracking> maintenanceTracking);
}
