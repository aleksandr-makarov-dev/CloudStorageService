using CloudStorage.Application.Common.Exceptions;
using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudStorage.Application.Resources.GetDownloadUrl;

public record GetDownloadUrlQuery(Guid Id) : IRequest<DownloadUrlResponse>;

internal sealed class GetDownloadUrlHandler(
    IApplicationDbContext dbContext,
    IFileStorage fileStorage,
    ILogger<GetDownloadUrlHandler> logger) : IRequestHandler<GetDownloadUrlQuery, DownloadUrlResponse>
{
    public async ValueTask<DownloadUrlResponse> Handle(GetDownloadUrlQuery query,
        CancellationToken cancellationToken)
    {
        var resource = await dbContext.Resources.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (resource is null)
        {
            logger.LogWarning("Could not find resource with id {ResourceId}.", query.Id);
            throw new NotFoundException(nameof(Resource), query.Id);
        }

        // TODO: find a way to handle folders
        if (resource.IsFolder)
        {
            throw new NotImplementedException("Method not implemented.");
        }

        var downloadUrl =
            await fileStorage.GetDownloadUrlAsync(resource.Key, resource.Name, resource.ContentType, cancellationToken);

        return new DownloadUrlResponse(downloadUrl.Url, downloadUrl.ExpiresAtUtc);
    }
}