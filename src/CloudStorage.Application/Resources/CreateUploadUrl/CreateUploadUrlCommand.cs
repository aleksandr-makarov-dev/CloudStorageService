using Mediator;

namespace CloudStorage.Application.Resources.CreateUploadUrl;

public sealed record CreateUploadUrlCommand(string Name, string ContentType, long ContentLength, Guid? ParentId)
    : IRequest<CreateUploadUrlResponse>;