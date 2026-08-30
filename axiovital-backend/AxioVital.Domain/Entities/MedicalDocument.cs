using AxioVital.Domain.Enums;

namespace AxioVital.Domain.Entities;

/// <summary>
/// Represents a medical document or file uploaded to the system.
/// Stores metadata while the actual binary is kept in object storage (MinIO).
/// </summary>
public class MedicalDocument : AuditableEntity
{
    /// <summary>
    /// Tenant owning this document (multi-tenant isolation).
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// Optional patient this document is associated with.
    /// Null if the document is organization-level (e.g., policies, templates).
    /// </summary>
    public Guid? PatientId { get; set; }

    /// <summary>
    /// Original file name as uploaded by the user (e.g., "Blood_Work_2024.pdf").
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Unique key used to store and retrieve the file in object storage.
    /// Format: {tenantId}/{yyyy/MM}/{guid}.{ext}
    /// </summary>
    public string StorageObjectName { get; set; } = string.Empty;

    /// <summary>
    /// The storage bucket name where the file resides.
    /// </summary>
    public string StorageBucket { get; set; } = "axiovital-documents";

    /// <summary>
    /// MIME content type (e.g., "application/pdf", "image/jpeg").
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// File extension including the dot (e.g., ".pdf", ".dcm").
    /// </summary>
    public string FileExtension { get; set; } = string.Empty;

    /// <summary>
    /// File size in bytes.
    /// </summary>
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// Document category for organization and filtering.
    /// </summary>
    public FileCategory Category { get; set; } = FileCategory.Other;

    /// <summary>
    /// Optional user-provided description of the document.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Comma-separated tags for flexible search and filtering.
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Display name of the user who uploaded this document.
    /// Denormalized for fast reads without joining User table.
    /// </summary>
    public string? UploadedByName { get; set; }
}
