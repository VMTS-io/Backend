using System.Net.Http.Json;
using VMTS.Core.Entities.Ai;
using VMTS.Core.Entities.Report;
using VMTS.Core.Interfaces;
using VMTS.Core.Interfaces.UnitOfWork;

namespace VMTS.Service.Services;

public class FaultPredictionService : IFaultPredictionService
{
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;

    public FaultPredictionService(HttpClient httpClient, IUnitOfWork unitOfWork)
    {
        _httpClient = httpClient;
        _unitOfWork = unitOfWork;
    }

    public async Task<FaultPredictionResult> PredictAsync(string faultDetails, string id)
    {
        var modelUrl = await _unitOfWork.GetRepo<AiEndpointConfig>().GetByIdAsync(id);
        var payload = new { report_text = faultDetails };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{modelUrl.Url}/analyze_report",
                payload
            );
            var responseText = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new FaultPredictionResult { IsSuccess = false };
            }

            // Expecting: "Brakes issue, High"
            var parts = responseText
                .Trim('"') // Remove any wrapping quotes from raw response
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                var predictedType = parts[0].Trim();
                var predictedUrgency = parts[1].Trim();

                return new FaultPredictionResult
                {
                    IsSuccess = true,
                    predicted_issue = predictedType,
                    predicted_urgency = predictedUrgency,
                    ModelVersion = "v1",
                };
            }

            return new FaultPredictionResult { IsSuccess = false };
        }
        catch
        {
            return new FaultPredictionResult { IsSuccess = false };
        }
    }
}
