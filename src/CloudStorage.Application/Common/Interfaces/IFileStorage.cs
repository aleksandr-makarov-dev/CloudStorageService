using CloudStorage.Application.Common.Models;

namespace CloudStorage.Application.Common.Interfaces;

public interface IFileStorage
{
    public Task<ObjectInfo?> GetObjectInfoAsync(string objectName, CancellationToken cancellationToken = default);

    public Task<UploadUrl> GetUploadUrlAsync(string objectName, string contentType, long contentLength,
        TimeSpan timeToLive, CancellationToken cancellationToken = default);

    public Task<DownloadUrl> GetDownloadUrlAsync(string objectName, string name, string contentType,
        TimeSpan timeToLive,
        CancellationToken cancellationToken = default);

    public Task<IEnumerable<RemoveObjectError>> RemoveObjectsAsync(List<string> objectNames,
        CancellationToken cancellationToken = default);
}