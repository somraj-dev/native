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

public sealed partial class DischargeListView : UserControl
{
    public event EventHandler<PatientSelectedEventArgs>? PatientSelected;

    private readonly List<DischargePatientItem> _allDischargePatients = new();

    public DischargeListView()
    {
        this.InitializeComponent();
        LoadSampleDischargePatients();
        ApplyFilterAndRender();
    }

    private void LoadSampleDischargePatients()
    {
        _allDischargePatients.Clear();

        // -------------------------------------------------------------
        // PENDING DISCHARGE PATIENTS (Action / Clearance Required)
        // -------------------------------------------------------------
        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "1000245689",
            AxioId = "AXIO-MOCK-ID",
            PatientName = "AXIO, MOCK",
            AgeGender = "30 yrs / Male",
            DOB = "08/08/96",
            AdmittedOn = "22-Aug-2026 09:15 AM",
            DischargeDateTime = "Est: Today 03:00 PM",
            Department = "General Medicine",
            LocationUnit = "General Med - 3W Bed 04",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Med Rec)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Draft - Needs Med Rec",
            SummaryStatusForeground = "#B45309",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Awaiting final discharge med reconciliation by pharmacy"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64802090",
            AxioId = "AXIO-88210",
            PatientName = "TEST, NEWMERGE ONE",
            AgeGender = "75 yrs / Female",
            DOB = "01/01/51",
            AdmittedOn = "19-Aug-2026 02:40 PM",
            DischargeDateTime = "Est: Today 04:30 PM",
            Department = "Cardiology",
            LocationUnit = "Cardiology - 4B Bed 12",
            AttendingPhysician = "Dr. S. Miller, MD",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Attending Sign)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home Health & PT",
            DischargeSummaryStatus = "Draft - Awaiting Sign",
            SummaryStatusForeground = "#B45309",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Summary drafted, pending Dr. Miller co-signature"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64802647",
            AxioId = "AXIO-91402",
            PatientName = "TESTRODNEY, INPATIENT",
            AgeGender = "48 yrs / Male",
            DOB = "05/25/78",
            AdmittedOn = "21-Aug-2026 10:00 AM",
            DischargeDateTime = "Est: Today 05:00 PM",
            Department = "General Surgery",
            LocationUnit = "Surgery - 2A Bed 08",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Transport)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home with Family",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Family transport arriving at 04:45 PM"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801102",
            AxioId = "AXIO-44321",
            PatientName = "ADAMS, ELEANOR",
            AgeGender = "47 yrs / Female",
            DOB = "11/14/79",
            AdmittedOn = "23-Aug-2026 08:20 AM",
            DischargeDateTime = "Est: Tomorrow 10:00 AM",
            Department = "Cardiology",
            LocationUnit = "Cardiology - 4A Bed 02",
            AttendingPhysician = "Dr. S. Miller, MD",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Echo Review)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home with Outpatient Follow-up",
            DischargeSummaryStatus = "Pending Final Orders",
            SummaryStatusForeground = "#B45309",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Post-op echocardiogram report review required"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801906",
            AxioId = "AXIO-77519",
            PatientName = "MEDTEST, JR",
            AgeGender = "51 yrs / Male",
            DOB = "09/24/75",
            AdmittedOn = "20-Aug-2026 11:30 AM",
            DischargeDateTime = "Est: Today 06:00 PM",
            Department = "Orthopedics",
            LocationUnit = "Orthopedics - 5E Bed 15",
            AttendingPhysician = "Dr. J. Henderson, MD",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (PT Clearance)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Inpatient Rehab (St. Jude)",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Post-op ambulatory weight-bearing assessment pending"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801103",
            AxioId = "AXIO-66321",
            PatientName = "BAKER, SAMUEL",
            AgeGender = "43 yrs / Male",
            DOB = "04/18/83",
            AdmittedOn = "24-Aug-2026 01:10 PM",
            DischargeDateTime = "Est: Today 03:30 PM",
            Department = "General Medicine",
            LocationUnit = "General Med - 3E Bed 11",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Rx Delivery)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Bedside pharmacy delivery in progress"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801105",
            AxioId = "AXIO-55198",
            PatientName = "DAVIS, CLAIRE",
            AgeGender = "34 yrs / Female",
            DOB = "06/30/92",
            AdmittedOn = "23-Aug-2026 07:45 AM",
            DischargeDateTime = "Est: Today 04:00 PM",
            Department = "General Surgery",
            LocationUnit = "Surgery - 2B Bed 06",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Dressing Change)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home with Home Health",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Nurse wound care education in progress"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801107",
            AxioId = "AXIO-33821",
            PatientName = "FOSTER, GRACE",
            AgeGender = "31 yrs / Female",
            DOB = "08/20/95",
            AdmittedOn = "24-Aug-2026 09:00 AM",
            DischargeDateTime = "Est: Tomorrow 11:00 AM",
            Department = "Pulmonology",
            LocationUnit = "Pulmonology - 3W Bed 09",
            AttendingPhysician = "Dr. A. Sharma, MD",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Home O2 Setup)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home with Oxygen Delivery",
            DischargeSummaryStatus = "Draft - Pending O2 Rx",
            SummaryStatusForeground = "#B45309",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "DME company confirming delivery window"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801109",
            AxioId = "AXIO-99214",
            PatientName = "HARRIS, EMMA",
            AgeGender = "36 yrs / Female",
            DOB = "07/15/90",
            AdmittedOn = "22-Aug-2026 03:20 PM",
            DischargeDateTime = "Est: Today 05:30 PM",
            Department = "Neurology",
            LocationUnit = "Neurology - 4E Bed 07",
            AttendingPhysician = "Dr. L. Chen, MD",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (SNF Bed)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "SNF Transfer (Evergreen)",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Evergreen SNF intake coordinator confirming room"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801111",
            AxioId = "AXIO-88123",
            PatientName = "KING, OLIVIA",
            AgeGender = "32 yrs / Female",
            DOB = "01/19/94",
            AdmittedOn = "25-Aug-2026 10:15 AM",
            DischargeDateTime = "Est: Today 02:30 PM",
            Department = "General Medicine",
            LocationUnit = "General Med - 3E Bed 03",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Orders Co-Sign)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Draft - Pending Review",
            SummaryStatusForeground = "#B45309",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Discharge lab values reviewed, co-sign needed"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801113",
            AxioId = "AXIO-77218",
            PatientName = "MILLER, SOPHIA",
            AgeGender = "39 yrs / Female",
            DOB = "11/12/86",
            AdmittedOn = "23-Aug-2026 06:40 PM",
            DischargeDateTime = "Est: Today 04:15 PM",
            Department = "Orthopedics",
            LocationUnit = "Orthopedics - 5W Bed 14",
            AttendingPhysician = "Dr. J. Henderson, MD",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Brace Fit)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home with Outpatient PT",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Orthotist knee immobilizer fitting scheduled 03:30 PM"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801114",
            AxioId = "AXIO-66190",
            PatientName = "NELSON, ETHAN",
            AgeGender = "33 yrs / Male",
            DOB = "02/28/93",
            AdmittedOn = "24-Aug-2026 12:00 PM",
            DischargeDateTime = "Est: Today 03:45 PM",
            Department = "General Surgery",
            LocationUnit = "Surgery - 2W Bed 10",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Diet Tolerated)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Tolerating regular diet, IV fluids discontinued"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64802042",
            AxioId = "AXIO-11928",
            PatientName = "PHARMDRC, EIGHTMONTH",
            AgeGender = "9 yrs / Male",
            DOB = "09/22/16",
            AdmittedOn = "23-Aug-2026 08:30 AM",
            DischargeDateTime = "Est: Today 02:00 PM",
            Department = "General Medicine",
            LocationUnit = "Pediatrics - 2N Bed 01",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Parent Edu)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home with Parents",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Pediatric asthma action plan review with mother"
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801201",
            AxioId = "AXIO-33291",
            PatientName = "UCTEST, CPABBLINGCOMB",
            AgeGender = "17 yrs / Female",
            DOB = "03/14/09",
            AdmittedOn = "24-Aug-2026 02:15 PM",
            DischargeDateTime = "Est: Today 04:30 PM",
            Department = "General Surgery",
            LocationUnit = "Surgery - 2N Bed 05",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "Pending Discharge",
            StatusBadgeText = "Pending (Post-Op Void)",
            StatusForeground = "#B45309",
            StatusBackground = "#FEF3C7",
            StatusBorder = "#FDE68A",
            Disposition = "Home with Family",
            DischargeSummaryStatus = "Draft - Awaiting Sign",
            SummaryStatusForeground = "#B45309",
            RowBackground = "#FFFDF5",
            IsPending = true,
            PendingActionNote = "Spontaneous void achieved, discharge vitals logged"
        });

        // -------------------------------------------------------------
        // COMPLETED DISCHARGE PATIENTS
        // -------------------------------------------------------------
        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801104",
            AxioId = "AXIO-22109",
            PatientName = "CLARK, HENRY",
            AgeGender = "58 yrs / Male",
            DOB = "09/05/68",
            AdmittedOn = "18-Aug-2026 09:00 AM",
            DischargeDateTime = "26-Aug-2026 11:30 AM",
            Department = "Cardiology",
            LocationUnit = "Cardiology - 4B",
            AttendingPhysician = "Dr. S. Miller, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFFFF",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801106",
            AxioId = "AXIO-33104",
            PatientName = "EVANS, LUCAS",
            AgeGender = "38 yrs / Male",
            DOB = "12/01/87",
            AdmittedOn = "20-Aug-2026 01:45 PM",
            DischargeDateTime = "26-Aug-2026 10:15 AM",
            Department = "General Surgery",
            LocationUnit = "Surgery - 2A",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#F8FAFC",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801108",
            AxioId = "AXIO-44109",
            PatientName = "GARCIA, LIAM",
            AgeGender = "45 yrs / Male",
            DOB = "03/12/81",
            AdmittedOn = "17-Aug-2026 08:15 AM",
            DischargeDateTime = "26-Aug-2026 09:45 AM",
            Department = "Orthopedics",
            LocationUnit = "Orthopedics - 5E",
            AttendingPhysician = "Dr. J. Henderson, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Rehab)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Inpatient Rehab (St. Jude)",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFFFF",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801110",
            AxioId = "AXIO-55102",
            PatientName = "JACKSON, NOAH",
            AgeGender = "37 yrs / Male",
            DOB = "10/25/89",
            AdmittedOn = "21-Aug-2026 11:20 AM",
            DischargeDateTime = "26-Aug-2026 08:30 AM",
            Department = "General Medicine",
            LocationUnit = "General Med - 3W",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#F8FAFC",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801112",
            AxioId = "AXIO-66108",
            PatientName = "LOPEZ, ALEXANDER",
            AgeGender = "49 yrs / Male",
            DOB = "05/08/77",
            AdmittedOn = "19-Aug-2026 03:00 PM",
            DischargeDateTime = "25-Aug-2026 04:15 PM",
            Department = "Cardiology",
            LocationUnit = "Cardiology - 4A",
            AttendingPhysician = "Dr. S. Miller, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Cardiac Rehab",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFFFF",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801115",
            AxioId = "AXIO-77105",
            PatientName = "PEREZ, ISABELLA",
            AgeGender = "41 yrs / Female",
            DOB = "09/17/85",
            AdmittedOn = "22-Aug-2026 10:45 AM",
            DischargeDateTime = "25-Aug-2026 03:00 PM",
            Department = "General Surgery",
            LocationUnit = "Surgery - 2B",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#F8FAFC",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64802043",
            AxioId = "AXIO-88109",
            PatientName = "PHARMDRC, EIGHTYEAR",
            AgeGender = "17 yrs / Male",
            DOB = "05/22/09",
            AdmittedOn = "20-Aug-2026 09:30 AM",
            DischargeDateTime = "25-Aug-2026 01:20 PM",
            Department = "General Medicine",
            LocationUnit = "General Med - 3E",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFFFF",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64802044",
            AxioId = "AXIO-99104",
            PatientName = "PHARMDRC, EIGHTYEARCP",
            AgeGender = "17 yrs / Female",
            DOB = "05/22/09",
            AdmittedOn = "21-Aug-2026 02:00 PM",
            DischargeDateTime = "25-Aug-2026 11:45 AM",
            Department = "Pulmonology",
            LocationUnit = "Pulmonology - 3W",
            AttendingPhysician = "Dr. A. Sharma, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Outpatient Follow-up",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#F8FAFC",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801227",
            AxioId = "AXIO-11227",
            PatientName = "UCTEST, CPADEFECTTVWO",
            AgeGender = "35 yrs / Female",
            DOB = "10/14/91",
            AdmittedOn = "19-Aug-2026 04:00 PM",
            DischargeDateTime = "24-Aug-2026 05:00 PM",
            Department = "Neurology",
            LocationUnit = "Neurology - 4E",
            AttendingPhysician = "Dr. L. Chen, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFFFF",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64802066",
            AxioId = "AXIO-22266",
            PatientName = "ZZZTEST, DRADADMISSIONTWO",
            AgeGender = "36 yrs / Male",
            DOB = "11/11/90",
            AdmittedOn = "18-Aug-2026 01:15 PM",
            DischargeDateTime = "24-Aug-2026 02:30 PM",
            Department = "Cardiology",
            LocationUnit = "Cardiology - 4B",
            AttendingPhysician = "Dr. S. Miller, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#F8FAFC",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64802086",
            AxioId = "AXIO-33286",
            PatientName = "AWESOMEDUDEONE, MEME",
            AgeGender = "74 yrs / Male",
            DOB = "12/23/52",
            AdmittedOn = "15-Aug-2026 10:00 AM",
            DischargeDateTime = "24-Aug-2026 11:00 AM",
            Department = "Orthopedics",
            LocationUnit = "Orthopedics - 5E",
            AttendingPhysician = "Dr. J. Henderson, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (SNF)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "SNF Transfer (Evergreen)",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFFFF",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64800472",
            AxioId = "AXIO-44472",
            PatientName = "QUALITYCONNECT, AMY",
            AgeGender = "46 yrs / Female",
            DOB = "02/10/80",
            AdmittedOn = "16-Aug-2026 08:30 AM",
            DischargeDateTime = "23-Aug-2026 03:45 PM",
            Department = "General Surgery",
            LocationUnit = "Surgery - 2A",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#F8FAFC",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64801954",
            AxioId = "AXIO-55954",
            PatientName = "NURSING, RENAL",
            AgeGender = "74 yrs / Female",
            DOB = "07/30/52",
            AdmittedOn = "14-Aug-2026 11:00 AM",
            DischargeDateTime = "23-Aug-2026 01:15 PM",
            Department = "General Medicine",
            LocationUnit = "General Med - 3W",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Dialysis)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Outpatient Dialysis",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFFFF",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64802079",
            AxioId = "AXIO-66079",
            PatientName = "PHARMDRC, THIRTEEN",
            AgeGender = "22 yrs / Female",
            DOB = "05/21/04",
            AdmittedOn = "19-Aug-2026 09:15 AM",
            DischargeDateTime = "22-Aug-2026 04:30 PM",
            Department = "General Medicine",
            LocationUnit = "General Med - 3E",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#F8FAFC",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64803101",
            AxioId = "AXIO-77301",
            PatientName = "ROBERTS, WILLIAM",
            AgeGender = "62 yrs / Male",
            DOB = "04/03/64",
            AdmittedOn = "13-Aug-2026 08:00 AM",
            DischargeDateTime = "22-Aug-2026 02:00 PM",
            Department = "Cardiology",
            LocationUnit = "Cardiology - 4B",
            AttendingPhysician = "Dr. S. Miller, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Home Health",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFFFF",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64803102",
            AxioId = "AXIO-77302",
            PatientName = "SANCHEZ, MARIA",
            AgeGender = "55 yrs / Female",
            DOB = "08/19/71",
            AdmittedOn = "15-Aug-2026 02:30 PM",
            DischargeDateTime = "21-Aug-2026 03:15 PM",
            Department = "General Surgery",
            LocationUnit = "Surgery - 2A",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#F8FAFC",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64803103",
            AxioId = "AXIO-77303",
            PatientName = "TAYLOR, ROBERT",
            AgeGender = "67 yrs / Male",
            DOB = "01/30/59",
            AdmittedOn = "12-Aug-2026 10:00 AM",
            DischargeDateTime = "21-Aug-2026 11:30 AM",
            Department = "Orthopedics",
            LocationUnit = "Orthopedics - 5E",
            AttendingPhysician = "Dr. J. Henderson, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Rehab)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Inpatient Rehab (St. Jude)",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFFFF",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64803104",
            AxioId = "AXIO-77304",
            PatientName = "WALKER, JESSICA",
            AgeGender = "29 yrs / Female",
            DOB = "06/14/97",
            AdmittedOn = "16-Aug-2026 09:45 AM",
            DischargeDateTime = "20-Aug-2026 04:00 PM",
            Department = "General Medicine",
            LocationUnit = "General Med - 3W",
            AttendingPhysician = "Dr. TestUser, General (MD)",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#F8FAFC",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64803105",
            AxioId = "AXIO-77305",
            PatientName = "YOUNG, DANIEL",
            AgeGender = "53 yrs / Male",
            DOB = "03/25/73",
            AdmittedOn = "14-Aug-2026 01:00 PM",
            DischargeDateTime = "20-Aug-2026 01:30 PM",
            Department = "Pulmonology",
            LocationUnit = "Pulmonology - 3W",
            AttendingPhysician = "Dr. A. Sharma, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Outpatient Follow-up",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFFFF",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64803106",
            AxioId = "AXIO-77306",
            PatientName = "WHITE, EMILY",
            AgeGender = "42 yrs / Female",
            DOB = "10/09/84",
            AdmittedOn = "15-Aug-2026 03:15 PM",
            DischargeDateTime = "19-Aug-2026 02:45 PM",
            Department = "Neurology",
            LocationUnit = "Neurology - 4A",
            AttendingPhysician = "Dr. L. Chen, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#F8FAFC",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64803107",
            AxioId = "AXIO-77307",
            PatientName = "HARRIS, BENJAMIN",
            AgeGender = "61 yrs / Male",
            DOB = "05/17/65",
            AdmittedOn = "11-Aug-2026 11:30 AM",
            DischargeDateTime = "19-Aug-2026 10:15 AM",
            Department = "Cardiology",
            LocationUnit = "Cardiology - 4B",
            AttendingPhysician = "Dr. S. Miller, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (SNF)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "SNF Transfer (Oakwood)",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#FFFFFF",
            IsPending = false
        });

        _allDischargePatients.Add(new DischargePatientItem
        {
            MRN = "64803108",
            AxioId = "AXIO-77308",
            PatientName = "MARTIN, CHLOE",
            AgeGender = "27 yrs / Female",
            DOB = "12/04/98",
            AdmittedOn = "13-Aug-2026 09:00 AM",
            DischargeDateTime = "18-Aug-2026 03:30 PM",
            Department = "General Surgery",
            LocationUnit = "Surgery - 2B",
            AttendingPhysician = "Dr. K. Reynolds, MD",
            Status = "Discharged",
            StatusBadgeText = "✓ Discharged (Home)",
            StatusForeground = "#15803D",
            StatusBackground = "#DCFCE7",
            StatusBorder = "#BBF7D0",
            Disposition = "Home with Self-Care",
            DischargeSummaryStatus = "Signed & Complete",
            SummaryStatusForeground = "#15803D",
            RowBackground = "#F8FAFC",
            IsPending = false
        });
    }

    private void ApplyFilterAndRender()
    {
        var filtered = _allDischargePatients.ToList();

        // Alternating row background for clean EHR view
        for (int i = 0; i < filtered.Count; i++)
        {
            if (filtered[i].IsPending)
            {
                filtered[i].RowBackground = (i % 2 == 0) ? "#FFFDF5" : "#FFF8E7";
            }
            else
            {
                filtered[i].RowBackground = (i % 2 == 0) ? "#FFFFFF" : "#F8FAFC";
            }
        }

        if (DischargeItemsControl != null)
        {
            DischargeItemsControl.ItemsSource = filtered;
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
        if (sender is Border b && b.DataContext is DischargePatientItem item)
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
