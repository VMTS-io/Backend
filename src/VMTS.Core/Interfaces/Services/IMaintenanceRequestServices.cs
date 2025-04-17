using System.Security.Claims;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Specifications.Maintenance;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceRequestServices
{
    Task<MaintenaceRequest> CreateAsync(MaintenaceRequest model, ClaimsPrincipal user);
    Task<MaintenaceRequest> UpdateAsync(MaintenaceRequest model, ClaimsPrincipal user);
    Task<MaintenaceRequest?> GetByIdAsync(string id);
    Task<IReadOnlyList<MaintenaceRequest>> GetAllAsync(MaintenanceRequestSpecParams specParams);
    Task<IReadOnlyList<MaintenaceRequest>> GetAllForUserAsync(
        MaintenanceRequestSpecParamsForMechanic mechanicSpecParams,
        ClaimsPrincipal user
    );
}
