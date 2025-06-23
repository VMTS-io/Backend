using VMTS.Core.Entities.Reports;
using VMTS.Core.Specifications.DriverReports;

namespace VMTS.Core.Interfaces.Services;

public interface IDriverReportsService
{
    Task<DriverReportsResult> GetDriverReportsAsync(
        string managerId,
        DriverReportsSpecParams specParams
    );
}
