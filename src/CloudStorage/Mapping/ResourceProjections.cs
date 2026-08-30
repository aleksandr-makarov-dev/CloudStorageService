using System.Linq.Expressions;
using CloudStorage.Domain;
using CloudStorage.Models;

namespace CloudStorage.Mapping;

public static class ResourceProjections
{
    public static readonly Expression<Func<Resource, ResourceResponse>> ToResourceResponse = x =>
        new ResourceResponse
        {
            Id = x.Id,
            Name = x.Name,
            ContentType = x.ContentType,
            ContentLength = x.ContentLength,
            CreatedAtUtc = x.CreatedAtUtc,
            LastModifiedAtUtc = x.LastModifiedAtUtc
        };
}