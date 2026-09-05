using Mediator;

namespace CloudStorage.Application.Resources.CreateUploadUrl;

public sealed record CreateUploadUrlRequest(string Name, string ContentType, long ContentLength, Guid? ParentId)
    : IRequest<CreateUploadUrlResponse>;