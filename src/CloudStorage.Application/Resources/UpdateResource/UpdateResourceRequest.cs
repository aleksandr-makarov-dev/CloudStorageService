using CloudStorage.Application.Resources.ListResources;
using Mediator;

namespace CloudStorage.Application.Resources.UpdateResource;

public sealed record UpdateResourceRequest(Guid Id, string Name) : IRequest<ResourceResponse>;