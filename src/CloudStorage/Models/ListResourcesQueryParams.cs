namespace CloudStorage.Models;

public sealed record ListResourcesQueryParams
{
    public Guid? ParentId { get; init; }
};