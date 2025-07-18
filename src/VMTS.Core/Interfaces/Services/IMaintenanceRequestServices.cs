using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Specifications.Maintenance;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceRequestServices
{
    Task CreateAsync(MaintenaceRequest model, List<string> parts, string? falutReportId);
    Task UpdateAsync(MaintenaceRequest model, List<string> parts);
    Task DeleteAsync(string Id);
    Task<MaintenaceRequest> GetByIdAsync(string id);
    Task<IReadOnlyList<MaintenaceRequest>> GetAllAsync(MaintenanceRequestSpecParams specParams);
    Task<IReadOnlyList<MaintenaceRequest>> GetAllForUserAsync(
        MaintenanceRequestSpecParamsForMechanic mechanicSpecParams,
        string mechanicId
    );
}
