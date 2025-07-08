using Newtonsoft.Json;

namespace VMTS.Core.Entities.Report;

public class FaultPredictionResult : BaseEntity
{
    [JsonProperty("predicted_issue")]
    public string PredictedType { get; set; }

    [JsonProperty("predicted_urgency")]
    public FaultPriority Priority { get; set; }
    public bool IsSuccess { get; set; }
    public string ModelVersion { get; set; }
}
