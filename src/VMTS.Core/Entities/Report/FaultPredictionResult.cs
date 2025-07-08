using Newtonsoft.Json;

namespace VMTS.Core.Entities.Report;

public class FaultPredictionResult : BaseEntity
{
    public string predicted_issue { get; set; }

    public FaultPriority predicted_urgency { get; set; }
    public bool IsSuccess { get; set; }
    public string ModelVersion { get; set; }
}
