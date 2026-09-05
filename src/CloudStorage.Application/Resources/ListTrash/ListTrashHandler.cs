using CloudStorage.Application.Common;
using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Application.Common.Mappings;
using CloudStorage.Application.Resources.ListResources;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace CloudStorage.Application.Resources.ListTrash;

internal sealed class ListTrashHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ListTrashQuery, IEnumerable<ResourceResponse>>
{
    public async ValueTask<IEnumerable<ResourceResponse>> Handle(ListTrashQuery request,
        CancellationToken cancellationToken)
    {
        var resources = await dbContext.Resources
            .IgnoreQueryFilters([QueryFilters.SoftDelete])
            .Where(x => x.IsDeleted)
            .OrderByDescending(x => x.DeletedAtUtc)
            .Select(ResourceProjections.ToResourceResponse)
            .ToListAsync(cancellationToken);

        return resources;
    }
}