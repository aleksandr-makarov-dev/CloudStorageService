using CloudStorage.Application.Common.Exceptions;
using CloudStorage.Application.Common.Extensions;
using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Application.Common.Mappings;
using CloudStorage.Application.Resources.ListResources;
using CloudStorage.Domain;
using Mediator;
using Microsoft.Extensions.Logging;

namespace CloudStorage.Application.Resources.UpdateResource;

internal sealed class UpdateResourceHandler(IApplicationDbContext dbContext, ILogger<UpdateResourceHandler> logger)
    : IRequestHandler<UpdateResourceCommand, ResourceResponse>
{
    public async ValueTask<ResourceResponse> Handle(UpdateResourceCommand command, CancellationToken cancellationToken)
    {
        var file = await dbContext.Resources.FindFileByIdAsync(command.Id, cancellationToken);

        if (file is null)
        {
            logger.LogWarning("Could not find resource with id {ResourceId}.", command.Id);
            throw new NotFoundException(nameof(Resource), command.Id);
        }

        file.Rename(command.Name);
        await dbContext.SaveChangesAsync(cancellationToken);

        return file.ToResourceResponse();
    }
}