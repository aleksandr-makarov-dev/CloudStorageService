using System.Linq.Expressions;
using CloudStorage.Application.Resources.ListResources;
using CloudStorage.Domain;

namespace CloudStorage.Application.Common.Mappings;

public static class ResourceProjections
{
    public static readonly Expression<Func<Resource, ResourceResponse>> ToResourceResponse = resource =>
        new ResourceResponse
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