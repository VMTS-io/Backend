namespace VMTS.Core.Entities.Report;

public class FaultPredictionResult
{
    public string predicted_issue { get; set; }

    public string predicted_urgency { get; set; }
    public bool IsSuccess { get; set; }
    public string ModelVersion { get; set; }
}
