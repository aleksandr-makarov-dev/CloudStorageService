using CloudStorage.Application.Common;
using CloudStorage.Application.Common.Exceptions;
using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudStorage.Application.Resources.RestoreResource;

internal sealed class RestoreResourceHandler(IApplicationDbContext dbContext, ILogger<RestoreResourceHandler> logger)
    : IRequestHandler<RestoreResourceCommand>
{
    public async ValueTask<Unit> Handle(RestoreResourceCommand command, CancellationToken cancellationToken)
    {
        var resource = await dbContext.Resources
            .IgnoreQueryFilters([QueryFilters.SoftDelete])
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (resource is null)
        {
            logger.LogWarning("Could not find resource with id {ResourceId}.", command.Id);
            throw new NotFoundException(nameof(Resource), command.Id);
        }

        resource.Restore();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}