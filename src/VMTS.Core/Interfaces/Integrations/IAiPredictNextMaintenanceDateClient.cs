namespace VMTS.Core.Interfaces.Integrations;

public interface IAiPredictNextMaintenanceDateClient
{
    Task<int?> PostAsync(string json);
}
