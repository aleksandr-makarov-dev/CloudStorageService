using Mediator;

namespace CloudStorage.Application.Resources.ListResources;

public sealed record ListResourcesQuery(Guid? ParentId) : IRequest<IEnumerable<ResourceResponse>>;