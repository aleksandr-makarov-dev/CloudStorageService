namespace CloudStorage.Models;

public sealed record UpdateResourceRequest
{
    public string Name { get; init; }
}