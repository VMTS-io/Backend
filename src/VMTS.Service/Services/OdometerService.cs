using Microsoft.AspNetCore.Http;
using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Integrations;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Service.Integrations;

namespace VMTS.Service.Services;

public class OdometerService : IOdometerService
{
    private readonly IOdometerOcrClient _ocrClient;
    private readonly IUnitOfWork _unitOfwork;

    public OdometerService(IOdometerOcrClient ocrClient, IUnitOfWork unitOfWork)
    {
        _ocrClient = ocrClient;
        _unitOfwork = unitOfWork;
    }

    public async Task<int> ProcessAndUpdateOdometerReadingAsync(
        string vehicleId,
        Stream imageStream,
        string contentType,
        string fileName
    )
    {
        var extractedReading = await _ocrClient.ExtractOdometerReadingAsync(
            imageStream,
            contentType,
            fileName
        );

        var vehicle = await _unitOfwork.GetRepo<Vehicle>().GetByIdAsync(vehicleId);
        if (vehicle == null)
            throw new KeyNotFoundException("Vehicle not found.");

        vehicle.CurrentOdometerKM = extractedReading;
        _unitOfwork.GetRepo<Vehicle>().Update(vehicle);
        await _unitOfwork.SaveChanges();
        return vehicle.CurrentOdometerKM;
    }
}
