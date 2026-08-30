namespace AxioVital.Contracts.Requests;

/// <summary>
/// Request model for uploading a medical document.
/// Used as form data with multipart/form-data encoding.
/// </summary>
public sealed class UploadDocumentRequest
{
    /// <summary>
    /// Optional patient identifier to associate the document with.
    /// </summary>
    public Guid? PatientId { get; set; }

    /// <summary>
    /// Document category (e.g., "MedicalReport", "LabResult", "ImagingStudy").
    /// </summary>
    public string Category { get; set; } = "Other";

    /// <summary>
    /// Optional description of the document.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Optional comma-separated tags.
    /// </summary>
    public string? Tags { get; set; }
}
