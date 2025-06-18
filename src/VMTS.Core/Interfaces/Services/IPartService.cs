using VMTS.Core.Entities.Parts;

namespace VMTS.Core.Interfaces.Services;

public interface IPartService
{
    Task CreateAsync(Part part);
    Task UpdateAsync(Part part);
    Task DeleteAsync(string id);
    Task<Part> GetByIdAsync(string id);
    Task<IReadOnlyList<Part>> GetAllAsync();
}
