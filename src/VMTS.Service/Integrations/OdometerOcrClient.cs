using System.Net.Http.Headers;
using System.Text.Json;
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
        content.Add(fileContent, "file", fileName);

        var response = await _httpClient.PostAsync($"{url}/ocr/odometer", content);

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<OdometerOcrResponse>(json);

        if (result == null || !result.Reading.HasValue || !response.IsSuccessStatusCode)
            throw new UnprocessableEntityException(
                "Failed to extraxt the reding from the photo try to type it manully."
            );

        return result.Reading.Value;
    }

    private class OdometerOcrResponse
    {
        public int? Reading { get; set; }
    }
}
