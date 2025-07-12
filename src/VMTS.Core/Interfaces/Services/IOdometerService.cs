namespace VMTS.Core.Interfaces.Services;

public interface IOdometerService
{
    Task<int> ProcessAndUpdateOdometerReadingAsync(
        string vehicleId,
        Stream imageStream,
        string contentType,
        string fileName
    );
}
