namespace AxioVital.Domain.Enums;

/// <summary>
/// Predefined system roles. Tenants may create custom roles in addition to these.
/// </summary>
public enum SystemRole
{
    /// <summary>System-wide administrator.</summary>
    SystemAdmin = 0,

    /// <summary>Tenant-level administrator.</summary>
    TenantAdmin = 1,

    /// <summary>Physician / care provider.</summary>
    Physician = 2,

    /// <summary>Nurse / clinical staff.</summary>
    Nurse = 3,

    /// <summary>Front-desk / receptionist.</summary>
    Receptionist = 4,

    /// <summary>Laboratory technician.</summary>
    LabTechnician = 5,

    /// <summary>Pharmacist.</summary>
    Pharmacist = 6,

    /// <summary>Patient.</summary>
    Patient = 7,

    /// <summary>Read-only viewer.</summary>
    Viewer = 8
}
