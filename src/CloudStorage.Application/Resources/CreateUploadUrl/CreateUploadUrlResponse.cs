namespace CloudStorage.Application.Resources.CreateUploadUrl;

public sealed record CreateUploadUrlResponse(
    Guid Id,
    string Url,
    DateTime ExpirestAtUtc,
    Dictionary<string, string> FormFields);