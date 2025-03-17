using System.Linq.Expressions;
using VMTS.Core.Entities;
using VMTS.Core.Interfaces.Specifications;

namespace VMTS.Core.Specifications;

public class BaseSpecification<T> : ISpecification<T>
    where T : BaseEntity
{
    public Expression<Func<T, bool>> Criteria { get; set; } = null;
    public List<Expression<Func<T, object>>> Includes { get; set; } =
        new List<Expression<Func<T, object>>>();
    public Expression<Func<T, object>> OrderBy { get; set; } = null;
    public Expression<Func<T, object>> OrderByDesc { get; set; } = null;
    public int Skip { get; set; }
    public int Take { get; set; }
    public bool IsPaginaitonEnabled { get; set; }

    public BaseSpecification() { }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    public void AddOrderBy(Expression<Func<T, object>> orderByAsec)
    {
        OrderBy = orderByAsec;
    }

    public void AddOrderByDesc(Expression<Func<T, object>> orderByDesc)
    {
        OrderByDesc = orderByDesc;
    }

    public void AddPaginaiton(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPaginaitonEnabled = true;
    }
}

