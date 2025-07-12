using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class MaintenanceCategoryServices : IMaintenanceCategoryServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<MaintenaceCategories> _categoryRepo;

    public MaintenanceCategoryServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _categoryRepo = _unitOfWork.GetRepo<MaintenaceCategories>();
    }

    #region Create
    public async Task CreateCategoryAsync(MaintenaceCategories category)
    {
        await _categoryRepo.CreateAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }
    #endregion

    #region Update
    public async Task UpdateCategoryAsync(MaintenaceCategories category)
    {
        await GetCategoryOrThrowAsync(category.Id);
        _categoryRepo.Update(category);
        await _unitOfWork.SaveChangesAsync();
    }
    #endregion

    #region Delete
    public async Task DeleteCategoryAsync(string id)
    {
        var category = await GetCategoryOrThrowAsync(id);
        _categoryRepo.Delete(category);
        await _unitOfWork.SaveChangesAsync();
    }
    #endregion

    #region Get All
    public async Task<IReadOnlyList<MaintenaceCategories>> GetAllCategoriesAsync()
    {
        return await _categoryRepo.GetAllAsync();
    }
    #endregion

    #region Get By Id
    public async Task<MaintenaceCategories> GetCategoryByIdAsync(string id)
    {
        return await GetCategoryOrThrowAsync(id);
    }
    #endregion

    #region Get or throw
    private async Task<MaintenaceCategories> GetCategoryOrThrowAsync(string id)
    {
        return await _categoryRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Category with ID {id} not found.");
    }
    #endregion
}
