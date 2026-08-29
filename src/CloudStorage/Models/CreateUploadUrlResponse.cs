namespace CloudStorage.Models;

public sealed record CreateUploadUrlResponse
{
    public Guid Id { get; init; }
    public string Url { get; init; }
    public DateTime ExpiresAtUtc { get; init; }
    public Dictionary<string, string> FormFields { get; init; } = new();
};