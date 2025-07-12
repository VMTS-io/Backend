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

    public async Task<byte[]> SendPrioritiesAndGetChartAsync(ChartRequestDto chartDto)
    {
        var endPoint = (await _unitOfWork.GetRepo<AiEndpointConfig>().GetAllAsync()).FirstOrDefault(
            aic => aic.Name == "FaultPriviledge"
        );

        var response = await _httpClient.PostAsJsonAsync($"{endPoint.Url}/urgency_chart", chartDto);

        var errorContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"AI error: {response.StatusCode}, body: {errorContent}");

        return await response.Content.ReadAsByteArrayAsync(); // base64 image
    }

    public async Task<CostChartDto> SendMonthlyCostsAndGetChartAsync(MonthlyCostsChartDto dto)
    {
        var endPoint = (await _unitOfWork.GetRepo<AiEndpointConfig>().GetAllAsync())
            .FirstOrDefault(aic => aic.Name == "FaultPriviledge");

        var response = await _httpClient.PostAsJsonAsync($"{endPoint.Url}/costs_chart", dto);
        response.EnsureSuccessStatusCode();

        var imageBytes = await response.Content.ReadAsByteArrayAsync();

        return new CostChartDto { ChartBytes = imageBytes };
    }
}
