using Mediator;

namespace CloudStorage.Application.Resources.RestoreResource;

public sealed record RestoreResourceCommand(Guid Id) : IRequest;