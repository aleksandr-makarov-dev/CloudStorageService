using CloudStorage.Application.Common.Exceptions;
using CloudStorage.Application.Common.Extensions;
using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Domain;
using Mediator;
using Microsoft.Extensions.Logging;

namespace CloudStorage.Application.Resources.CompleteUpload;

internal sealed class CompleteUploadHandler(
    IApplicationDbContext dbContext,
    IFileStorage fileStorage,
    ILogger<CompleteUploadHandler> logger) : IRequestHandler<CompleteUploadCommand>
{
    public async ValueTask<Unit> Handle(CompleteUploadCommand command, CancellationToken cancellationToken)
    {
        var file = await dbContext.Resources.FindFileByIdAsync(command.Id, cancellationToken);

        if (file is null)
        {
            logger.LogWarning("Could not find resource with id {ResourceId}.", command.Id);
            throw new NotFoundException(nameof(Resource), command.Id);
        }

        var objectInfo = await fileStorage.GetObjectInfoAsync(file.Key, cancellationToken);

        if (objectInfo is null)
        {
            logger.LogWarning("Could not find resource with key {ResourceKey} in object storage.", file.Key);
            throw new ConflictException($"File with id '{file.Id}' was not uploaded.");
        }

        if (file.ContentLength.Value != objectInfo.ContentLength)
        {
            logger.LogWarning(
                "Uploaded file {ResourceId} has an unexpected size. Expected: {ExpectedSize} bytes, actual: {ActualSize} bytes.",
                file.Id, file.ContentLength.Value, objectInfo.ContentLength);

            throw new ConflictException($"Uploaded file with id '{file.Id}' has an invalid size.");
        }

        if (file.ContentType != objectInfo.ContentType)
        {
            logger.LogWarning(
                "Uploaded file {ResourceId} has an unexpected content type. Expected: {ExpectedContentType}, actual: {ActualContentType}.",
                file.Id, file.ContentType, objectInfo.ContentType);

            throw new ConflictException($"Uploaded file with id '{file.Id}' has an invalid content type.");
        }

        file.MarkUploaded(DateTime.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}