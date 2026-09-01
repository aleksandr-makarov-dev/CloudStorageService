using CloudStorage.Domain;
using CloudStorage.Models;

namespace CloudStorage.Services;

public interface IResourceService
{
    Task<CreateUploadUrlResponse> CreateUploadUrlAsync(CreateUploadUrlRequest request,
        CancellationToken cancellationToken = default);

    Task<ResourceResponse> CompleteUploadAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ResourceResponse>> ListAsync(ListResourcesQueryParams query,
        CancellationToken cancellationToken = default);

    Task<ResourceResponse> UpdateAsync(Guid id, UpdateResourceRequest request,
        CancellationToken cancellationToken = default);

    Task<ResourceResponse> CreateFolderAsync(CreateFolderRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}