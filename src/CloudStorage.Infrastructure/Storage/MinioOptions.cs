namespace CloudStorage.Infrastructure.Storage;

public sealed class MinioOptions
{
    public const string SectionName = nameof(MinioOptions);

    public string BucketName { get; init; }
    public string Endpoint { get; init; }
    public string AccessKey { get; init; }
    public string SecretKey { get; init; }
    public bool UseSsl { get; init; }
}