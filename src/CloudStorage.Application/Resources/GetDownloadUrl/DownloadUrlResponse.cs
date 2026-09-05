namespace CloudStorage.Application.Resources.GetDownloadUrl;

public record DownloadUrlResponse(string Url, DateTime ExpiresAtUtc);