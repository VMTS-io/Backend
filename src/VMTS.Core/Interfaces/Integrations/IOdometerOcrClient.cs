namespace VMTS.Core.Interfaces.Integrations
{
    public interface IOdometerOcrClient
    {
        Task<int> ExtractOdometerReadingAsync(Stream stream, string contentType, string fileName);
    }
}
