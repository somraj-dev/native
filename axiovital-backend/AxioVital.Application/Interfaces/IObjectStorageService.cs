namespace AxioVital.Application.Interfaces;

/// <summary>
/// Abstraction for S3-compatible object storage (MinIO, AWS S3, etc.).
/// </summary>
public interface IObjectStorageService
{
    /// <summary>
    /// Uploads an object to the specified bucket.
    /// </summary>
    Task<string> UploadAsync(string bucket, string objectName, Stream content, string contentType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads an object from the specified bucket.
    /// </summary>
    Task<Stream> DownloadAsync(string bucket, string objectName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an object from the specified bucket.
    /// </summary>
    Task DeleteAsync(string bucket, string objectName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets metadata for an object.
    /// </summary>
    Task<ObjectMetadata?> GetMetadataAsync(string bucket, string objectName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a presigned URL for temporary access.
    /// </summary>
    Task<string> GetPresignedUrlAsync(string bucket, string objectName, TimeSpan expiration, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ensures a bucket exists, creating it if necessary.
    /// </summary>
    Task EnsureBucketExistsAsync(string bucket, CancellationToken cancellationToken = default);
}

/// <summary>
/// Metadata about a stored object.
/// </summary>
public sealed class ObjectMetadata
{
    public string ObjectName { get; set; } = string.Empty;
    public long Size { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public DateTime LastModifiedUtc { get; set; }
    public Dictionary<string, string> CustomMetadata { get; set; } = new();
}
