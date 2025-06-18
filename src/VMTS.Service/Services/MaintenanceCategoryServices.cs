using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class MaintenanceCategoryServices : IMaintenanceCategoryServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<MaintenaceCategory> _categoryRepo;

    public MaintenanceCategoryServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _categoryRepo = _unitOfWork.GetRepo<MaintenaceCategory>();
    }

    #region Create
    public async Task CreateCategoryAsync(MaintenaceCategory category)
    {
        await _categoryRepo.CreateAsync(category);
        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Update
    public async Task UpdateCategoryAsync(MaintenaceCategory category)
    {
        await GetCategoryOrThrowAsync(category.Id);
        _categoryRepo.Update(category);
        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Delete
    public async Task DeleteCategoryAsync(string id)
    {
        var category = await GetCategoryOrThrowAsync(id);
        _categoryRepo.Delete(category);
        await _unitOfWork.SaveChanges();
    }
    #endregion

    #region Get All
    public async Task<IReadOnlyList<MaintenaceCategory>> GetAllCategoriesAsync()
    {
        return await _categoryRepo.GetAllAsync();
    }
    #endregion

    #region Get By Id
    public async Task<MaintenaceCategory> GetCategoryByIdAsync(string id)
    {
        return await GetCategoryOrThrowAsync(id);
    }
    #endregion

    #region Get or throw
    private async Task<MaintenaceCategory> GetCategoryOrThrowAsync(string id)
    {
        return await _categoryRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Category with ID {id} not found.");
    }
    #endregion
}
