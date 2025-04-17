using System.Security.Claims;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Entities.Report;

namespace VMTS.Core.ServicesContract;

public interface IReportService
{
    Task<FaultReport> CreateFaultReportAsync(
        string userEmail,
        string details,
        MaintenanceType faultType,
        string address);

   
}