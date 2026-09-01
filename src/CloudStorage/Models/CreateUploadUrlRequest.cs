namespace CloudStorage.Models;

public sealed record CreateUploadUrlRequest
{
    public Guid? ParentId { get; init; }
    public string Name { get; init; }
    public string ContentType { get; init; }
    public long ContentLength { get; init; }
};