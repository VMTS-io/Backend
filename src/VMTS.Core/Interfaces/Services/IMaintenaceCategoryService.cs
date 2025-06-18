using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceCategoryServices
{
    Task CreateCategoryAsync(MaintenaceCategory category);
    Task UpdateCategoryAsync(MaintenaceCategory category);
    Task DeleteCategoryAsync(string id);
    Task<IReadOnlyList<MaintenaceCategory>> GetAllCategoriesAsync();
    Task<MaintenaceCategory> GetCategoryByIdAsync(string id);
}
