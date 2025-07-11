using VMTS.Core.Non_Entities_Class;

namespace VMTS.Core.Interfaces.Services;

public interface IVehicleMaintenanceBulkPredictionService
{
    Task<List<VehicleMaintenancePredictionItem>> PredictAsync(
        List<VehicleMaintenanceInputDto> vehicles
    );
}
