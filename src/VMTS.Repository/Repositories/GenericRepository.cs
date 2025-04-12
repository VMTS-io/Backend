using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities;
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

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
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
}
