using System.Security.Claims;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Specifications.Maintenance;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceRequestServices
{
    Task CreateAsync(MaintenaceRequest model, ClaimsPrincipal user);
    Task UpdateAsync(MaintenaceRequest model);
    Task<MaintenaceRequest?> GetByIdAsync(string id);
    Task<IReadOnlyList<MaintenaceRequest>> GetAllAsync(MaintenanceRequestSpecParams specParams);
    Task<IReadOnlyList<MaintenaceRequest>> GetAllForUserAsync(
        string id,
        MaintenanceRequestSpecParamsForMechanic mechanicSpecParams
    );
}
