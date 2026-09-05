using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Application.Common.Mappings;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.Application.Resources.ListResources;

internal sealed class ListResourcesHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ListResourcesQuery, IEnumerable<ResourceResponse>>
{
    public async ValueTask<IEnumerable<ResourceResponse>> Handle(ListResourcesQuery request,
        CancellationToken cancellationToken)
    {
        var resources = await dbContext.Resources
            .Where(x => (x.IsFolder || x.IsUploaded) && !x.IsDeleted && x.ParentId == request.ParentId)
            .Select(ResourceProjections.ToResourceResponse)
            .ToListAsync(cancellationToken);

        return resources;
    }
}