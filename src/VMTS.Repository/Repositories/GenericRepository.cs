using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.Specifications;
using VMTS.Repository.Data;

namespace VMTS.Repository.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : BaseEntity
{
    private readonly VTMSDbContext _dbContext;

    public GenericRepository(VTMSDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Attach(MaintenanceTracking entity)
    {
        _dbContext.MaintenanceTrackings.Attach(entity);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _dbContext
            .Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(Entity => Entity.Id == id);
    }

    public async Task CreateAsync(T entity)
    {
        await _dbContext.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbContext.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Remove(entity);
    }

    public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specs)
    {
        return await GetQuery(specs).ToListAsync();
    }

    public async Task<T?> GetByIdWithSpecificationAsync(ISpecification<T> specs)
    {
        return await GetQuery(specs).FirstOrDefaultAsync();
    }

    public async Task<int> GetCountAsync(ISpecification<T> specs)
    {
        return await GetQuery(specs).CountAsync();
    }

    private IQueryable<T> GetQuery(ISpecification<T> specs)
    {
        return SpecificationElvaluator<T>.BuildQuery(_dbContext.Set<T>(), specs);
    }

    public IQueryable<T> AsQueryable() => _dbContext.Set<T>().AsQueryable();

    public async Task<bool> ExistAsync(string id)
    {
        return await _dbContext.Set<T>().AnyAsync(entity => entity.Id == id);
    }

    public async Task<IReadOnlyList<T>> GetByIdsAsync(IEnumerable<string> ids)
    {
        return await _dbContext.Set<T>().Where(e => ids.Contains(e.Id)).ToListAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> range)
    {
        await _dbContext.AddRangeAsync(range);
    }

    public void UpdateRange(IEnumerable<T> range)
    {
        _dbContext.UpdateRange(range);
    }

    public async Task<decimal> SumWithSpecificationAsync(
        ISpecification<T> specification,
        Expression<Func<T, decimal>> selector
    )
    {
        return await GetQuery(specification).SumAsync(selector);
    }
}
