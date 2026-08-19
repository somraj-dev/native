namespace AxioVital.Desktop.Models;

public class OngoingActivityItem
{
    public string Location { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string PatientSubInfo { get; set; } = string.Empty;
    public string PhysicianContact { get; set; } = string.Empty;
    public bool IsPhysicianAssignLink { get; set; } = false;
    public string Diagnosis { get; set; } = string.Empty;
    public bool IsDiagnosisAddLink { get; set; } = false;
    public string RowBackground { get; set; } = "#FFFFFF";
}
