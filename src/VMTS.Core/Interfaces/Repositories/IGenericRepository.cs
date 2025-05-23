using VMTS.Core.Entities;
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
    bool Exist(string id);
}
