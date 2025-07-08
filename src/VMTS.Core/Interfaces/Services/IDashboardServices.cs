namespace VMTS.Core.Interfaces.Services;

public interface IDashboardServices
{
    Task<int> GetCountTripsWithFaultsAsync();

    Task<int> GetCountTripsWithoutFaultsAsync();

    Task<decimal> GetTotalMaintenanceCostAsync();

    Task<decimal> GetTotalFuelCostAsync();

    Task<int> GetTotalTripsAsync();

    Task<int> GetTotalVehicleCount();

    Task<int> GetVehicleUnderMaintenanceCount();

    Task<int> GetVehicleAvailableCount();
}
