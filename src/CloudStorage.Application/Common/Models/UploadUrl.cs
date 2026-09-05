namespace CloudStorage.Application.Common.Models;

public record UploadUrl(string Url, DateTime ExpiresAtUtc, Dictionary<string, string> FormFields);