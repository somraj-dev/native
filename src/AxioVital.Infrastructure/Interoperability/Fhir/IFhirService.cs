namespace AxioVital.Infrastructure.Interoperability.Fhir;

/// <summary>
/// Abstraction for FHIR R4 operations.
/// Implementations will handle serialization, validation, and communication with FHIR servers.
/// </summary>
public interface IFhirService
{
    /// <summary>
    /// Reads a FHIR resource by type and ID.
    /// </summary>
    Task<T?> ReadAsync<T>(string resourceType, string id, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Creates a new FHIR resource.
    /// </summary>
    Task<string> CreateAsync<T>(T resource, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Updates an existing FHIR resource.
    /// </summary>
    Task UpdateAsync<T>(string id, T resource, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Searches FHIR resources with the given parameters.
    /// </summary>
    Task<IReadOnlyList<T>> SearchAsync<T>(string resourceType, IDictionary<string, string> parameters, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Validates a FHIR resource against the R4 specification.
    /// </summary>
    Task<FhirValidationResult> ValidateAsync<T>(T resource, CancellationToken cancellationToken = default) where T : class;
}

/// <summary>
/// Result of FHIR resource validation.
/// </summary>
public class FhirValidationResult
{
    public bool IsValid { get; set; }
    public IReadOnlyList<string> Issues { get; set; } = Array.Empty<string>();
}

/// <summary>
/// Configuration for FHIR R4 connectivity.
/// </summary>
public class FhirSettings
{
    public const string SectionName = "Fhir";

    public string BaseUrl { get; set; } = string.Empty;
    public string? AuthToken { get; set; }
    public int TimeoutSeconds { get; set; } = 30;
}
