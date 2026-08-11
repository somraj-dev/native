namespace AxioVital.Infrastructure.Interoperability.Dicom;

/// <summary>
/// Abstraction for DICOM operations.
/// Implementations will handle DICOM image storage, retrieval, and query.
/// </summary>
public interface IDicomService
{
    /// <summary>
    /// Stores a DICOM file.
    /// </summary>
    Task<string> StoreAsync(Stream dicomStream, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a DICOM file by SOP Instance UID.
    /// </summary>
    Task<Stream?> RetrieveAsync(string sopInstanceUid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Queries DICOM instances with the given parameters.
    /// </summary>
    Task<IReadOnlyList<DicomInstanceInfo>> QueryAsync(DicomQueryParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a DICOM instance.
    /// </summary>
    Task DeleteAsync(string sopInstanceUid, CancellationToken cancellationToken = default);
}

/// <summary>
/// Information about a DICOM instance.
/// </summary>
public class DicomInstanceInfo
{
    public string SopInstanceUid { get; set; } = string.Empty;
    public string StudyInstanceUid { get; set; } = string.Empty;
    public string SeriesInstanceUid { get; set; } = string.Empty;
    public string? PatientId { get; set; }
    public string? PatientName { get; set; }
    public string? Modality { get; set; }
    public DateTime? StudyDate { get; set; }
    public string? StudyDescription { get; set; }
}

/// <summary>
/// Query parameters for DICOM search.
/// </summary>
public class DicomQueryParameters
{
    public string? PatientId { get; set; }
    public string? StudyInstanceUid { get; set; }
    public string? Modality { get; set; }
    public DateTime? StudyDateFrom { get; set; }
    public DateTime? StudyDateTo { get; set; }
}

/// <summary>
/// Configuration for DICOM connectivity.
/// </summary>
public class DicomSettings
{
    public const string SectionName = "Dicom";

    public string AeTitle { get; set; } = "AXIOVITAL";
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 104;
    public string StoragePath { get; set; } = string.Empty;
}
