namespace CloudStorage.Application.Common.Models;

public sealed record UploadUrl(string Url, DateTime ExpiresAtUtc, Dictionary<string, string> FormFields);