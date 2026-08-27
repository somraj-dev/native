using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using AxioVital.Desktop.Models;

namespace AxioVital.Desktop.Views;

public sealed partial class ReferralsTransferListView : UserControl
{
    public event EventHandler<PatientSelectedEventArgs>? PatientSelected;

    private readonly List<ReferralTransferPatientItem> _allPatients = new();

    public ReferralsTransferListView()
    {
        this.InitializeComponent();
        LoadSamplePatients();
        ApplyFilterAndRender();
    }

    private void LoadSamplePatients()
    {
        _allPatients.Clear();

        // 1. Pending Transfers / In Transit / Completed / Referrals
        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "1000245689",
            AxioId = "AXIO-MOCK-ID",
            PatientName = "AXIO, MOCK",
            AgeGender = "30 yrs / Male",
            DOB = "08/08/96",
            TransferType = "Facility Transfer",
            SendingFacility = "City Care - Med 3W",
            ReceivingFacility = "Metro Advanced Hospital",
            RequestDate = "26-Aug-2026 09:15 AM",
            TransferDate = "26-Aug-2026 02:00 PM",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Pending Authorization",
            StatusBadgeText = "⏳ Pending Auth",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Priority = "High",
            Reason = "Advanced Cardiac Intervention (Cath Lab)"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801002",
            AxioId = "AXIO-88201",
            PatientName = "SMITH, ELEANOR",
            AgeGender = "62 yrs / Female",
            DOB = "04/12/64",
            TransferType = "Specialist Referral",
            SendingFacility = "Axio Ambulatory Clinic",
            ReceivingFacility = "Northwestern Cardiology",
            RequestDate = "26-Aug-2026 08:30 AM",
            TransferDate = "28-Aug-2026 10:00 AM",
            AttendingPhysician = "Dr. S. Miller, MD",
            Status = "Accepted / Scheduled",
            StatusBadgeText = "✓ Accepted",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Priority = "Routine",
            Reason = "Complex Arrhythmia & EP Study"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801004",
            AxioId = "AXIO-88203",
            PatientName = "GONZALEZ, CARLOS",
            AgeGender = "45 yrs / Male",
            DOB = "11/20/80",
            TransferType = "Facility Transfer",
            SendingFacility = "City Care - ICU 2A",
            ReceivingFacility = "University Level 1 Trauma",
            RequestDate = "26-Aug-2026 10:45 AM",
            TransferDate = "26-Aug-2026 11:30 AM",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "In Transit",
            StatusBadgeText = "🚑 In Transit (ALS)",
            StatusForeground = "#0369A1",
            StatusBackground = "#E0F2FE",
            StatusBorder = "#BAE6FD",
            Priority = "Urgent",
            Reason = "Polytrauma / Emergency Neurosurgery"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801006",
            AxioId = "AXIO-88205",
            PatientName = "PATERSON, ARTHUR",
            AgeGender = "74 yrs / Male",
            DOB = "06/03/52",
            TransferType = "Facility Transfer",
            SendingFacility = "City Care - Ortho 5E",
            ReceivingFacility = "St. Jude Inpatient Rehab",
            RequestDate = "25-Aug-2026 02:15 PM",
            TransferDate = "26-Aug-2026 01:00 PM",
            AttendingPhysician = "Dr. J. Henderson, MD",
            Status = "Awaiting Bed",
            StatusBadgeText = "⏳ Awaiting Bed",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Priority = "Routine",
            Reason = "Post-TKR Intensive Physical Therapy"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801008",
            AxioId = "AXIO-88207",
            PatientName = "KAPOOR, ANANYA",
            AgeGender = "29 yrs / Female",
            DOB = "09/14/96",
            TransferType = "Specialist Referral",
            SendingFacility = "Axio Pulmonary Clinic",
            ReceivingFacility = "Chicago Lung Institute",
            RequestDate = "25-Aug-2026 11:00 AM",
            TransferDate = "29-Aug-2026 02:30 PM",
            AttendingPhysician = "Dr. A. Sharma, MD",
            Status = "Pending Review",
            StatusBadgeText = "⏳ Pending Review",
            StatusForeground = "#6366F1",
            StatusBackground = "#EEF2FF",
            StatusBorder = "#C7D2FE",
            Priority = "Routine",
            Reason = "Interstitial Lung Disease 2nd Opinion"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801010",
            AxioId = "AXIO-88209",
            PatientName = "O'CONNOR, LIAM",
            AgeGender = "51 yrs / Male",
            DOB = "03/28/75",
            TransferType = "Facility Transfer",
            SendingFacility = "City Care - Neuro 4B",
            ReceivingFacility = "Rush Comprehensive Stroke",
            RequestDate = "24-Aug-2026 04:30 PM",
            TransferDate = "24-Aug-2026 06:15 PM",
            AttendingPhysician = "Dr. L. Chen, MD",
            Status = "Completed",
            StatusBadgeText = "✓ Completed",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Priority = "Urgent",
            Reason = "Thrombectomy & Interventional Neuro"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801012",
            AxioId = "AXIO-88211",
            PatientName = "WILLIAMS, CLAIRE",
            AgeGender = "38 yrs / Female",
            DOB = "12/05/87",
            TransferType = "Specialist Referral",
            SendingFacility = "City Care - Oncology",
            ReceivingFacility = "MD Anderson Cancer Ctr",
            RequestDate = "24-Aug-2026 01:20 PM",
            TransferDate = "01-Sep-2026 09:00 AM",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Accepted / Scheduled",
            StatusBadgeText = "✓ Accepted",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Priority = "High",
            Reason = "Targeted Immunotherapy Trial"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801014",
            AxioId = "AXIO-88213",
            PatientName = "BAKER, GEORGE",
            AgeGender = "68 yrs / Male",
            DOB = "07/19/58",
            TransferType = "Facility Transfer",
            SendingFacility = "City Care - Surg 2B",
            ReceivingFacility = "Oakwood Skilled Nursing",
            RequestDate = "23-Aug-2026 10:00 AM",
            TransferDate = "24-Aug-2026 11:45 AM",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "Completed",
            StatusBadgeText = "✓ Completed",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Priority = "Routine",
            Reason = "Post-Surgical Wound Care & SNF Stepdown"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801016",
            AxioId = "AXIO-88215",
            PatientName = "MARTINEZ, SOFIA",
            AgeGender = "8 yrs / Female",
            DOB = "01/15/18",
            TransferType = "Tertiary Transfer",
            SendingFacility = "City Care - Peds ED",
            ReceivingFacility = "Lurie Children's Hospital",
            RequestDate = "23-Aug-2026 03:40 PM",
            TransferDate = "23-Aug-2026 05:00 PM",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Completed",
            StatusBadgeText = "✓ Completed",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Priority = "Urgent",
            Reason = "Pediatric Specialty PICU Escalation"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801018",
            AxioId = "AXIO-88217",
            PatientName = "DAVIS, HAROLD",
            AgeGender = "81 yrs / Male",
            DOB = "05/11/45",
            TransferType = "Facility Transfer",
            SendingFacility = "City Care - Med 3E",
            ReceivingFacility = "Mercy Health Hospice Care",
            RequestDate = "22-Aug-2026 02:00 PM",
            TransferDate = "23-Aug-2026 10:30 AM",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Completed",
            StatusBadgeText = "✓ Completed",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Priority = "Routine",
            Reason = "Inpatient Palliative & Hospice Transition"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801020",
            AxioId = "AXIO-88219",
            PatientName = "CHANG, MEI-LING",
            AgeGender = "44 yrs / Female",
            DOB = "10/02/81",
            TransferType = "Specialist Referral",
            SendingFacility = "Axio Outpatient Clinic",
            ReceivingFacility = "Loyola Nephrology Specialty",
            RequestDate = "22-Aug-2026 09:30 AM",
            TransferDate = "30-Aug-2026 01:15 PM",
            AttendingPhysician = "Dr. S. Miller, MD",
            Status = "Accepted / Scheduled",
            StatusBadgeText = "✓ Accepted",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Priority = "Routine",
            Reason = "Renal Transplant Workup & Clearance"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801022",
            AxioId = "AXIO-88221",
            PatientName = "ROSS, ETHAN",
            AgeGender = "33 yrs / Male",
            DOB = "08/29/93",
            TransferType = "Facility Transfer",
            SendingFacility = "City Care - Burn 1A",
            ReceivingFacility = "Loyola Regional Burn Center",
            RequestDate = "21-Aug-2026 11:15 AM",
            TransferDate = "21-Aug-2026 12:30 PM",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "Completed",
            StatusBadgeText = "✓ Completed",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Priority = "Urgent",
            Reason = "Severe Thermal Inhalation & Grafting"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801024",
            AxioId = "AXIO-88223",
            PatientName = "SCOTT, RACHEL",
            AgeGender = "57 yrs / Female",
            DOB = "03/14/69",
            TransferType = "Specialist Referral",
            SendingFacility = "Axio Rheumatology",
            ReceivingFacility = "UChicago Autoimmune Center",
            RequestDate = "20-Aug-2026 03:00 PM",
            TransferDate = "02-Sep-2026 11:00 AM",
            AttendingPhysician = "Dr. A. Sharma, MD",
            Status = "Accepted / Scheduled",
            StatusBadgeText = "✓ Accepted",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Priority = "Routine",
            Reason = "Refractory Lupus & Biologic Infusion"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801026",
            AxioId = "AXIO-88225",
            PatientName = "HERNANDEZ, MIGUEL",
            AgeGender = "50 yrs / Male",
            DOB = "12/22/75",
            TransferType = "Facility Transfer",
            SendingFacility = "City Care - Med 3W",
            ReceivingFacility = "Methodist Stepdown Unit",
            RequestDate = "19-Aug-2026 01:45 PM",
            TransferDate = "20-Aug-2026 09:15 AM",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Completed",
            StatusBadgeText = "✓ Completed",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Priority = "Routine",
            Reason = "Subacute Telemetry Monitoring"
        });

        _allPatients.Add(new ReferralTransferPatientItem
        {
            MRN = "64801028",
            AxioId = "AXIO-88227",
            PatientName = "FOSTER, SAMANTHA",
            AgeGender = "26 yrs / Female",
            DOB = "07/04/00",
            TransferType = "Specialist Referral",
            SendingFacility = "Axio Women's Health",
            ReceivingFacility = "Prentice Fetal Care Pavilion",
            RequestDate = "18-Aug-2026 10:30 AM",
            TransferDate = "28-Aug-2026 09:30 AM",
            AttendingPhysician = "Dr. L. Chen, MD",
            Status = "Accepted / Scheduled",
            StatusBadgeText = "✓ Accepted",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Priority = "High",
            Reason = "High-Risk Maternal-Fetal Medicine Consult"
        });
    }

    private void ApplyFilterAndRender()
    {
        var filtered = _allPatients.ToList();

        for (int i = 0; i < filtered.Count; i++)
        {
            filtered[i].RowBackground = (i % 2 == 0) ? "#FFFFFF" : "#F8FAFC";
        }

        if (ReferralItemsControl != null)
        {
            ReferralItemsControl.ItemsSource = filtered;
        }

        if (TotalPatientsBannerText != null)
        {
            TotalPatientsBannerText.Text = $"Total Patients: {filtered.Count}";
        }
    }

    private void OnPatientNamePointerPressed(object sender, PointerRoutedEventArgs e)
    {
        string patientName = "AXIO, MOCK";
        if (sender is TextBlock tb && !string.IsNullOrWhiteSpace(tb.Text))
        {
            patientName = tb.Text.Trim();
        }
        else if (sender is FrameworkElement fe && fe.Tag is string tag && !string.IsNullOrWhiteSpace(tag))
        {
            patientName = tag.Trim();
        }

        PatientSelected?.Invoke(this, new PatientSelectedEventArgs { PatientName = patientName });
    }

    private void OnRowPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b)
        {
            b.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 229, 241, 251)); // #E5F1FB Hover
        }
    }

    private void OnRowPointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b && b.DataContext is ReferralTransferPatientItem item)
        {
            b.Background = new SolidColorBrush(ParseColor(item.RowBackground));
        }
    }

    private static Windows.UI.Color ParseColor(string hex)
    {
        hex = hex.Replace("#", "");
        if (hex.Length == 6)
        {
            byte r = Convert.ToByte(hex.Substring(0, 2), 16);
            byte g = Convert.ToByte(hex.Substring(2, 2), 16);
            byte b = Convert.ToByte(hex.Substring(4, 2), 16);
            return ColorHelper.FromArgb(255, r, g, b);
        }
        return Colors.White;
    }
}
