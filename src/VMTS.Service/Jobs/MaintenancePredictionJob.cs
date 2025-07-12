using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Parts;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Non_Entities_Class;

namespace VMTS.Service.Jobs;

public class MaintenancePredictionJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVehicleMaintenanceBulkPredictionService _predictionService;

    public MaintenancePredictionJob(
        IUnitOfWork unitOfWork,
        IVehicleMaintenanceBulkPredictionService predictionService
    )
    {
        _unitOfWork = unitOfWork;
        _predictionService = predictionService;
    }

    public async Task<IReadOnlyList<MaintenancePredictionItem>> RunAndUpdatePredictionsAsync()
    {
        var vehicles = await _unitOfWork.GetRepo<Vehicle>().GetAllAsync();
        var vehiclesList = vehicles.ToList(); // Convert to list for index access

        var dtoList = vehiclesList
            .Select(v => new VehicleMaintenanceInputDto
            {
                Vehicle_Model = v.VehicleModel?.Category?.Name ?? "Unknown",
                Reported_Issues = v.FaultReports?.Count ?? 0,
                Vehicle_Age = DateTime.UtcNow.Year - v.JoinedYear.Year,
                Fuel_Type = v.FuelType.ToString(),
                Transmission_Type = v.TransmissionType?.ToString() ?? "Unknown",
                Engine_Size = int.TryParse(v.EngineSize, out var engineSize) ? engineSize : 0,
                Odometer_Reading = v.CurrentOdometerKM,
                Last_Service_Date = v.LastMaintenanceDate?.ToString("yyyy-MM-dd") ?? "",
                Tire_Condition = v.TireCondition?.ToString() ?? "Unknown",
                Brake_Condition = v.BrakeCondition?.ToString() ?? "Unknown",
                Battery_Status = v.BatteryStatus?.ToString() ?? "Unknown",
            })
            .ToList();

        var predictions = await _predictionService.PredictAsync(dtoList);

        // Update vehicles with predictions
        for (int i = 0; i < vehiclesList.Count; i++)
        {
            var vehicle = vehiclesList[i];
            var prediction = predictions[i];
            vehicle.NeedMaintenancePrediction = prediction.NeedsMaintenance;
        }

        // Save changes to the database
        await _unitOfWork.SaveChangesAsync();

        return predictions
            .Select(p => new MaintenancePredictionItem
            {
                Need_Maintenance = p.NeedsMaintenance,
                Prediction_Label = p.NeedsMaintenance ? "Yes" : "No",
            })
            .ToList();
    }

    public async Task ExecuteAsync()
    {
        await RunAndUpdatePredictionsAsync();
    }
}
