using VMTS.Core.Entities.Report;

namespace VMTS.Core.Interfaces;

public interface IFaultPredictionService
{
    Task<FaultPredictionResult> PredictAsync(string faultDetails, string id);
}
