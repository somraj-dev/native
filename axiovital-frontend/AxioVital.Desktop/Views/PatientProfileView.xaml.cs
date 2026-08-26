using System;
using System.Collections.Generic;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace AxioVital.Desktop.Views;

public class PatientClinicalProfile
{
    public string Name { get; set; } = string.Empty;
    public string Mrn { get; set; } = string.Empty;
    public string Dob { get; set; } = string.Empty;
    public string Age { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string BloodType { get; set; } = "O+";
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Status { get; set; } = "Active Inpatient";
    public string AttendingMd { get; set; } = "Dr. Robert Axio, MD";
    public string Allergies { get; set; } = "Allergies: shellfish";
    public string DoseWt { get; set; } = "Dose Wt: 80.000 kg (04/25/2021)";
    public string HealtheLife { get; set; } = "HealtheLife: No";
    public string Clinic { get; set; } = "Clinic: 01XTQ";
    public string CodeStatus { get; set; } = "Code Status: Active";
    public string Location { get; set; } = "Loc: GLW-01";
    public string Nominee { get; set; } = "Nominee: Mr. Ajan singh";
    public string Fin { get; set; } = "Inpatient FIN: 00095526415 [Admit Dt: 10/6/2020 3:14:12 PM CDT]";

    // Form data
    public string ImplantLot { get; set; } = "";
    public string DermalMatrix { get; set; } = "No";
    public string OpDescription { get; set; } = "";
    public string Comments { get; set; } = "";
    public string OtherComments { get; set; } = "I am signing this report at the direction of administration, in the absence of Cerner Surgeon 1.";

    // Clinical Sections Data
    public List<(string Title, string Date, string Author, string Summary)> Notes { get; set; } = new();
    public List<(string Allergen, string Reaction, string Severity, string Type)> AllergyList { get; set; } = new();
    public List<(string Code, string Description, string Type, string OnsetDate, string Status)> Diagnoses { get; set; } = new();
    public List<(string MedName, string Dose, string Route, string Frequency, string Status)> Medications { get; set; } = new();
    public List<(string Category, string Details)> Histories { get; set; } = new();
    public (string Provider, string PolicyNo, string GroupNo, string Guarantor, string PlanType) Insurance { get; set; }
}

public sealed partial class PatientProfileView : UserControl
{
    private PatientClinicalProfile? _currentPatient;
    private Border? _activeNavBorder;

    public PatientProfileView()
    {
        this.InitializeComponent();
        SetDefaultPatient();
        HighlightNav(NavDocBorder, NavDocBar, NavDocText);
    }

    public void SetDefaultPatient()
    {
        var defaultPatient = new PatientClinicalProfile
        {
            Name = "JOHN DOE",
            Mrn = "AVX-SL-A1H4XE",
            Dob = "03/22/1984",
            Age = "39",
            Gender = "Female",
            BloodType = "A+",
            Phone = "(416) 555-0192",
            Address = "742 Evergreen Terrace, Toronto, ON",
            Status = "Active Inpatient",
            AttendingMd = "Dr. Sarah Miller, MD",
            Allergies = "Allergies: shellfish",
            DoseWt = "Dose Wt: 80.000 kg (04/25/2021)",
            HealtheLife = "HealtheLife: No",
            Clinic = "Clinic: 01XTQ",
            CodeStatus = "Code Status: Active",
            Location = "Loc: GLW-01",
            Nominee = "Nominee: Mr. Ajan singh",
            Fin = "Inpatient FIN: 00095526415 [Admit Dt: 10/6/2020 3:14:12 PM CDT]",
            ImplantLot = "LOT-99281-CERNER-A",
            DermalMatrix = "No",
            OpDescription = "Routine laparoscopic bilateral exploration and tissue repair",
            Comments = "Patient tolerated general anesthesia without acute complications.",
            OtherComments = "I am signing this report at the direction of administration, in the absence of Cerner Surgeon 1.",
            Notes = new List<(string, string, string, string)>
            {
                ("Post-Operative Progress Note", "Today 08:30 AM", "Dr. Sarah Miller, MD", "Patient is alert, oriented x4. Surgical site clean, dry and intact. Pain well controlled on oral analgesics. Tolerating regular diet."),
                ("Pre-Anesthetic Evaluation", "Yesterday 04:15 PM", "Dr. Alan Reed, MD (Anesthesiology)", "ASA Class II. Airway Mallampati Class 1. No contraindications to general endotracheal anesthesia."),
                ("Nursing Admission Assessment", "10/06/2025 03:30 PM", "Jane Doe, RN", "Vitals stable upon unit arrival. Fall risk assessment score: Low. Allergy bracelet applied.")
            },
            AllergyList = new List<(string, string, string, string)>
            {
                ("Shellfish", "Anaphylaxis / Hives", "Severe", "Food"),
                ("Penicillin", "Maculopapular Rash", "Moderate", "Drug"),
                ("Latex", "Contact Dermatitis", "Mild", "Environmental")
            },
            Diagnoses = new List<(string, string, string, string, string)>
            {
                ("K40.90", "Unilateral inguinal hernia, without obstruction or gangrene", "Principal", "10/06/2025", "Active"),
                ("I10", "Essential (primary) hypertension", "Chronic", "04/12/2018", "Active"),
                ("E78.5", "Hyperlipidemia, unspecified", "Secondary", "09/19/2020", "Active")
            },
            Medications = new List<(string, string, string, string, string)>
            {
                ("Cefazolin IV", "2 g", "Intravenous", "Q8H x 3 doses", "Active"),
                ("Acetaminophen PO", "650 mg", "Oral", "Q6H PRN Pain", "Active"),
                ("Lisinopril PO", "10 mg", "Oral", "Daily (Morning)", "Active"),
                ("Enoxaparin SubQ", "40 mg", "Subcutaneous", "Daily (DVT Prophylaxis)", "Active")
            },
            Histories = new List<(string, string)>
            {
                ("Past Medical History", "Hypertension (diagnosed 2018), Hyperlipidemia."),
                ("Past Surgical History", "Appendectomy (2004), Right knee arthroscopy (2015)."),
                ("Family History", "Father: Myocardial infarction at age 62. Mother: Type 2 Diabetes."),
                ("Social History", "Non-smoker, occasional social alcohol, denies illicit substances.")
            },
            Insurance = ("Sun Life Financial Healthcare", "POL-99281-01", "GRP-7741", "Self (John Doe)", "Comprehensive Major Medical")
        };

        SetPatient(defaultPatient);
    }

