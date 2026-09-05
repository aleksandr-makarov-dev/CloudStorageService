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
    : IRequestHandler<UpdateResourceRequest, ResourceResponse>
{
    public async ValueTask<ResourceResponse> Handle(UpdateResourceRequest request, CancellationToken cancellationToken)
    {
        var file = await dbContext.Resources.FindFileByIdAsync(request.Id, cancellationToken);

        if (file is null)
        {
            logger.LogWarning("Could not find resource with id {ResourceId}.", request.Id);
            throw new NotFoundException(nameof(Resource), request.Id);
        }

        file.Rename(request.Name);
        await dbContext.SaveChangesAsync(cancellationToken);

        return file.ToResourceResponse();
    }
}