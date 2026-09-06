namespace CloudStorage.Application.Common.Options;

public sealed class StorageOptions
{
    public const string SectionName = nameof(StorageOptions);

    public TimeSpan UploadUrlTtl { get; set; } = TimeSpan.FromMinutes(15);
    public TimeSpan DownloadUrlTtl { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan OrphanRetention { get; set; } = TimeSpan.FromMinutes(60);
    public TimeSpan DeleteRetention { get; set; } = TimeSpan.FromDays(7);
}