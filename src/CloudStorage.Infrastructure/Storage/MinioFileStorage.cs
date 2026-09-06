using CloudStorage.Application.Common.Interfaces;
using CloudStorage.Application.Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;

namespace CloudStorage.Infrastructure.Storage;

public class MinioFileStorage(
    IMinioClient minioClient,
    IOptions<MinioOptions> minioOptions,
    ILogger<MinioFileStorage> logger) : IFileStorage
{
    public async Task<ObjectInfo?> GetObjectInfoAsync(string objectName, CancellationToken cancellationToken = default)
    {
        var statObjectArgs = new StatObjectArgs()
            .WithBucket(minioOptions.Value.BucketName)
            .WithObject(objectName);

        try
        {
            var result = await minioClient.StatObjectAsync(statObjectArgs, cancellationToken);

            return new ObjectInfo(result.ObjectName, result.ContentType, result.Size);
        }
        catch (Minio.Exceptions.ObjectNotFoundException exception)
        {
            logger.LogWarning(exception, "Could not find '{ObjectName}' in object storage.", objectName);
            return null;
        }
    }

    public async Task<UploadUrl> GetUploadUrlAsync(string objectName, string contentType, long contentLength,
        TimeSpan timeToLive, CancellationToken cancellationToken = default)
    {
        var expiresAtUtc = DateTime.UtcNow.Add(timeToLive);

        var policy = new PostPolicy();
        policy.SetKey(objectName);
        policy.SetContentType(contentType);
        policy.SetContentRange(1, contentLength);
        policy.SetBucket(minioOptions.Value.BucketName);
        policy.SetExpires(expiresAtUtc);

        var presignedPostPolicyArgs = new PresignedPostPolicyArgs()
            .WithBucket(minioOptions.Value.BucketName)
            .WithExpiration(expiresAtUtc)
            .WithObject(objectName)
            .WithPolicy(policy);

        var (uri, formFields) = await minioClient.PresignedPostPolicyAsync(presignedPostPolicyArgs);

        return new UploadUrl(uri.ToString(), expiresAtUtc, formFields.ToDictionary());
    }

    public async Task<DownloadUrl> GetDownloadUrlAsync(string objectName, string name, string contentType,
        TimeSpan timeToLive, CancellationToken cancellationToken = default)
    {
        var expiresAtUtc = DateTime.UtcNow.Add(timeToLive);

        var contentDisposition =
            $"attachment; filename=\"{name}\"; filename*=UTF-8''{Uri.EscapeDataString(name)}";

        var headers = new Dictionary<string, string>
        {
            ["response-content-type"] = contentType,
            ["response-content-disposition"] = contentDisposition,
        };

        var args = new PresignedGetObjectArgs()
            .WithBucket(minioOptions.Value.BucketName)
            .WithObject(objectName)
            .WithExpiry(timeToLive.Seconds)
            .WithHeaders(headers);

        var url = await minioClient
            .PresignedGetObjectAsync(args);

        return new DownloadUrl(url, expiresAtUtc);
    }

    public async Task<IEnumerable<RemoveObjectError>> RemoveObjectsAsync(List<string> objectNames,
        CancellationToken cancellationToken = default)
    {
        var removeObjectsArgs = new RemoveObjectsArgs()
            .WithBucket(minioOptions.Value.BucketName)
            .WithObjects(objectNames);

        var errors = await minioClient.RemoveObjectsAsync(removeObjectsArgs, cancellationToken);

        return errors.Select(x => new RemoveObjectError(x.Key, x.Code, x.Message));
    }
}