using CloudStorage.Domain;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<Resource> Resources { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}