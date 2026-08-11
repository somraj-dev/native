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
}
