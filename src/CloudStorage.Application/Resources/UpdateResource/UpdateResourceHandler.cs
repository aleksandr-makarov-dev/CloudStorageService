using CloudStorage.Application.Common.Exceptions;
using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Application.Common.Mappings;
using CloudStorage.Application.Resources.ListResources;
using CloudStorage.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudStorage.Application.Resources.UpdateResource;

internal sealed class UpdateResourceHandler(IApplicationDbContext dbContext, ILogger<UpdateResourceHandler> logger)
    : IRequestHandler<UpdateResourceCommand, ResourceResponse>
{
    public async ValueTask<ResourceResponse> Handle(UpdateResourceCommand command, CancellationToken cancellationToken)
    {
        var resource = await dbContext.Resources.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (resource is null)
        {
            logger.LogWarning("Could not find resource with id {ResourceId}.", command.Id);
            throw new NotFoundException(nameof(Resource), command.Id);
        }

        resource.Rename(command.Name);
        await dbContext.SaveChangesAsync(cancellationToken);

        return resource.ToResourceResponse();
    }
}