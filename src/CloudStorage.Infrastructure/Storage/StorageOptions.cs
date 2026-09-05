namespace CloudStorage.Infrastructure.Storage;

public sealed class StorageOptions
{
    public const string SectionName = nameof(StorageOptions);

    public int UploadUrlTtlMinutes { get; set; } = 15;
    public int DownloadUrlTtlMinutes { get; set; } = 5;
    public int OrphanRetentionMinutes { get; set; } = 60;
    public int DeleteRetentionDays { get; set; } = 7;
}