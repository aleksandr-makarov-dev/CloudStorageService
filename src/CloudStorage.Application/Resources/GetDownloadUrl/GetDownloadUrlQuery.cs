using Mediator;

namespace CloudStorage.Application.Resources.GetDownloadUrl;

public record GetDownloadUrlQuery(Guid Id) : IRequest<DownloadUrlResponse>;