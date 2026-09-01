using CloudStorage.Domain;
using CloudStorage.Exceptions;
using CloudStorage.Extensions;
using CloudStorage.Mapping;
using CloudStorage.Models;
using CloudStorage.Options;
using CloudStorage.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;

namespace CloudStorage.Services;

internal sealed class ResourceService(
    ApplicationDbContext dbContext,
    IMinioClient minioClient,
    IOptions<MinioOptions> minioOptions,
    IOptions<StorageOptions> storageOptions,
    ILogger<ResourceService> logger) : IResourceService
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
        policy.SetContentRange(1, request.ContentLength);
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
            IsFolder = false,
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

    public async Task<ResourceResponse> CompleteUploadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var resource = await dbContext.Resources.FindByIdAsync(id, cancellationToken);

        if (resource is null)
        {
            throw new NotFoundException(nameof(Resource), id);
        }

        // TODO: check if file is fully uploaded.

        var utcNow = DateTime.UtcNow;

        resource.IsUploaded = true;
        resource.UploadedAtUtc = utcNow;
        resource.LastModifiedAtUtc = utcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return resource.ToResourceResponse();
    }

    public async Task<IReadOnlyList<ResourceResponse>> ListAsync(ListResourcesQueryParams query,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Resources
            .AsNoTracking()
            // TODO: replace with global filter for soft delete
            .Where(x => x.IsUploaded && !x.IsDeleted)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(ResourceProjections.ToResourceResponse)
            .ToListAsync(cancellationToken);
    }

    public async Task<ResourceResponse> UpdateAsync(Guid id, UpdateResourceRequest request,
        CancellationToken cancellationToken = default)
    {
        var resource = await dbContext.Resources.FindByIdAsync(id, cancellationToken);

        if (resource is null)
        {
            throw new NotFoundException(nameof(Resource), id);
        }

        // TODO: check if resource with the same name exists.

        resource.Name = request.Name;
        resource.LastModifiedAtUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return resource.ToResourceResponse();
    }

    public async Task<ResourceResponse> CreateFolderAsync(CreateFolderRequest request,
        CancellationToken cancellationToken = default)
    {
        var resource = new Resource
        {
            Name = request.Name,
            IsFolder = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        dbContext.Resources.Add(resource);
        await dbContext.SaveChangesAsync(cancellationToken);

        return resource.ToResourceResponse();
    }
}