using Hangfire;
using Microsoft.AspNetCore.Mvc;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Core.Non_Entities_Class;
using VMTS.Service.Jobs;

namespace VMTS.API.Controllers;

public class MaintenancePredictionController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVehicleMaintenanceBulkPredictionService _predictionService;

    public MaintenancePredictionController(
        IUnitOfWork unitOfWork,
        IVehicleMaintenanceBulkPredictionService predictionService
    )
    {
        _unitOfWork = unitOfWork;
        _predictionService = predictionService;
    }

    [HttpPost("predict-now")]
    public async Task<ActionResult<PredictionResponse>> PredictNowAsync()
    {
        var job = new MaintenancePredictionJob(_unitOfWork, _predictionService);
        var predictions = await job.RunAndUpdatePredictionsAsync();

        return Ok(new PredictionResponse { Predictions = predictions.ToList() });
    }
}
