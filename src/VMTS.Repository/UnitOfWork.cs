using System.Collections;
using VMTS.Core.Entities;
using VMTS.Core.Interfaces.Repositories;
using VMTS.Core.Interfaces.UnitOfWork;
using VMTS.Repository.Data;
using VMTS.Repository.Repositories;

namespace VMTS.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly VTMSDbContext _dbContext;
    private readonly Hashtable _repos;

    public UnitOfWork(VTMSDbContext dbContext)
    {
        _dbContext = dbContext;
        _repos = [];
    }

    public async Task<int> SaveChanges() => await _dbContext.SaveChangesAsync();

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _dbContext.DisposeAsync();
    }

    public IGenericRepository<T> GetRepo<T>()
        where T : BaseEntity
    {
        var key = typeof(T).Name;

        if (!_repos.ContainsKey(key))
            _repos.Add(key, new GenericRepository<T>(_dbContext));

        return _repos[key] as IGenericRepository<T>;
    }
}
