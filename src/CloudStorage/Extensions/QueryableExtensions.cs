using CloudStorage.Domain;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.Extensions;

public static class QueryableExtensions
{
    public static Task<TEntity?> FindByIdAsync<TEntity>(this IQueryable<TEntity> queryable, Guid id,
        CancellationToken cancellationToken = default) where TEntity : Entity
    {
        return queryable.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}