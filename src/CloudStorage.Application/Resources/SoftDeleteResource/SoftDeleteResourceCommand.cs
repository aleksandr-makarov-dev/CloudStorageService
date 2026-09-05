using Mediator;

namespace CloudStorage.Application.Resources.SoftDeleteResource;

public sealed record SoftDeleteResourceCommand(Guid Id) : IRequest;