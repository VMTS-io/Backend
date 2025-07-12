using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using VMTS.Core.Entities.Ai;
using VMTS.Core.Interfaces.Integrations;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Integrations;

public class OdometerOcrClient : IOdometerOcrClient
{
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;

    public OdometerOcrClient(HttpClient client, IUnitOfWork unitOfWork)
    {
        _httpClient = client;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> ExtractOdometerReadingAsync(
        Stream stream,
        string contentType,
        string fileName
    )
    {
        using var content = new MultipartFormDataContent();
        using var fileContent = new StreamContent(stream);
        var url = (await _unitOfWork.GetRepo<AiEndpointConfig>().GetAllAsync())[0].Url;

        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        content.Add(fileContent, "image", fileName);

        var response = await _httpClient.PostAsync($"{url}/read-odometer", content);

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<OdometerOcrResponse>(json);

        if (result == null ||string.IsNullOrEmpty( result.Reading )|| !response.IsSuccessStatusCode)
            throw new UnprocessableEntityException(
                "Failed to extraxt the reding from the photo try to type it manully."
            );
        int.TryParse(result.Reading, out var reading);
        return reading;
    }

    private class OdometerOcrResponse
    {
        [JsonPropertyName("odometer_text")]
        public string? Reading { get; set; }
    }
}
