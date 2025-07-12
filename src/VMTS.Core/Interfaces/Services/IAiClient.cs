using VMTS.Core.Non_Entities_Class;

namespace VMTS.Core.Interfaces.Services;

public interface IAiClient
{
    Task<byte[]> SendPrioritiesAndGetChartAsync(ChartRequestDto chartDto);

    Task<CostChartDto> SendMonthlyCostsAndGetChartAsync(MonthlyCostsChartDto dto);
}
