using CloudStorage.Application.Common.Exceptions;
using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Application.Common.Options;
using CloudStorage.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudStorage.Application.Resources.GetDownloadUrl;

internal sealed class GetDownloadUrlHandler(
    IApplicationDbContext dbContext,
    IFileStorage fileStorage,
    IOptions<StorageOptions> storageOptions,
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

        // TODO: Support generating download URLs for folders.
        if (resource.IsFolder)
        {
            throw new NotImplementedException("Method not implemented.");
        }

        var timeToLive = storageOptions.Value.DownloadUrlTtl;
        var downloadUrl = await fileStorage.GetDownloadUrlAsync(resource.Key, resource.Name, resource.ContentType,
            timeToLive, cancellationToken);

        return new DownloadUrlResponse(downloadUrl.Url, downloadUrl.ExpiresAtUtc);
    }
}