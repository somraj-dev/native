namespace AxioVital.Desktop.Models;

public class AppointmentModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime AppointmentTime { get; set; }
    public string Department { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty;
    public string Status { get; set; } = "Scheduled";
    public string Notes { get; set; } = string.Empty;
}
