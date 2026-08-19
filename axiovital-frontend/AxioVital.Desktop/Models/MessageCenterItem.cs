namespace AxioVital.Desktop.Models;

public class MessageCenterItem
{
    public string PatientName { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string DetailsDate { get; set; } = string.Empty;
    public string DetailsDesc { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string OriginatorName { get; set; } = string.Empty;
    public string CreateDate { get; set; } = string.Empty;
    public string StopDate { get; set; } = string.Empty;
    public string StopType { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";

    public string Subject { get; set; } = string.Empty;
    public string DateTimeText { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string Priority { get; set; } = "Normal";
    public string PriorityColor { get; set; } = "#333333";
    public string StatusColor { get; set; } = "#28A745";
    public string RowBackground { get; set; } = "#FFFFFF";
}
