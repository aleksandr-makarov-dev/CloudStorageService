namespace CloudStorage.Models;

public sealed record ResourceResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string ContentType { get; init; }
    public long ContentLength { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? LastModifiedAtUtc { get; init; }
};