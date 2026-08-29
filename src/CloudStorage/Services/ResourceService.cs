using CloudStorage.Domain;
using CloudStorage.Models;
using CloudStorage.Options;
using CloudStorage.Persistence;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;

namespace CloudStorage.Services;

internal sealed class ResourceService(
    ApplicationDbContext dbContext,
    IMinioClient minioClient,
    IOptions<MinioOptions> minioOptions,
    IOptions<StorageOptions> storageOptions) : IResourceService
{
    private readonly MinioOptions _minioOptions = minioOptions.Value;
    private readonly StorageOptions _storageOptions = storageOptions.Value;

    public async Task<CreateUploadUrlResponse> CreateUploadUrlAsync(CreateUploadUrlRequest request,
        CancellationToken cancellationToken = default)
    {
        // TODO: check quota

        var utcNow = DateTime.UtcNow;

        var resourceId = Guid.NewGuid();
        var resourceKey = StorageKeyBuilder.Build(resourceId, utcNow);

        var expiresAtUtc = utcNow.AddMinutes(_storageOptions.UploadUrlTtlMinutes);

        var policy = new PostPolicy();
        policy.SetKey(resourceKey);
        policy.SetContentType(request.ContentType);
        policy.SetContentLength(request.ContentLength);
        policy.SetBucket(_minioOptions.BucketName);
        policy.SetExpires(expiresAtUtc);

        var presignedPostPolicyArgs = new PresignedPostPolicyArgs()
            .WithBucket(_minioOptions.BucketName)
            .WithExpiration(expiresAtUtc)
            .WithObject(resourceKey)
            .WithPolicy(policy);

        var (uri, formFields) = await minioClient.PresignedPostPolicyAsync(presignedPostPolicyArgs);

        var resource = new Resource
        {
            Id = resourceId,
            Key = resourceKey,
            Name = request.Name,
            ContentType = request.ContentType,
            ContentLength = request.ContentLength,
            CreatedAtUtc = utcNow,
        };

        dbContext.Resources.Add(resource);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateUploadUrlResponse
        {
            Id = resourceId,
            Url = uri.ToString(),
            ExpiresAtUtc = expiresAtUtc,
            FormFields = formFields.ToDictionary()
        };
    }
}