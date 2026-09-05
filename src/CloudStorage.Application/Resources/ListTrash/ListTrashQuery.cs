using CloudStorage.Application.Resources.ListResources;
using Mediator;

namespace CloudStorage.Application.Resources.ListTrash;

public sealed record ListTrashQuery() : IRequest<IEnumerable<ResourceResponse>>;