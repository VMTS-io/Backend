using System.Net.Http.Json;
using System.Text.Json;
using VMTS.Core.Entities.Ai;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Non_Entities_Class;

namespace VMTS.Service.Services;

public class VehicleMaintenanceBulkPredictionService : IVehicleMaintenanceBulkPredictionService
{
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;

    public VehicleMaintenanceBulkPredictionService(HttpClient httpClient, IUnitOfWork unitOfWork)
    {
        _httpClient = httpClient;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<VehicleMaintenancePredictionItem>> PredictAsync(
        List<VehicleMaintenanceInputDto> vehicles
    )
    {
        var endpoint = await _unitOfWork.GetRepo<AiEndpointConfig>().GetAllAsync();
        var modelUrl = endpoint.FirstOrDefault(e => e.Name == "FaultPriviledge");

        if (modelUrl == null || string.IsNullOrWhiteSpace(modelUrl.Url))
            return new();

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{modelUrl.Url}/classify_many",
                vehicles
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[AI Error] Status: {response.StatusCode}, Body: {error}");
                return new();
            }

            var aiResult = await response.Content.ReadFromJsonAsync<PredictionResponse>();

            if (aiResult?.Predictions == null || aiResult.Predictions.Count != vehicles.Count)
                return new();

            var result = new List<VehicleMaintenancePredictionItem>();

            for (int i = 0; i < vehicles.Count; i++)
            {
                result.Add(
                    new VehicleMaintenancePredictionItem
                    {
                        Vehicle_Model = vehicles[i].Vehicle_Model,
                        NeedsMaintenance = aiResult.Predictions[i].Need_Maintenance,
                        Label = aiResult.Predictions[i].Prediction_Label,
                    }
                );
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Exception] {ex.Message}");
            return new();
        }
    }
}
