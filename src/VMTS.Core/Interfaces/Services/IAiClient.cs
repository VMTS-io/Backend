using VMTS.Core.Non_Entities_Class;

namespace VMTS.Core.Interfaces.Services;

public interface IAiClient
{
    Task<string> SendPrioritiesAndGetChartAsync(ChartDto chartDto);
}
