using CloudStorage.Application.Common;
using CloudStorage.Application.Common.Exceptions;
using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudStorage.Application.Resources.CreateUploadUrl;

internal sealed class CreateUploadUrlHandler(
    IApplicationDbContext dbContext,
    IFileStorage fileStorage,
    ILogger<CreateUploadUrlHandler> logger)
    : IRequestHandler<CreateUploadUrlCommand, CreateUploadUrlResponse>
{
    public async ValueTask<CreateUploadUrlResponse> Handle(CreateUploadUrlCommand command,
        CancellationToken cancellationToken)
    {
        if (command.ParentId.HasValue &&
            !await dbContext.Resources.AnyAsync(x =>
                x.Id == command.ParentId && x.IsFolder, cancellationToken))
        {
            logger.LogWarning("Parent folder not found. ParentFolderId: {ParentFolderId}", command.ParentId.Value);

            throw new ConflictException($"Parent folder with id '{command.ParentId.Value}' does not exist.");
        }


        var utcNow = DateTime.UtcNow;

        var objectId = Guid.NewGuid();
        var objectKey = StorageKeyBuilder.Build(objectId, utcNow);

        var uploadUrl = await fileStorage.GetUploadUrlAsync(objectKey, command.ContentType, command.ContentLength,
            cancellationToken);

        var file = Resource.File(
            objectId,
            objectKey,
            command.Name,
            command.ContentType,
            command.ContentLength,
            command.ParentId
        );

        dbContext.Resources.Add(file);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateUploadUrlResponse(file.Id, uploadUrl.Url, uploadUrl.ExpiresAtUtc, uploadUrl.FormFields);
    }
}