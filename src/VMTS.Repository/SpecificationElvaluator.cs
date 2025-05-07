using Microsoft.EntityFrameworkCore;
using VMTS.Core.Entities;
using VMTS.Core.Interfaces.Specifications;

namespace VMTS.Repository;

public static class SpecificationElvaluator<T>
    where T : BaseEntity
{
    public static IQueryable<T> BuildQuery(IQueryable<T> inputQuery, ISpecification<T> specs)
    {
        var query = inputQuery;

        if (specs.Criteria is not null)
            query = query.Where(specs.Criteria);

        if (specs.IsPaginaitonEnabled)
            query = query.Skip(specs.Skip).Take(specs.Take);

        if (specs.OrderBy is not null)
            query = query.OrderBy(specs.OrderBy);

        if (specs.OrderByDesc is not null)
            query = query.OrderByDescending(specs.OrderByDesc);

        query = specs.Includes.Aggregate(
            query,
            (currentQuery, Expression) => currentQuery.Include(Expression)
        );

        query = specs.IncludeStrings.Aggregate(
            query,
            (currentQuery, Expression) => currentQuery.Include(Expression)
        );
        return query;
    }
}
