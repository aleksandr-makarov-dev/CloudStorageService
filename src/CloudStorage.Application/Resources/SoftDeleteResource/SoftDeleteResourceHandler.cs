using CloudStorage.Application.Common.Exceptions;
using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudStorage.Application.Resources.SoftDeleteResource;

internal sealed class SoftDeleteResourceHandler(
    IApplicationDbContext dbContext,
    ILogger<SoftDeleteResourceHandler> logger)
    : IRequestHandler<SoftDeleteResourceCommand>
{
    public async ValueTask<Unit> Handle(SoftDeleteResourceCommand command, CancellationToken cancellationToken)
    {
        var resource = await dbContext.Resources.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (resource is null)
        {
            logger.LogWarning("Could not find resource with id {ResourceId}.", command.Id);
            throw new NotFoundException(nameof(Resource), command.Id);
        }

        dbContext.Resources.Remove(resource);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}