namespace CloudStorage.Models;

public sealed record CreateUploadUrlRequest
{
    public string Name { get; init; }
    public string ContentType { get; init; }
    public long ContentLength { get; init; }
};