using CloudStorage.Models;

namespace CloudStorage.Services;

public interface IResourceService
{
    Task<CreateUploadUrlResponse> CreateUploadUrlAsync(CreateUploadUrlRequest request,
        CancellationToken cancellationToken = default);
}