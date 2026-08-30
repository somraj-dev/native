namespace AxioVital.Contracts.DTOs;

/// <summary>
/// Data transfer object for medical document metadata.
/// </summary>
public sealed class DocumentDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Guid? PatientId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Tags { get; set; }
    public string? UploadedByName { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string? PreviewUrl { get; set; }
}
