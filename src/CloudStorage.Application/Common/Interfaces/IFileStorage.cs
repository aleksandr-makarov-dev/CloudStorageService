using CloudStorage.Application.Common.Models;

namespace CloudStorage.Application.Common.Interfaces;

public interface IFileStorage
{
    public Task<ObjectInfo?> GetObjectInfoAsync(string objectName, CancellationToken cancellationToken = default);

    public Task<UploadUrl> GetUploadUrlAsync(string objectName, string contentType, long contentLength,
        CancellationToken cancellationToken = default);

    public Task<DownloadUrl> GetDownloadUrlAsync(string key, string name, string contentType,
        CancellationToken cancellationToken = default);
}