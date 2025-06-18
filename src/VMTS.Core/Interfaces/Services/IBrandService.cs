using VMTS.Core.Entities.Vehicle_Aggregate;

namespace VMTS.Core.Interfaces.Services;

public interface IBrandService
{
    Task<IReadOnlyList<Brand>> GetAllAsync();
    Task<Brand> GetByIdAsync(string id);
    Task CreateAsync(Brand brand);
    Task UpdateAsync(Brand brand);
    Task DeleteAsync(string id);
}
