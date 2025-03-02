using VMTS.Core.Entities;
using VMTS.Core.Interfaces.Repositories;

namespace VMTS.Core.Interfaces.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    Task SaveChanges();

    IGenericRepository<T> GetRepo<T>()
        where T : BaseEntity;
}
