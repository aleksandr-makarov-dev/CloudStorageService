using CloudStorage.Persistence;
using Minio;

namespace CloudStorage.Services;

internal sealed class ResourceService(ApplicationDbContext dbContext, IMinioClient minioClient) : IResourceService
{
}