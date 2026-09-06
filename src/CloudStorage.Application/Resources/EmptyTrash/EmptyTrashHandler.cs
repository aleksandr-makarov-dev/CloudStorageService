using CloudStorage.Application.Common;
using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Application.Common.Options;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudStorage.Application.Resources.EmptyTrash;

internal sealed class EmptyTrashHandler(
    IApplicationDbContext dbContext,
    IFileStorage fileStorage,
    IOptions<StorageOptions> storageOptions,
    ILogger<EmptyTrashHandler> logger) : IRequestHandler<EmptyTrashCommand>
{
    public async ValueTask<Unit> Handle(EmptyTrashCommand request, CancellationToken cancellationToken)
    { 
        // Deleting resources from the trash at the user's initiative
        var resources = await dbContext.Resources
            .IgnoreQueryFilters([QueryFilters.SoftDelete])
            .Where(x => x.IsDeleted)
            .Select(x => new { x.Id, x.Key })
            .ToListAsync(cancellationToken);

        if (resources.Count == 0)
            return Unit.Value;

        var keys = resources
            .Where(x => !string.IsNullOrEmpty(x.Key))
            .Select(x => x.Key!)
            .Distinct()
            .ToList();

        await fileStorage.RemoveObjectsAsync(keys, cancellationToken);

        var resourceIdsToDelete = resources.Select(x => x.Id).ToList();

        await dbContext.Resources
            .IgnoreQueryFilters([QueryFilters.SoftDelete])
            .Where(x => resourceIdsToDelete.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken);

        logger.LogInformation("Permanently deleted {Count} resources.", resourceIdsToDelete.Count);

        return Unit.Value;
    }
}