using CloudStorage.Domain;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.Application.Common.Extensions;

internal static class QueryableExtensions
{
    internal static Task<bool> FolderExistsAsync(this IQueryable<Resource> query, Guid id,
        CancellationToken cancellationToken = default)
    {
        return query.AnyAsync(x => x.Id == id && x.IsFolder, cancellationToken);
    }

    internal static Task<Resource?> FindFileByIdAsync(this IQueryable<Resource> query, Guid id,
        CancellationToken cancellationToken = default)
    {
        return query.FirstOrDefaultAsync(x => x.Id == id && !x.IsFolder, cancellationToken);
    }
}