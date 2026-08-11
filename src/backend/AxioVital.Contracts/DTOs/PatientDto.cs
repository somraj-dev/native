namespace AxioVital.Contracts.DTOs;

public class PatientDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MedicalRecordNumber { get; set; } = string.Empty;
}
