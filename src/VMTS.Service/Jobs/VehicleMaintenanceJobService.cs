// using VMTS.Core.Entities.Vehicle_Aggregate;
// using VMTS.Core.Interfaces.Repositories;
// using VMTS.Core.Interfaces.Services;
//
// namespace VMTS.Service.Jobs;
//
// public class VehicleMaintenanceJobService
// {
//     private readonly IAiPredictionService _aiPredictionService;
//     private readonly IGenericRepository<Vehicle> _vehicleRepository;
//     private readonly IVehicleSerivces _vehicleService;
//
//     public VehicleMaintenanceJobService(
//         IAiPredictionService aiPredictionService,
//         IGenericRepository<Vehicle> vehicleRepository,
//         IVehicleSerivces vehicleService
//     )
//     {
//         _aiPredictionService = aiPredictionService;
//         _vehicleRepository = vehicleRepository;
//         _vehicleService = vehicleService;
//     }
//
//     public async Task PredictAndUpdateMaintenanceDateAsync(string vehicleId)
//     {
//         var dto = await _vehicleService.PrepareVehiclePredictionDataAsync(vehicleId);
//
//         var predictedDays = await _aiPredictionService.GetNextMaintenanceInDaysAsync(dto);
//
//         var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
//         if (vehicle == null)
//             return;
//
//         vehicle.ExpectedNextMaintenanceDate = DateTime.UtcNow.AddDays(predictedDays);
//
//         _vehicleRepository.Update(vehicle);
//     }
// }
