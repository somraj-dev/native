namespace AxioVital.Desktop.Models;

public class QualityMeasureRecord
{
    public string AttendanceNumber { get; set; } = string.Empty;
    public string GenderDesc { get; set; } = string.Empty;
    public string Diagnosis1 { get; set; } = string.Empty;
    public string Diagnosis2 { get; set; } = string.Empty;
    public string Diagnosis3 { get; set; } = string.Empty;
}

public class AgeBandAttendanceData
{
    public string AgeBand { get; set; } = string.Empty;
    public double FemaleCount { get; set; }
    public double MaleCount { get; set; }

    public double FemaleHeight => (FemaleCount / 160.0) * 160.0;
    public double MaleHeight => (MaleCount / 160.0) * 160.0;
    public string ToolTipText => $"Age Band: {AgeBand}\nFemale: {FemaleCount}\nMale: {MaleCount}";
}
