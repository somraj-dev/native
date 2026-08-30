using AxioVital.Application.Interfaces;
using AxioVital.Contracts.DTOs;
using AxioVital.Domain.Entities;
using AxioVital.Domain.Enums;
using AxioVital.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace AxioVital.Application.Services;

/// <summary>
/// Manages medical document lifecycle: upload, download, list, delete, and preview.
/// Files are stored in MinIO; metadata is persisted in PostgreSQL.
/// </summary>
public class DocumentService : IDocumentService
{
    private readonly IObjectStorageService _storageService;
    private readonly IRepository<MedicalDocument> _documentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantProvider _tenantProvider;
    private readonly ILogger<DocumentService> _logger;

    private const string DefaultBucket = "axiovital-documents";
    private const long MaxFileSizeBytes = 100 * 1024 * 1024; // 100 MB

    /// <summary>
    /// Whitelist of allowed file extensions.
    /// </summary>
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".doc", ".docx", ".txt", ".rtf", ".odt",
        ".xls", ".xlsx", ".csv", ".ods",
        ".jpg", ".jpeg", ".png", ".tiff", ".tif", ".bmp",
        ".dcm", ".dicom", ".nii",
        ".xml", ".json", ".hl7", ".cda",
        ".zip", ".rar", ".7z"
    };

    public DocumentService(
        IObjectStorageService storageService,
        IRepository<MedicalDocument> documentRepository,
        IUnitOfWork unitOfWork,
        ITenantProvider tenantProvider,
        ILogger<DocumentService> logger)
    {
        _storageService = storageService;
        _documentRepository = documentRepository;
        _unitOfWork = unitOfWork;
        _tenantProvider = tenantProvider;
        _logger = logger;
    }

    public async Task<DocumentDto> UploadAsync(
        Stream content,
        string fileName,
        string contentType,
        long fileSize,
        Guid? patientId,
        FileCategory category,
        string? description,
        string? tags,
        CancellationToken cancellationToken = default)
    {
        // Validate file size
        if (fileSize > MaxFileSizeBytes)
            throw new InvalidOperationException($"File exceeds maximum allowed size of {MaxFileSizeBytes / (1024 * 1024)} MB.");

        // Validate extension
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant() ?? string.Empty;
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
            throw new InvalidOperationException($"File extension '{extension}' is not allowed. Supported: {string.Join(", ", AllowedExtensions)}");

        // Generate unique storage key
        var tenantId = _tenantProvider.HasTenant ? _tenantProvider.TenantId : Guid.Empty;
        var now = DateTime.UtcNow;
        var storageObjectName = $"{tenantId}/{now:yyyy/MM}/{Guid.NewGuid()}{extension}";

        // Upload binary to object storage
        await _storageService.UploadAsync(DefaultBucket, storageObjectName, content, contentType, cancellationToken);

        // Persist metadata
        var document = new MedicalDocument
        {
            TenantId = tenantId,
            PatientId = patientId,
            FileName = fileName,
            StorageObjectName = storageObjectName,
            StorageBucket = DefaultBucket,
            ContentType = contentType,
            FileExtension = extension,
            FileSizeBytes = fileSize,
            Category = category,
            Description = description,
            Tags = tags,
            CreatedBy = null // Would come from current user context
        };

        await _documentRepository.AddAsync(document, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Uploaded document {FileName} ({Size} bytes) as {StorageKey}",
            fileName, fileSize, storageObjectName);

        return MapToDto(document);
    }

    public async Task<(Stream Content, string ContentType, string FileName)> DownloadAsync(
        Guid documentId,
        CancellationToken cancellationToken = default)
    {
        var document = await _documentRepository.GetByIdAsync(documentId, cancellationToken)
            ?? throw new KeyNotFoundException($"Document {documentId} not found.");

        var stream = await _storageService.DownloadAsync(document.StorageBucket, document.StorageObjectName, cancellationToken);
        return (stream, document.ContentType, document.FileName);
    }

    public async Task<IReadOnlyList<DocumentDto>> ListAsync(
        Guid? patientId = null,
        FileCategory? category = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var all = await _documentRepository.GetAllAsync(cancellationToken);
        var query = all.AsEnumerable();

        if (patientId.HasValue)
            query = query.Where(d => d.PatientId == patientId.Value);

        if (category.HasValue)
            query = query.Where(d => d.Category == category.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim();
            query = query.Where(d =>
                d.FileName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                (d.Description?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (d.Tags?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        return query
            .OrderByDescending(d => d.CreatedAtUtc)
            .Select(MapToDto)
            .ToList()
            .AsReadOnly();
    }

    public async Task DeleteAsync(Guid documentId, CancellationToken cancellationToken = default)
    {
        var document = await _documentRepository.GetByIdAsync(documentId, cancellationToken)
            ?? throw new KeyNotFoundException($"Document {documentId} not found.");

        // Soft-delete (AuditableEntity.IsDeleted)
        document.IsDeleted = true;
        document.DeletedAtUtc = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Soft-deleted document {DocumentId} ({FileName})", documentId, document.FileName);
    }

    public async Task<string> GetPreviewUrlAsync(
        Guid documentId,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        var document = await _documentRepository.GetByIdAsync(documentId, cancellationToken)
            ?? throw new KeyNotFoundException($"Document {documentId} not found.");

        var exp = expiration ?? TimeSpan.FromMinutes(15);
        return await _storageService.GetPresignedUrlAsync(document.StorageBucket, document.StorageObjectName, exp, cancellationToken);
    }

    private static DocumentDto MapToDto(MedicalDocument doc) => new()
    {
        Id = doc.Id,
        TenantId = doc.TenantId,
        PatientId = doc.PatientId,
        FileName = doc.FileName,
        ContentType = doc.ContentType,
        FileExtension = doc.FileExtension,
        FileSizeBytes = doc.FileSizeBytes,
        Category = doc.Category.ToString(),
        Description = doc.Description,
        Tags = doc.Tags,
        UploadedByName = doc.UploadedByName,
        CreatedAtUtc = doc.CreatedAtUtc
    };
}
