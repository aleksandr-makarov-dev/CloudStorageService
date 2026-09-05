using CloudStorage.Application.Resources.ListResources;
using Mediator;

namespace CloudStorage.Application.Resources.CompleteUpload;

public sealed record CompleteUploadRequest(Guid Id) : IRequest;