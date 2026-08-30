using AxioVital.Contracts.DTOs;
using AxioVital.Domain.Enums;

namespace AxioVital.Application.Interfaces;

/// <summary>
/// Service for managing medical documents (upload, download, list, delete, preview).
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Uploads a document to object storage and persists its metadata.
    /// </summary>
    Task<DocumentDto> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        long fileSize,
        Guid? patientId,
        FileCategory category,
        string? description,
        string? tags,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a document's binary content.
    /// </summary>
    Task<(Stream Content, string ContentType, string FileName)> DownloadAsync(
        Guid documentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists documents with optional filters.
    /// </summary>
    Task<IReadOnlyList<DocumentDto>> ListAsync(
        Guid? patientId = null,
        FileCategory? category = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft-deletes a document (marks as deleted but retains data).
    /// </summary>
    Task DeleteAsync(Guid documentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a time-limited presigned URL for inline viewing/preview.
    /// </summary>
    Task<string> GetPreviewUrlAsync(
        Guid documentId,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default);
}
