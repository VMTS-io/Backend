using VMTS.Core.Entities.Parts;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Services;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Service.Exceptions;

namespace VMTS.Service.Services;

public class PartService : IPartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Part> _partRepo;

    public PartService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _partRepo = _unitOfWork.GetRepo<Part>();
    }

    public async Task CreateAsync(Part part)
    {
        await _partRepo.CreateAsync(part);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(Part part)
    {
        var existing =
            await _partRepo.GetByIdAsync(part.Id)
            ?? throw new NotFoundException($"Part with ID {part.Id} not found");

        existing.Name = part.Name;
        existing.Quantity = part.Quantity;
        existing.Cost = part.Cost;

        _partRepo.Update(existing);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var part =
            await _partRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Part with ID {id} not found");

        _partRepo.Delete(part);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Part> GetByIdAsync(string id)
    {
        return await _partRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Part with ID {id} not found");
    }

    public async Task<IReadOnlyList<Part>> GetAllAsync()
    {
        return await _partRepo.GetAllAsync();
    }

    public async Task<bool> IsExist(string id)
    {
        return await _partRepo.ExistAsync(id);
    }

    public async Task<Dictionary<string, Part>> ValidatePartIdsExistAsync(
        IEnumerable<string> partIds
    )
    {
        var partIdSet = partIds.ToHashSet();

        var foundParts = await _partRepo.GetByIdsAsync(partIdSet);
        var foundDict = foundParts.ToDictionary(p => p.Id);

        var missingIds = partIdSet.Except(foundDict.Keys).ToList();

        if (missingIds.Any())
            throw new NotFoundException(
                $"The following part IDs do not exist: {string.Join(", ", missingIds)}"
            );

        return foundDict;
    }
}
