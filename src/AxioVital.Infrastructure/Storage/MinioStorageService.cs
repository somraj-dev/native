using AxioVital.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace AxioVital.Infrastructure.Storage;

/// <summary>
/// MinIO configuration options.
/// </summary>
public class MinioSettings
{
    public const string SectionName = "Minio";

    public string Endpoint { get; set; } = "localhost:9000";
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public bool Secure { get; set; } = false;
}

/// <summary>
/// Object storage service implementation using native Minio C# SDK.
/// </summary>
public class MinioStorageService : IObjectStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioStorageService> _logger;

    public MinioStorageService(IMinioClient minioClient, ILogger<MinioStorageService> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<string> UploadAsync(string bucket, string objectName, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        await EnsureBucketExistsAsync(bucket, cancellationToken);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucket)
            .WithObject(objectName)
            .WithStreamData(content)
            .WithObjectSize(content.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);
        _logger.LogDebug("Uploaded {ObjectName} to bucket {Bucket}", objectName, bucket);

        return objectName;
    }

    public async Task<Stream> DownloadAsync(string bucket, string objectName, CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(bucket)
            .WithObject(objectName)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream));

        await _minioClient.GetObjectAsync(getObjectArgs, cancellationToken);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task DeleteAsync(string bucket, string objectName, CancellationToken cancellationToken = default)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucket)
            .WithObject(objectName);

        await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
        _logger.LogDebug("Deleted {ObjectName} from bucket {Bucket}", objectName, bucket);
    }

    public async Task<ObjectMetadata?> GetMetadataAsync(string bucket, string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName);

            var stat = await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);
            return new ObjectMetadata
            {
                ObjectName = stat.ObjectName,
                Size = stat.Size,
                ContentType = stat.ContentType,
                LastModifiedUtc = stat.LastModified,
                CustomMetadata = stat.MetaData != null
                    ? stat.MetaData.ToDictionary(k => k.Key, k => k.Value)
                    : new Dictionary<string, string>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Object metadata check failed for {Bucket}/{ObjectName}", bucket, objectName);
            return null;
        }
    }

    public async Task<string> GetPresignedUrlAsync(string bucket, string objectName, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        var presignedArgs = new PresignedGetObjectArgs()
            .WithBucket(bucket)
            .WithObject(objectName)
            .WithExpiry((int)expiration.TotalSeconds);

        return await _minioClient.PresignedGetObjectAsync(presignedArgs);
    }

    public async Task EnsureBucketExistsAsync(string bucket, CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucket);
            var exists = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

            if (!exists)
            {
                var makeBucketArgs = new MakeBucketArgs().WithBucket(bucket);
                await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                _logger.LogInformation("Created bucket {Bucket}", bucket);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not verify/create bucket {Bucket}", bucket);
        }
    }
}
