using CloudStorage.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CloudStorage.Infrastructure.Persistence;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null) return ValueTask.FromResult(result);

        var entries = eventData.Context.ChangeTracker.Entries<ISoftDeletable>()
            .Where(x => x.State == EntityState.Deleted);

        var utcNow = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            entry.Entity.MarkDeleted(utcNow);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}