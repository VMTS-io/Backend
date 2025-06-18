using VMTS.Core.Entities.Vehicle_Aggregate;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class BrandService : IBrandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Brand> _brandRepo;

    public BrandService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _brandRepo = _unitOfWork.GetRepo<Brand>();
    }

    public async Task<IReadOnlyList<Brand>> GetAllAsync()
    {
        return await _brandRepo.GetAllAsync();
    }

    public async Task<Brand> GetByIdAsync(string id)
    {
        return await _brandRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Brand with ID {id} not found");
    }

    public async Task CreateAsync(Brand brand)
    {
        await _brandRepo.CreateAsync(brand);
        await _unitOfWork.SaveChanges();
    }

    public async Task UpdateAsync(Brand brand)
    {
        var existing = await GetByIdAsync(brand.Id);
        existing.Name = brand.Name;
        existing.Country = brand.Country;
        _brandRepo.Update(existing);
        await _unitOfWork.SaveChanges();
    }

    public async Task DeleteAsync(string id)
    {
        var brand = await GetByIdAsync(id);
        _brandRepo.Delete(brand);
        await _unitOfWork.SaveChanges();
    }
}
