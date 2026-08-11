namespace AxioVital.Desktop.Models;

public class LoginDomainModel
{
    public string DomainName { get; set; } = "PROD";
    public string Description { get; set; } = "Production Environment";
    public bool IsActive { get; set; } = true;

    public static List<string> DefaultDomains => new()
    {
        "PROD",
        "STAGING",
        "DEV",
        "DEMO"
    };

    public static List<string> DefaultUsers => new()
    {
        "Select User...",
        "Dr. Alex Vance (Cardiology)",
        "Dr. Sarah Jenkins (Neurology)",
        "System Administrator",
        "Nurse Operator"
    };
}
