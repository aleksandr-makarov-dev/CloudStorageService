using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace CloudStorage.Infrastructure.Storage;

public sealed class MinioStartupHostedService(
    IMinioClient minioClient,
    IOptions<MinioOptions> minioOptions,
    ILogger<MinioStartupHostedService> logger) : IHostedService
{
    private readonly MinioOptions _minioOptions = minioOptions.Value;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(_minioOptions.BucketName);

        var bucketName = _minioOptions.BucketName;

        logger.LogInformation("Checking MinIO bucket {BucketName}", bucketName);

        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(bucketName);

        var bucketExists = await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

        if (bucketExists)
        {
            logger.LogInformation("MinIO bucket {BucketName} already exists", bucketName);

            return;
        }

        logger.LogInformation("Creating MinIO bucket {BucketName}", bucketName);

        var makeBucketArgs = new MakeBucketArgs()
            .WithBucket(bucketName);

        await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);

        logger.LogInformation("MinIO bucket {BucketName} created", bucketName);
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}