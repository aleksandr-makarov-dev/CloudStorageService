using CloudStorage.Application.Resources.ListResources;
using CloudStorage.Domain;

namespace CloudStorage.Application.Common.Mappings;

public static class ResourceMappings
{
    public static ResourceResponse ToResourceResponse(this Resource resource)
    {
        return new ResourceResponse
        (
            resource.Id,
            resource.Name,
            resource.ContentType,
            resource.ContentLength,
            resource.IsFolder,
            resource.CreatedAtUtc,
            resource.LastModifiedAtUtc
        );
    }
}