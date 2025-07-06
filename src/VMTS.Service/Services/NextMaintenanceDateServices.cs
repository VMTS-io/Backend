using System.Text.Json;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Integrations;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Non_Entities_Class;
using VMTS.Core.Specifications.MaintenanceTracking;

namespace VMTS.Service.Services;

public class NextMaintenanceDateServices : INextMaintenanceDateServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<MaintenanceTracking> _trackingRepo;
    private readonly IGenericRepository<Vehicle> _vehicleRepo;
    private readonly IAiPredictNextMaintenanceDateClient _aiClient;
    private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public NextMaintenanceDateServices(
        IUnitOfWork unitOfWork,
        IAiPredictNextMaintenanceDateClient aiClient
    )
    {
        _unitOfWork = unitOfWork;
        _trackingRepo = _unitOfWork.GetRepo<MaintenanceTracking>();
        _vehicleRepo = _unitOfWork.GetRepo<Vehicle>();
        _aiClient = aiClient;
    }

    public async Task UpdateNextVehicelMaintenanceDate()
    {
        var trackingSpec = new TrackingDuePartsSpecification()
        {
            Includes =
            [
                mt => mt.Vehicle.VehicleModel.Brand,
                mt => mt.Vehicle.VehicleModel.Category,
            ],
        };
        var trackings = await _trackingRepo.GetAllWithSpecificationAsync(trackingSpec);

        var tackingGrouped = trackings.GroupBy(mt => mt.VehicleId);

        var httpclient = new HttpClient();
        foreach (var tracking in tackingGrouped)
        {
            var request = GetVehicleAiInputJson([.. tracking]);
            //call the httpclient here
            //var NextMaintenanceInDays= httpclient("3basy endpoitn")
            var response = await _aiClient.PostAsync(request);
            if (response is null)
                continue;
            var nextMaintenanceDate = DateTime.UtcNow.AddDays(30);
            // var nextMaintenanceDate = DateTime.UtcNow.AddDays(response.Value);
            var vehicle = tracking.First().Vehicle;
            vehicle.ExpectedNextMaintenanceDate = nextMaintenanceDate;
            _vehicleRepo.Update(vehicle);
        }
        await _unitOfWork.SaveChanges();
    }

    private static string GetVehicleAiInputJson(List<MaintenanceTracking> model)
    {
        var vehicle = model.First().Vehicle;
        var now = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(now);
        var daysSinceManufacture = today.DayNumber - vehicle.ModelYear.DayNumber;
        var dto = new MaintenanceTrackingAi
        {
            VehicleType = vehicle.VehicleModel.Category.Name,
            Make = vehicle.VehicleModel.Brand.Name,
            DrivingCondition = vehicle.DrivingCondition!.Value,
            AvgDailyKm = vehicle.CurrentOdometerKM / daysSinceManufacture,
            VehicleAge = DateTime.UtcNow.Year - vehicle.ModelYear.Year, //model.Vehicle.Age,
            TotalKm = vehicle.CurrentOdometerKM,
        };

        foreach (var tracking in model)
        {
            var partKey = tracking.Part.Name.Trim().ToLower().Replace(" ", "_");

            dto.KmSince[$"km_since_{partKey}"] = dto.TotalKm - tracking.KMAtLastChange;
            dto.DaysSince[$"days_since_{partKey}_change"] =
                today.DayNumber - DateOnly.FromDateTime(tracking.LastChangedDate).DayNumber;
        }

        // Combine vehicle info and both km_since & days_since into one flat dictionary
        var result = new Dictionary<string, object>
        {
            ["vehicle_type"] = dto.VehicleType,
            ["make"] = dto.Make,
            ["driving_condition"] = dto.DrivingCondition,
            ["avg_daily_km"] = dto.AvgDailyKm,
            ["vehicle_age"] = dto.VehicleAge,
            ["total_km"] = dto.TotalKm,
        };

        foreach (var kv in dto.KmSince)
            result[kv.Key] = kv.Value;

        foreach (var kv in dto.DaysSince)
            result[kv.Key] = kv.Value;

        var json = JsonSerializer.Serialize(result, _jsonOptions);

        return json;
    }
}
