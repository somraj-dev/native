namespace AxioVital.Domain.Enums;

/// <summary>
/// Categorizes medical documents for filtering and organization.
/// </summary>
public enum FileCategory
{
    /// <summary>General medical reports (discharge summaries, progress notes, etc.)</summary>
    MedicalReport = 0,

    /// <summary>Laboratory test results (blood work, urinalysis, etc.)</summary>
    LabResult = 1,

    /// <summary>Diagnostic imaging studies (X-ray, CT, MRI, ultrasound reports)</summary>
    ImagingStudy = 2,

    /// <summary>Prescriptions and medication orders</summary>
    Prescription = 3,

    /// <summary>Insurance documents, claim forms, EOBs</summary>
    InsuranceDocument = 4,

    /// <summary>Patient consent and authorization forms</summary>
    ConsentForm = 5,

    /// <summary>Referral letters and correspondence</summary>
    Referral = 6,

    /// <summary>Surgical and procedure notes</summary>
    SurgicalNote = 7,

    /// <summary>Uncategorized or miscellaneous documents</summary>
    Other = 99
}
