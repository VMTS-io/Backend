using System.Linq.Expressions;
using VMTS.Core.Entities;
using VMTS.Core.Entities.Maintenace;
using VMTS.Core.Interfaces.Specifications;

namespace VMTS.Core.Interfaces.Repositories;

public interface IGenericRepository<T>
    where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
    Task CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specs);
    Task<T?> GetByIdWithSpecificationAsync(ISpecification<T> specs);
    Task<int> GetCountAsync(ISpecification<T> specs);
    Task<bool> ExistAsync(string id);
    Task<IReadOnlyList<T>> GetByIdsAsync(IEnumerable<string> ids);
    IQueryable<T> AsQueryable();
    Task AddRangeAsync(IEnumerable<T> range);
    void UpdateRange(IEnumerable<T> range);

    Task<decimal> SumWithSpecificationAsync(
        ISpecification<T> specification,
        Expression<Func<T, decimal>> selector
    );
}
