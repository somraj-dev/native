namespace AxioVital.Desktop.Models;

public class LabOrderItem
{
    public string PatientName { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string DetailsDate { get; set; } = string.Empty;
    public string DetailsNotes { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = "#008000";
    public string RowBackground { get; set; } = "#FFFFFF";
}
