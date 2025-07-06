using System.Text;
using Newtonsoft.Json;
using VMTS.Core.Interfaces.Integrations;

namespace VMTS.Service.Integrations;

public class AiPredictNextMaintenanceDateClient : IAiPredictNextMaintenanceDateClient
{
    private readonly HttpClient _httpClient;
    private readonly string _modelUrl = "https://your-ngrok-url.ngrok.io/predict";

    public AiPredictNextMaintenanceDateClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int?> PostAsync(string json)
    {
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_modelUrl, content);

        if (!response.IsSuccessStatusCode)
            return null;

        var responseBody = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<MaintenancePrediction>(responseBody);

        return result is null ? null : (int)Math.Round(result.PredictedDaysToNextMaintenance);
    }
}

public class MaintenancePrediction
{
    [JsonProperty("predicted_days_to_next_maintenance")]
    public double PredictedDaysToNextMaintenance { get; set; }
}