    public void SetPatient(PatientClinicalProfile patient)
    {
        _currentPatient = patient;

        // 1. Form Browser Fields
        if (FormImplantLotBox != null) FormImplantLotBox.Text = patient.ImplantLot;
        if (FormCommentsBox != null) FormCommentsBox.Text = patient.Comments;
        if (FormOtherCommentsBox != null) FormOtherCommentsBox.Text = patient.OtherComments;

        if (FormDermalYes != null) FormDermalYes.IsChecked = (patient.DermalMatrix == "Yes");
        if (FormDermalNo != null) FormDermalNo.IsChecked = (patient.DermalMatrix == "No");
        if (FormDermalUnknown != null) FormDermalUnknown.IsChecked = (patient.DermalMatrix == "Unknown");

        if (FormOpDescriptionCombo != null)
        {
            FormOpDescriptionCombo.Items.Clear();
            FormOpDescriptionCombo.Items.Add(patient.OpDescription);
            FormOpDescriptionCombo.Items.Add("Diagnostic Arthroscopy with debridement");
            FormOpDescriptionCombo.Items.Add("Robotic assisted laparoscopic resection");
            FormOpDescriptionCombo.Items.Add("Endoscopic mucosal evaluation & biopsy");
            FormOpDescriptionCombo.SelectedIndex = 0;
        }

        // Render Clinical Panels
        RenderDocumentation(patient.Notes);
        RenderAllergies(patient.AllergyList);
        RenderDiagnoses(patient.Diagnoses);
        RenderMedications(patient.Medications);
        RenderInsurance(patient.Insurance);
        RenderProviderView();
        RenderResultsReview();
        RenderOrders();
        RenderOutsideRecords();
        RenderClinicalMedia();
        RenderInteractiveView();
        RenderMarSummary();
    }

