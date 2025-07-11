using System.Net.Http.Json;
using VMTS.Core.Entities.Ai;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Non_Entities_Class;

namespace VMTS.Service.Services;

public class AiClient : IAiClient
{
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;

    public AiClient(HttpClient httpClient, IUnitOfWork unitOfWork)
    {
        _httpClient = httpClient;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> SendPrioritiesAndGetChartAsync(ChartDto chartDto)
    {
        var endPoint = (await _unitOfWork.GetRepo<AiEndpointConfig>().GetAllAsync())
            .Where(aic => aic.Name == "FaultPriviledge")
            .FirstOrDefault();
        var response = await _httpClient.PostAsJsonAsync($"{endPoint.Url}/urgency_chart", chartDto);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
        }

        return await response.Content.ReadAsStringAsync();
    }
}
