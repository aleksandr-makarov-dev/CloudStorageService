using System.ComponentModel.Design;
using CloudStorage.Application.Resources.ListResources;
using Mediator;

namespace CloudStorage.Application.Resources.CreateFolder;

public record CreateFolderCommand(string Name, Guid? ParentId = null) : IRequest<ResourceResponse>;