    private void RenderProviderView()
    {
        if (ProviderNotesContainer == null) return;
        ProviderNotesContainer.Children.Clear();

        var items = new (string Title, string Date, string Author, string Content)[]
        {
            ("Surgical Discharge Summary Plan", "Today 09:15 AM", "Dr. Sarah Miller, MD", "Patient is recovering well post laparoscopic repair. Scheduled for outpatient suture removal in 10 days. Resume activity as tolerated."),
            ("Attending Daily Rounds", "Today 07:45 AM", "Dr. Robert Axio, MD", "Wound site clean without erythema. Pain score 2/10. Discontinue IV antibiotics post 3rd dose. Plan for discharge tomorrow morning."),
            ("Clinical Pharmacy Consultation", "Yesterday 02:00 PM", "Dr. Emily Chen, PharmD", "Renal dosage adjustment reviewed. Lisinopril 10mg verified. No drug-drug interactions detected.")
        };

        foreach (var itm in items)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 248, 250, 252)),
                BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(14, 10, 14, 10)
            };
            var sp = new StackPanel { Spacing = 4 };
            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            var titleTb = new TextBlock { Text = itm.Title, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 23, 42)), FontSize = 12 };
            var dateTb = new TextBlock { Text = itm.Date, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)), FontSize = 10 };
            Grid.SetColumn(titleTb, 0);
            Grid.SetColumn(dateTb, 1);
            headerGrid.Children.Add(titleTb);
            headerGrid.Children.Add(dateTb);

            var authorTb = new TextBlock { Text = itm.Author, FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 2, 132, 199)), FontWeight = Microsoft.UI.Text.FontWeights.SemiBold };
            var bodyTb = new TextBlock { Text = itm.Content, FontSize = 11, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 2, 0, 0) };

            sp.Children.Add(headerGrid);
            sp.Children.Add(authorTb);
            sp.Children.Add(bodyTb);
            card.Child = sp;
            ProviderNotesContainer.Children.Add(card);
        }
    }

    private void RenderResultsReview()
    {
        if (ResultsListContainer == null) return;
        ResultsListContainer.Children.Clear();

        var tests = new (string Category, (string Name, string Value, string Unit, string RefRange, string Flag)[] Panels)[]
        {
            ("Complete Blood Count (CBC) • Today 06:30 AM", new [] {
                ("Hemoglobin", "14.2", "g/dL", "13.5 - 17.5", "Normal"),
                ("White Blood Cell (WBC)", "8.4", "x10^3/uL", "4.5 - 11.0", "Normal"),
                ("Platelets", "248", "x10^3/uL", "150 - 450", "Normal"),
                ("Hematocrit", "42.1", "%", "41.0 - 50.0", "Normal")
            }),
            ("Basic Metabolic Panel (BMP) • Today 06:30 AM", new [] {
                ("Sodium", "139", "mmol/L", "135 - 145", "Normal"),
                ("Potassium", "4.1", "mmol/L", "3.5 - 5.0", "Normal"),
                ("Chloride", "102", "mmol/L", "96 - 106", "Normal"),
                ("Creatinine", "0.92", "mg/dL", "0.70 - 1.30", "Normal"),
                ("Glucose (Fasting)", "108", "mg/dL", "70 - 99", "High")
            })
        };

        foreach (var group in tests)
        {
            var header = new TextBlock { Text = group.Category, FontSize = 12, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59)), Margin = new Thickness(0, 4, 0, 2) };
            ResultsListContainer.Children.Add(header);

            var border = new Border { Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(4) };
            var stack = new StackPanel();

            foreach (var row in group.Panels)
            {
                var rowGrid = new Grid { Height = 28, Padding = new Thickness(12, 0, 12, 0), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 241, 245, 249)), BorderThickness = new Thickness(0, 0, 0, 1) };
                rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
                rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
                rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
                rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
                rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

                var nameTb = new TextBlock { Text = row.Name, FontSize = 11, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 23, 42)), VerticalAlignment = VerticalAlignment.Center };
                var valTb = new TextBlock { Text = row.Value, FontSize = 11, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, Foreground = row.Flag == "High" ? new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 220, 38, 38)) : new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 23, 42)), VerticalAlignment = VerticalAlignment.Center };
                var unitTb = new TextBlock { Text = row.Unit, FontSize = 10, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)), VerticalAlignment = VerticalAlignment.Center };
                var refTb = new TextBlock { Text = row.RefRange, FontSize = 10, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)), VerticalAlignment = VerticalAlignment.Center };
                var flagTb = new TextBlock { Text = row.Flag, FontSize = 10, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = row.Flag == "High" ? new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 220, 38, 38)) : new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 16, 185, 129)), VerticalAlignment = VerticalAlignment.Center };

                Grid.SetColumn(nameTb, 0);
                Grid.SetColumn(valTb, 1);
                Grid.SetColumn(unitTb, 2);
                Grid.SetColumn(refTb, 3);
                Grid.SetColumn(flagTb, 4);

                rowGrid.Children.Add(nameTb);
                rowGrid.Children.Add(valTb);
                rowGrid.Children.Add(unitTb);
                rowGrid.Children.Add(refTb);
                rowGrid.Children.Add(flagTb);
                stack.Children.Add(rowGrid);
            }
            border.Child = stack;
            ResultsListContainer.Children.Add(border);
        }
    }

    private void RenderOrders()
    {
        if (OrdersListContainer == null) return;
        OrdersListContainer.Children.Clear();

        var orders = new (string Id, string Type, string Description, string OrderingMd, string Status, string OrderedAt)[]
        {
            ("ORD-2025-8841", "Pharmacy", "Cefazolin IV 2g Q8H x 3 doses", "Dr. Sarah Miller, MD", "Active", "Today 07:00 AM"),
            ("ORD-2025-8842", "Nursing", "Vital signs every 4 hours, record I&O", "Dr. Robert Axio, MD", "Active", "Today 07:15 AM"),
            ("ORD-2025-8843", "Laboratory", "Repeat CBC & Basic Metabolic Panel in AM", "Dr. Sarah Miller, MD", "Pending", "Today 08:30 AM"),
            ("ORD-2025-8844", "Dietary", "Regular post-op diet as tolerated", "Dr. Sarah Miller, MD", "Active", "Today 08:00 AM")
        };

        foreach (var ord in orders)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 248, 250, 252)),
                BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(14, 8, 14, 8)
            };
            var g = new Grid();
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(110) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(160) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

            var idTb = new TextBlock { Text = ord.Id, FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)), VerticalAlignment = VerticalAlignment.Center };
            var typeTb = new TextBlock { Text = ord.Type, FontSize = 10.5, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 2, 132, 199)), VerticalAlignment = VerticalAlignment.Center };
            var descTb = new TextBlock { Text = ord.Description, FontSize = 11, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 23, 42)), VerticalAlignment = VerticalAlignment.Center };
            var mdTb = new TextBlock { Text = $"{ord.OrderingMd} ({ord.OrderedAt})", FontSize = 10, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 71, 85, 105)), VerticalAlignment = VerticalAlignment.Center };
            var statTb = new TextBlock { Text = ord.Status, FontSize = 10.5, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = ord.Status == "Active" ? new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 16, 185, 129)) : new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 234, 179, 8)), VerticalAlignment = VerticalAlignment.Center };

            Grid.SetColumn(idTb, 0);
            Grid.SetColumn(typeTb, 1);
            Grid.SetColumn(descTb, 2);
            Grid.SetColumn(mdTb, 3);
            Grid.SetColumn(statTb, 4);

            g.Children.Add(idTb);
            g.Children.Add(typeTb);
            g.Children.Add(descTb);
            g.Children.Add(mdTb);
            g.Children.Add(statTb);

            card.Child = g;
            OrdersListContainer.Children.Add(card);
        }
    }

    private void RenderOutsideRecords()
    {
        if (OutsideRecordsListContainer == null) return;
        OutsideRecordsListContainer.Children.Clear();

        var docs = new (string Source, string Type, string Date, string Summary)[]
        {
            ("University Health Network (HIE)", "Continuity of Care Document (CCD)", "09/14/2025", "Outpatient Cardiology Consultation. Normal ECG, LVEF 60%. Continues Lisinopril 10mg."),
            ("Mount Sinai Regional Clinic", "Operative Report", "05/18/2021", "Right knee arthroscopy with partial meniscectomy. Dr. P. Johnson, MD."),
            ("Toronto Western Diagnostic Imaging", "Radiology / Ultrasound Report", "08/10/2025", "Right groin ultrasound confirming reducible indirect inguinal hernia.")
        };

        foreach (var d in docs)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 248, 250, 252)),
                BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(14, 10, 14, 10)
            };
            var sp = new StackPanel { Spacing = 3 };
            sp.Children.Add(new TextBlock { Text = d.Source, FontSize = 12, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 2, 132, 199)) });
            sp.Children.Add(new TextBlock { Text = $"{d.Type} • Date: {d.Date}", FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)) });
            sp.Children.Add(new TextBlock { Text = d.Summary, FontSize = 11, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)), Margin = new Thickness(0, 2, 0, 0) });
            card.Child = sp;
            OutsideRecordsListContainer.Children.Add(card);
        }
    }

    private void RenderClinicalMedia()
    {
        if (ClinicalMediaContainer == null) return;
        ClinicalMediaContainer.Children.Clear();

        var mediaList = new (string Title, string Date, string Modality, string Description)[]
        {
            ("Intraoperative Laparoscopy Capture", "10/06/2025 11:20 AM", "Video Still / High-Res HD", "Bilateral inguinal ring inspection and synthetic mesh placement verification."),
            ("Surgical Incision Site Baseline", "10/06/2025 02:45 PM", "Clinical Photo", "Baseline photo of surgical portal closures. Skin intact, sterile dressings intact."),
            ("Pre-Op Chest X-Ray (PA & Lateral)", "10/05/2025 04:10 PM", "DICOM Radiography", "Clear lung fields, normal cardiothoracic ratio, no active consolidation.")
        };

        foreach (var m in mediaList)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 248, 250, 252)),
                BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(14, 10, 14, 10)
            };
            var sp = new StackPanel { Spacing = 3 };
            sp.Children.Add(new TextBlock { Text = m.Title, FontSize = 12, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 23, 42)) });
            sp.Children.Add(new TextBlock { Text = $"Modality: {m.Modality} • Captured: {m.Date}", FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)) });
            sp.Children.Add(new TextBlock { Text = m.Description, FontSize = 11, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)) });
            card.Child = sp;
            ClinicalMediaContainer.Children.Add(card);
        }
    }

    private void RenderInteractiveView()
    {
        if (IoListContainer == null) return;
        IoListContainer.Children.Clear();

        var flows = new (string Time, string IntakeOral, string IntakeIv, string OutputUrine, string OutputDrain, string NetBalance)[]
        {
            ("08:00 AM", "250 mL (Water)", "125 mL/hr Normal Saline", "350 mL", "15 mL (Serosanguinous)", "+10 mL"),
            ("04:00 AM", "100 mL (Ice chips)", "125 mL/hr Normal Saline", "280 mL", "20 mL", "-35 mL"),
            ("12:00 AM", "150 mL (Juice)", "125 mL/hr Normal Saline", "320 mL", "25 mL", "-20 mL"),
            ("08:00 PM (Yesterday)", "400 mL (Clear Broth)", "125 mL/hr Normal Saline", "450 mL", "30 mL", "+45 mL")
        };

        var border = new Border { Background = new SolidColorBrush(Colors.White), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(1), CornerRadius = new CornerRadius(4) };
        var stack = new StackPanel();

        // Header
        var hGrid = new Grid { Height = 26, Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 241, 245, 249)), Padding = new Thickness(10, 0, 10, 0) };
        hGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(140) });
        hGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
        hGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(180) });
        hGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
        hGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
        hGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });

        hGrid.Children.Add(new TextBlock { Text = "Time Interval", FontSize = 10, FontWeight = Microsoft.UI.Text.FontWeights.Bold });
        var t1 = new TextBlock { Text = "Oral Intake", FontSize = 10, FontWeight = Microsoft.UI.Text.FontWeights.Bold }; Grid.SetColumn(t1, 1); hGrid.Children.Add(t1);
        var t2 = new TextBlock { Text = "IV Infusion", FontSize = 10, FontWeight = Microsoft.UI.Text.FontWeights.Bold }; Grid.SetColumn(t2, 2); hGrid.Children.Add(t2);
        var t3 = new TextBlock { Text = "Urine Output", FontSize = 10, FontWeight = Microsoft.UI.Text.FontWeights.Bold }; Grid.SetColumn(t3, 3); hGrid.Children.Add(t3);
        var t4 = new TextBlock { Text = "Surgical Drains", FontSize = 10, FontWeight = Microsoft.UI.Text.FontWeights.Bold }; Grid.SetColumn(t4, 4); hGrid.Children.Add(t4);
        var t5 = new TextBlock { Text = "Net Balance", FontSize = 10, FontWeight = Microsoft.UI.Text.FontWeights.Bold }; Grid.SetColumn(t5, 5); hGrid.Children.Add(t5);

        stack.Children.Add(hGrid);

        foreach (var f in flows)
        {
            var rowGrid = new Grid { Height = 26, Padding = new Thickness(10, 0, 10, 0), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 241, 245, 249)), BorderThickness = new Thickness(0, 0, 0, 1) };
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(140) });
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(180) });
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });

            var c0 = new TextBlock { Text = f.Time, FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 23, 42)), VerticalAlignment = VerticalAlignment.Center };
            var c1 = new TextBlock { Text = f.IntakeOral, FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)), VerticalAlignment = VerticalAlignment.Center };
            var c2 = new TextBlock { Text = f.IntakeIv, FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)), VerticalAlignment = VerticalAlignment.Center };
            var c3 = new TextBlock { Text = f.OutputUrine, FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)), VerticalAlignment = VerticalAlignment.Center };
            var c4 = new TextBlock { Text = f.OutputDrain, FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)), VerticalAlignment = VerticalAlignment.Center };
            var c5 = new TextBlock { Text = f.NetBalance, FontSize = 10.5, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 16, 185, 129)), VerticalAlignment = VerticalAlignment.Center };

            Grid.SetColumn(c0, 0);
            Grid.SetColumn(c1, 1);
            Grid.SetColumn(c2, 2);
            Grid.SetColumn(c3, 3);
            Grid.SetColumn(c4, 4);
            Grid.SetColumn(c5, 5);

            rowGrid.Children.Add(c0);
            rowGrid.Children.Add(c1);
            rowGrid.Children.Add(c2);
            rowGrid.Children.Add(c3);
            rowGrid.Children.Add(c4);
            rowGrid.Children.Add(c5);
            stack.Children.Add(rowGrid);
        }
        border.Child = stack;
        IoListContainer.Children.Add(border);
    }

    private void RenderMarSummary()
    {
        if (MarSummaryContainer == null) return;
        MarSummaryContainer.Children.Clear();

        var mars = new (string Med, string Dose, string Route, string SchedTime, string GivenAt, string GivenBy, string BarcodeStatus)[]
        {
            ("Cefazolin IV", "2 g", "IV Piggyback", "08:00 AM", "08:05 AM", "Jane Doe, RN", "Verified (BCMA Scanned)"),
            ("Lisinopril PO", "10 mg", "Oral", "08:00 AM", "08:12 AM", "Jane Doe, RN", "Verified (BCMA Scanned)"),
            ("Enoxaparin SubQ", "40 mg", "Subcutaneous", "09:00 PM (Yesterday)", "09:02 PM", "Mark Taylor, RN", "Verified (BCMA Scanned)"),
            ("Acetaminophen PO", "650 mg", "Oral PRN", "PRN (06:30 AM)", "06:35 AM", "Jane Doe, RN", "Verified (BCMA Scanned)")
        };

        foreach (var m in mars)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 248, 250, 252)),
                BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(14, 8, 14, 8)
            };
            var g = new Grid();
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(140) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(110) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            g.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var m0 = new TextBlock { Text = m.Med, FontSize = 11, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 23, 42)), VerticalAlignment = VerticalAlignment.Center };
            var m1 = new TextBlock { Text = m.Dose, FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)), VerticalAlignment = VerticalAlignment.Center };
            var m2 = new TextBlock { Text = m.Route, FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)), VerticalAlignment = VerticalAlignment.Center };
            var m3 = new TextBlock { Text = $"Sched: {m.SchedTime}", FontSize = 10, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)), VerticalAlignment = VerticalAlignment.Center };
            var m4 = new TextBlock { Text = $"Given: {m.GivenAt} ({m.GivenBy})", FontSize = 10, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)), VerticalAlignment = VerticalAlignment.Center };
            var m5 = new TextBlock { Text = m.BarcodeStatus, FontSize = 10, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 16, 185, 129)), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Right };

            Grid.SetColumn(m0, 0);
            Grid.SetColumn(m1, 1);
            Grid.SetColumn(m2, 2);
            Grid.SetColumn(m3, 3);
            Grid.SetColumn(m4, 4);
            Grid.SetColumn(m5, 5);

            g.Children.Add(m0);
            g.Children.Add(m1);
            g.Children.Add(m2);
            g.Children.Add(m3);
            g.Children.Add(m4);
            g.Children.Add(m5);

            card.Child = g;
            MarSummaryContainer.Children.Add(card);
        }
    }

    private void RenderDocumentation(List<(string Title, string Date, string Author, string Summary)> notes)
    {
        if (DocumentationListContainer == null) return;
        DocumentationListContainer.Children.Clear();

        foreach (var note in notes)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 248, 250, 252)),
                BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(14, 10, 14, 10)
            };

            var stack = new StackPanel { Spacing = 4 };
            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            var titleTb = new TextBlock { Text = note.Title, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 23, 42)), FontSize = 12 };
            var dateTb = new TextBlock { Text = note.Date, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)), FontSize = 10 };
            Grid.SetColumn(titleTb, 0);
            Grid.SetColumn(dateTb, 1);
            headerGrid.Children.Add(titleTb);
            headerGrid.Children.Add(dateTb);

            var authorTb = new TextBlock { Text = note.Author, FontSize = 10.5, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 2, 132, 199)), FontWeight = Microsoft.UI.Text.FontWeights.SemiBold };
            var bodyTb = new TextBlock { Text = note.Summary, FontSize = 11, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 2, 0, 0) };

            stack.Children.Add(headerGrid);
            stack.Children.Add(authorTb);
            stack.Children.Add(bodyTb);
            card.Child = stack;

            DocumentationListContainer.Children.Add(card);
        }
    }

    private void RenderAllergies(List<(string Allergen, string Reaction, string Severity, string Type)> allergies)
    {
        if (AllergiesListContainer == null) return;
        AllergiesListContainer.Children.Clear();

        foreach (var a in allergies)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 254, 242, 242)),
                BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 254, 202, 202)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(12, 8, 12, 8)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(140) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

            var nameTb = new TextBlock { Text = a.Allergen, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 185, 28, 28)), FontSize = 11.5, VerticalAlignment = VerticalAlignment.Center };
            var reactTb = new TextBlock { Text = a.Reaction, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)), FontSize = 11, VerticalAlignment = VerticalAlignment.Center };
            var sevTb = new TextBlock { Text = a.Severity, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 220, 38, 38)), FontSize = 10.5, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, VerticalAlignment = VerticalAlignment.Center };
            var typeTb = new TextBlock { Text = a.Type, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)), FontSize = 10.5, VerticalAlignment = VerticalAlignment.Center };

            Grid.SetColumn(nameTb, 0);
            Grid.SetColumn(reactTb, 1);
            Grid.SetColumn(sevTb, 2);
            Grid.SetColumn(typeTb, 3);

            grid.Children.Add(nameTb);
            grid.Children.Add(reactTb);
            grid.Children.Add(sevTb);
            grid.Children.Add(typeTb);

            card.Child = grid;
            AllergiesListContainer.Children.Add(card);
        }
    }

    private void RenderDiagnoses(List<(string Code, string Description, string Type, string OnsetDate, string Status)> diagnoses)
    {
        if (DiagnosesListContainer == null) return;
        DiagnosesListContainer.Children.Clear();

        foreach (var d in diagnoses)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 248, 250, 252)),
                BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(12, 8, 12, 8)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });

            var codeTb = new TextBlock { Text = d.Code, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 2, 132, 199)), FontSize = 11, VerticalAlignment = VerticalAlignment.Center };
            var descTb = new TextBlock { Text = d.Description, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 23, 42)), FontSize = 11, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, VerticalAlignment = VerticalAlignment.Center };
            var typeTb = new TextBlock { Text = d.Type, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)), FontSize = 10.5, VerticalAlignment = VerticalAlignment.Center };
            var dateTb = new TextBlock { Text = d.OnsetDate, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)), FontSize = 10.5, VerticalAlignment = VerticalAlignment.Center };
            var statusTb = new TextBlock { Text = d.Status, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 16, 185, 129)), FontSize = 10.5, FontWeight = Microsoft.UI.Text.FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };

            Grid.SetColumn(codeTb, 0);
            Grid.SetColumn(descTb, 1);
            Grid.SetColumn(typeTb, 2);
            Grid.SetColumn(dateTb, 3);
            Grid.SetColumn(statusTb, 4);

            grid.Children.Add(codeTb);
            grid.Children.Add(descTb);
            grid.Children.Add(typeTb);
            grid.Children.Add(dateTb);
            grid.Children.Add(statusTb);

            card.Child = grid;
            DiagnosesListContainer.Children.Add(card);
        }
    }

    private void RenderMedications(List<(string MedName, string Dose, string Route, string Frequency, string Status)> meds)
    {
        if (MedicationsListContainer == null) return;
        MedicationsListContainer.Children.Clear();

        foreach (var m in meds)
        {
            var card = new Border
            {
                Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 248, 250, 252)),
                BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(12, 8, 12, 8)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(160) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(110) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(140) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });

            var nameTb = new TextBlock { Text = m.MedName, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 23, 42)), FontSize = 11, VerticalAlignment = VerticalAlignment.Center };
            var doseTb = new TextBlock { Text = m.Dose, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)), FontSize = 11, VerticalAlignment = VerticalAlignment.Center };
            var routeTb = new TextBlock { Text = m.Route, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139)), FontSize = 10.5, VerticalAlignment = VerticalAlignment.Center };
            var freqTb = new TextBlock { Text = m.Frequency, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 71, 85, 105)), FontSize = 10.5, VerticalAlignment = VerticalAlignment.Center };
            var statTb = new TextBlock { Text = m.Status, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 16, 185, 129)), FontSize = 10.5, FontWeight = Microsoft.UI.Text.FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };

            Grid.SetColumn(nameTb, 0);
            Grid.SetColumn(doseTb, 1);
            Grid.SetColumn(routeTb, 2);
            Grid.SetColumn(freqTb, 3);
            Grid.SetColumn(statTb, 4);

            grid.Children.Add(nameTb);
            grid.Children.Add(doseTb);
            grid.Children.Add(routeTb);
            grid.Children.Add(freqTb);
            grid.Children.Add(statTb);

            card.Child = grid;
            MedicationsListContainer.Children.Add(card);
        }
    }

    private void RenderInsurance((string Provider, string PolicyNo, string GroupNo, string Guarantor, string PlanType) ins)
    {
        if (InsuranceDetailsContainer == null) return;
        InsuranceDetailsContainer.Children.Clear();

        var card = new Border
        {
            Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 248, 250, 252)),
            BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(4),
            Padding = new Thickness(16, 12, 16, 12)
        };

        var stack = new StackPanel { Spacing = 8 };
        stack.Children.Add(new TextBlock { Text = $"Primary Carrier: {ins.Provider}", FontSize = 13, FontWeight = Microsoft.UI.Text.FontWeights.Bold, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 23, 42)) });
        stack.Children.Add(new TextBlock { Text = $"Policy / Member ID: {ins.PolicyNo}", FontSize = 11, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)) });
        stack.Children.Add(new TextBlock { Text = $"Group Number: {ins.GroupNo}", FontSize = 11, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)) });
        stack.Children.Add(new TextBlock { Text = $"Guarantor / Subscriber: {ins.Guarantor}", FontSize = 11, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)) });
        stack.Children.Add(new TextBlock { Text = $"Coverage Type: {ins.PlanType}", FontSize = 11, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 65, 85)) });

        card.Child = stack;
        InsuranceDetailsContainer.Children.Add(card);
    }

    public void SelectSection(string sectionName)
    {
        switch (sectionName.ToLowerInvariant())
        {
            case "provider_view":
            case "providerview":
            case "clin_provider_view":
                HighlightNav(NavProviderViewBorder, NavProviderViewBar, NavProviderViewText);
                ProviderViewPanel.Visibility = Visibility.Visible;
                break;
            case "results_review":
            case "resultsreview":
            case "clin_results_review":
                HighlightNav(NavResultsReviewBorder, NavResultsReviewBar, NavResultsReviewText);
                ResultsReviewPanel.Visibility = Visibility.Visible;
                break;
            case "orders":
            case "clin_orders":
                HighlightNav(NavOrdersBorder, NavOrdersBar, NavOrdersText);
                OrdersPanel.Visibility = Visibility.Visible;
                break;
            case "documentation":
            case "doc":
            case "clin_documentation":
                HighlightNav(NavDocBorder, NavDocBar, NavDocText);
                DocumentationPanel.Visibility = Visibility.Visible;
                break;
            case "outside_records":
            case "outsiderecords":
            case "clin_outside_records":
                HighlightNav(NavOutsideRecordsBorder, NavOutsideRecordsBar, NavOutsideRecordsText);
                OutsideRecordsPanel.Visibility = Visibility.Visible;
                break;
            case "allergies":
            case "clin_allergies":
                HighlightNav(NavAllergiesBorder, NavAllergiesBar, NavAllergiesText);
                AllergiesPanel.Visibility = Visibility.Visible;
                break;
            case "clinical_media":
            case "clinicalmedia":
            case "clin_clinical_media":
                HighlightNav(NavClinicalMediaBorder, NavClinicalMediaBar, NavClinicalMediaText);
                ClinicalMediaPanel.Visibility = Visibility.Visible;
                break;
            case "diagnoses":
            case "diagnoses and problems":
            case "clin_diagnoses":
                HighlightNav(NavDiagnosesBorder, NavDiagnosesBar, NavDiagnosesText);
                DiagnosesPanel.Visibility = Visibility.Visible;
                break;
            case "form_browser":
            case "formbrowser":
            case "clin_form_browser":
                HighlightNav(NavFormBrowserBorder, NavFormBrowserBar, NavFormBrowserText);
                FormBrowserPanel.Visibility = Visibility.Visible;
                break;
            case "growth_chart":
            case "growthchart":
            case "clin_growth_chart":
                HighlightNav(NavGrowthChartBorder, NavGrowthChartBar, NavGrowthChartText);
                if (GrowthChartPanel != null) GrowthChartPanel.Visibility = Visibility.Visible;
                break;
            case "insurance":
            case "patient_info":
            case "patientinfo":
            case "clin_patient_info":
                HighlightNav(NavInsuranceBorder, NavInsuranceBar, NavInsuranceText);
                InsurancePanel.Visibility = Visibility.Visible;
                break;
            case "histories":
            case "clin_histories":
                HighlightNav(NavHistoriesBorder, NavHistoriesBar, NavHistoriesText);
                if (HistoriesChartPanel != null) HistoriesChartPanel.Visibility = Visibility.Visible;
                break;
            case "interactive_view":
            case "interactiveview":
            case "clin_interactive_view":
                HighlightNav(NavInteractiveViewBorder, NavInteractiveViewBar, NavInteractiveViewText);
                InteractiveViewPanel.Visibility = Visibility.Visible;
                break;
            case "mar_summary":
            case "marsummary":
            case "clin_mar_summary":
                HighlightNav(NavMarBorder, NavMarBar, NavMarText);
                MarSummaryPanel.Visibility = Visibility.Visible;
                break;
            case "medication_list":
            case "medications":
            case "clin_medication_list":
                HighlightNav(NavMedicationBorder, NavMedicationBar, NavMedicationText);
                MedicationsPanel.Visibility = Visibility.Visible;
                break;
            default:
                HighlightNav(NavDocBorder, NavDocBar, NavDocText);
                DocumentationPanel.Visibility = Visibility.Visible;
                break;
        }
    }

    // Sidebar navigation switches
    private void ResetSidebarHighlights()
    {
        var transparent = new SolidColorBrush(Colors.Transparent);
        var white = new SolidColorBrush(Colors.White);

        // Reset borders
        NavProviderViewBorder.Background = transparent;
        NavResultsReviewBorder.Background = transparent;
        NavOrdersBorder.Background = transparent;
        NavDocBorder.Background = transparent;
        NavOutsideRecordsBorder.Background = transparent;
        NavAllergiesBorder.Background = transparent;
        NavClinicalMediaBorder.Background = transparent;
        NavDiagnosesBorder.Background = transparent;
        NavFormBrowserBorder.Background = transparent;
        NavGrowthChartBorder.Background = transparent;
        NavInsuranceBorder.Background = transparent;
        NavHistoriesBorder.Background = transparent;
        NavInteractiveViewBorder.Background = transparent;
        NavMarBorder.Background = transparent;
        NavMedicationBorder.Background = transparent;

        // Reset cyan accent bars
        NavProviderViewBar.Background = transparent;
        NavResultsReviewBar.Background = transparent;
        NavOrdersBar.Background = transparent;
        NavDocBar.Background = transparent;
        NavOutsideRecordsBar.Background = transparent;
        NavAllergiesBar.Background = transparent;
        NavClinicalMediaBar.Background = transparent;
        NavDiagnosesBar.Background = transparent;
        NavFormBrowserBar.Background = transparent;
        NavGrowthChartBar.Background = transparent;
        NavInsuranceBar.Background = transparent;
        NavHistoriesBar.Background = transparent;
        NavInteractiveViewBar.Background = transparent;
        NavMarBar.Background = transparent;
        NavMedicationBar.Background = transparent;

        // Reset text weights
        NavProviderViewText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavResultsReviewText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavOrdersText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavDocText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavOutsideRecordsText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavAllergiesText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavClinicalMediaText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavDiagnosesText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavFormBrowserText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavGrowthChartText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavInsuranceText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavHistoriesText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavInteractiveViewText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavMarText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        NavMedicationText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;

        // Collapse all content panels
        if (ProviderViewPanel != null) ProviderViewPanel.Visibility = Visibility.Collapsed;
        if (ResultsReviewPanel != null) ResultsReviewPanel.Visibility = Visibility.Collapsed;
        if (OrdersPanel != null) OrdersPanel.Visibility = Visibility.Collapsed;
        if (DocumentationPanel != null) DocumentationPanel.Visibility = Visibility.Collapsed;
        if (OutsideRecordsPanel != null) OutsideRecordsPanel.Visibility = Visibility.Collapsed;
        if (AllergiesPanel != null) AllergiesPanel.Visibility = Visibility.Collapsed;
        if (ClinicalMediaPanel != null) ClinicalMediaPanel.Visibility = Visibility.Collapsed;
        if (DiagnosesPanel != null) DiagnosesPanel.Visibility = Visibility.Collapsed;
        if (FormBrowserPanel != null) FormBrowserPanel.Visibility = Visibility.Collapsed;
        if (GrowthChartPanel != null) GrowthChartPanel.Visibility = Visibility.Collapsed;
        if (InsurancePanel != null) InsurancePanel.Visibility = Visibility.Collapsed;
        if (HistoriesChartPanel != null) HistoriesChartPanel.Visibility = Visibility.Collapsed;
        if (InteractiveViewPanel != null) InteractiveViewPanel.Visibility = Visibility.Collapsed;
        if (MarSummaryPanel != null) MarSummaryPanel.Visibility = Visibility.Collapsed;
        if (MedicationsPanel != null) MedicationsPanel.Visibility = Visibility.Collapsed;
    }

    private void HighlightNav(Border border, Border bar, TextBlock text)
    {
        ResetSidebarHighlights();
        _activeNavBorder = border;
        border.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 14, 75, 117)); // #0E4B75
        bar.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 0, 162, 237));      // #00A2ED Cyan
        text.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;
    }

    private void OnNavProviderViewPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavProviderViewBorder, NavProviderViewBar, NavProviderViewText);
        ProviderViewPanel.Visibility = Visibility.Visible;
    }

    private void OnNavResultsReviewPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavResultsReviewBorder, NavResultsReviewBar, NavResultsReviewText);
        ResultsReviewPanel.Visibility = Visibility.Visible;
    }

    private void OnNavOrdersPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavOrdersBorder, NavOrdersBar, NavOrdersText);
        OrdersPanel.Visibility = Visibility.Visible;
    }

    private void OnNavDocPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavDocBorder, NavDocBar, NavDocText);
        DocumentationPanel.Visibility = Visibility.Visible;
    }

    private void OnNavOutsideRecordsPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavOutsideRecordsBorder, NavOutsideRecordsBar, NavOutsideRecordsText);
        OutsideRecordsPanel.Visibility = Visibility.Visible;
    }

    private void OnNavAllergiesPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavAllergiesBorder, NavAllergiesBar, NavAllergiesText);
        AllergiesPanel.Visibility = Visibility.Visible;
    }

    private void OnNavClinicalMediaPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavClinicalMediaBorder, NavClinicalMediaBar, NavClinicalMediaText);
        ClinicalMediaPanel.Visibility = Visibility.Visible;
    }

    private void OnNavDiagnosesPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavDiagnosesBorder, NavDiagnosesBar, NavDiagnosesText);
        DiagnosesPanel.Visibility = Visibility.Visible;
    }

    private void OnNavFormBrowserPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavFormBrowserBorder, NavFormBrowserBar, NavFormBrowserText);
        FormBrowserPanel.Visibility = Visibility.Visible;
    }

    private void OnNavGrowthChartPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavGrowthChartBorder, NavGrowthChartBar, NavGrowthChartText);
        if (GrowthChartPanel != null) GrowthChartPanel.Visibility = Visibility.Visible;
    }

    private void OnNavInsurancePressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavInsuranceBorder, NavInsuranceBar, NavInsuranceText);
        InsurancePanel.Visibility = Visibility.Visible;
    }

    private void OnNavHistoriesPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavHistoriesBorder, NavHistoriesBar, NavHistoriesText);
        if (HistoriesChartPanel != null) HistoriesChartPanel.Visibility = Visibility.Visible;
    }

    private void OnNavInteractiveViewPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavInteractiveViewBorder, NavInteractiveViewBar, NavInteractiveViewText);
        InteractiveViewPanel.Visibility = Visibility.Visible;
    }

    private void OnNavMarPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavMarBorder, NavMarBar, NavMarText);
        MarSummaryPanel.Visibility = Visibility.Visible;
    }

    private void OnNavMedicationPressed(object sender, PointerRoutedEventArgs e)
    {
        HighlightNav(NavMedicationBorder, NavMedicationBar, NavMedicationText);
        MedicationsPanel.Visibility = Visibility.Visible;
    }

    private void OnNavPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b && b != _activeNavBorder)
        {
            b.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 19, 77, 116)); // #134D74
        }
    }

    private void OnNavPointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b && b != _activeNavBorder)
        {
            b.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
