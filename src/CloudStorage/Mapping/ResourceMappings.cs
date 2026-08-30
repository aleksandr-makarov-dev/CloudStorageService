using CloudStorage.Domain;
using CloudStorage.Models;

namespace CloudStorage.Mapping;

public static class ResourceMappings
{
    public static ResourceResponse ToResourceResponse(this Resource resource)
    {
        return new ResourceResponse
        {
            Id = resource.Id,
            Name = resource.Name,
            ContentType = resource.ContentType,
            ContentLength = resource.ContentLength,
            CreatedAtUtc = resource.CreatedAtUtc,
            LastModifiedAtUtc = resource.LastModifiedAtUtc
        };
    }
}