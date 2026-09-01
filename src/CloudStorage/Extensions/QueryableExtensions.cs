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

    public static Task<bool> FolderExistsByIdAsync(this IQueryable<Resource> queryable, Guid id,
        CancellationToken cancellationToken = default)
    {
        return queryable.AnyAsync(x => x.Id == id && x.IsFolder, cancellationToken);
    }
}