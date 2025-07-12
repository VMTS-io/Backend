using VMTS.Core.Non_Entities_Class;

namespace VMTS.Core.Interfaces.Services;

public interface IDashboardServices
{
    Task<int> GetCountTripsWithFaultsAsync(int? month);

    Task<int> GetCountTripsWithoutFaultsAsync(int? month);

    Task<decimal> GetTotalMaintenanceCostAsync(int? month);

    Task<decimal> GetTotalFuelCostAsync(int? month);

    Task<int> GetTotalTripsAsync(int? month);

    Task<int> GetTotalVehicleCount();

    Task<int> GetVehicleUnderMaintenanceCount();

    Task<int> GetVehicleAvailableCount();
    Task<CostChartDto> GetTimeSeriesCostChartAsync();
}
