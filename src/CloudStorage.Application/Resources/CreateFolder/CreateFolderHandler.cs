using CloudStorage.Application.Common.Exceptions;
using CloudStorage.Application.Common.Extensions;
using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Application.Common.Mappings;
using CloudStorage.Application.Resources.ListResources;
using CloudStorage.Domain;
using Mediator;
using Microsoft.Extensions.Logging;

namespace CloudStorage.Application.Resources.CreateFolder;

internal sealed class CreateFolderHandler(IApplicationDbContext dbContext, ILogger<CreateFolderHandler> logger)
    : IRequestHandler<CreateFolderCommand, ResourceResponse>
{
    public async ValueTask<ResourceResponse> Handle(CreateFolderCommand command, CancellationToken cancellationToken)
    {
        // Check if parent folder exists and not marked for deletion
        if (command.ParentId.HasValue &&
            !await dbContext.Resources.FolderExistsAsync(command.ParentId.Value, cancellationToken))
        {
            logger.LogWarning("Parent folder not found. ParentFolderId: {ParentFolderId}", command.ParentId.Value);

            throw new ConflictException($"Parent folder with id '{command.ParentId.Value}' does not exist.");
        }

        var folder = Resource.Folder(command.Name, command.ParentId);

        dbContext.Resources.Add(folder);
        await dbContext.SaveChangesAsync(cancellationToken);

        return folder.ToResourceResponse();
    }
}