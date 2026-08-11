namespace AxioVital.Infrastructure.Interoperability.Hl7;

/// <summary>
/// Abstraction for HL7 v2.x message processing.
/// Implementations will handle parsing, creating, and routing HL7 v2.x messages.
/// </summary>
public interface IHl7Service
{
    /// <summary>
    /// Parses a raw HL7 v2.x message string into a structured representation.
    /// </summary>
    Task<Hl7Message> ParseAsync(string rawMessage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an HL7 v2.x message from structured data.
    /// </summary>
    Task<string> CreateMessageAsync(Hl7Message message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an HL7 v2.x message to a configured endpoint.
    /// </summary>
    Task<Hl7Acknowledgment> SendAsync(string message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates an HL7 v2.x message structure.
    /// </summary>
    Task<Hl7ValidationResult> ValidateAsync(string rawMessage, CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents a parsed HL7 v2.x message.
/// </summary>
public class Hl7Message
{
    public string MessageType { get; set; } = string.Empty;
    public string TriggerEvent { get; set; } = string.Empty;
    public string ControlId { get; set; } = string.Empty;
    public string Version { get; set; } = "2.5.1";
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    public Dictionary<string, List<Dictionary<string, string>>> Segments { get; set; } = new();
}

/// <summary>
/// HL7 acknowledgment response.
/// </summary>
public class Hl7Acknowledgment
{
    public string AckCode { get; set; } = string.Empty;
    public string? TextMessage { get; set; }
    public bool IsAccepted => AckCode is "AA" or "CA";
}

/// <summary>
/// Result of HL7 message validation.
/// </summary>
public class Hl7ValidationResult
{
    public bool IsValid { get; set; }
    public IReadOnlyList<string> Errors { get; set; } = Array.Empty<string>();
}

/// <summary>
/// Configuration for HL7 v2.x connectivity.
/// </summary>
public class Hl7Settings
{
    public const string SectionName = "Hl7";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 2575;
    public int TimeoutSeconds { get; set; } = 30;
}
