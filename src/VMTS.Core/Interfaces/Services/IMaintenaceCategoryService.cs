using VMTS.Core.Entities.Maintenace;

namespace VMTS.Core.Interfaces.Services;

public interface IMaintenanceCategoryServices
{
    Task CreateCategoryAsync(MaintenaceCategories category);
    Task UpdateCategoryAsync(MaintenaceCategories category);
    Task DeleteCategoryAsync(string id);
    Task<IReadOnlyList<MaintenaceCategories>> GetAllCategoriesAsync();
    Task<MaintenaceCategories> GetCategoryByIdAsync(string id);
}
