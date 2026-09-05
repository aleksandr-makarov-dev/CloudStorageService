using CloudStorage.Application.Resources.ListResources;
using Mediator;

namespace CloudStorage.Application.Resources.UpdateResource;

public sealed record UpdateResourceCommand(Guid Id, string Name) : IRequest<ResourceResponse>;