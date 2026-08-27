namespace AxioVital.Desktop.Models;

public class DischargePatientItem
{
    public string MRN { get; set; } = string.Empty;
    public string AxioId { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string AgeGender { get; set; } = string.Empty;
    public string DOB { get; set; } = string.Empty;
    public string AdmittedOn { get; set; } = string.Empty;
    public string DischargeDateTime { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string LocationUnit { get; set; } = string.Empty;
    public string AttendingPhysician { get; set; } = string.Empty;
    public string Status { get; set; } = "Discharged";
    public string StatusBadgeText { get; set; } = "Discharged";
    public string StatusForeground { get; set; } = "#059669";
    public string StatusBackground { get; set; } = "#ECFDF5";
    public string StatusBorder { get; set; } = "#A7F3D0";
    public string Disposition { get; set; } = string.Empty;
    public string DischargeSummaryStatus { get; set; } = string.Empty;
    public string SummaryStatusForeground { get; set; } = "#1E293B";
    public string RowBackground { get; set; } = "#FFFFFF";
    public bool IsPending { get; set; } = false;
    public string PendingActionNote { get; set; } = string.Empty;
}
