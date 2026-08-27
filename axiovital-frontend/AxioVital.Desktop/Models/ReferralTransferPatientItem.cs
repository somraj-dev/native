namespace AxioVital.Desktop.Models;

public class ReferralTransferPatientItem
{
    public string MRN { get; set; } = string.Empty;
    public string AxioId { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string AgeGender { get; set; } = string.Empty;
    public string DOB { get; set; } = string.Empty;
    public string TransferType { get; set; } = string.Empty;
    public string SendingFacility { get; set; } = string.Empty;
    public string ReceivingFacility { get; set; } = string.Empty;
    public string RequestDate { get; set; } = string.Empty;
    public string TransferDate { get; set; } = string.Empty;
    public string AttendingPhysician { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusBadgeText { get; set; } = string.Empty;
    public string StatusForeground { get; set; } = "#15803D";
    public string StatusBackground { get; set; } = "#DCFCE7";
    public string StatusBorder { get; set; } = "#BBF7D0";
    public string Priority { get; set; } = "Routine";
    public string Reason { get; set; } = string.Empty;
    public string RowBackground { get; set; } = "#FFFFFF";
}
