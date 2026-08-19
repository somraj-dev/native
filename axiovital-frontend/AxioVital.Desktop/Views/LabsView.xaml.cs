using AxioVital.Desktop.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace AxioVital.Desktop.Views;

public sealed partial class LabsView : UserControl
{
    public LabsView()
    {
        this.InitializeComponent();
        LabsItemsControl.ItemsSource = GetSampleLabOrderItems();
    }

    private static List<LabOrderItem> GetSampleLabOrderItems()
    {
        return new List<LabOrderItem>
        {
            new LabOrderItem { PatientName = "JAMES, WILLIAM", PlanName = "CBC with Differential", Action = "Plan", DetailsDate = "05/28/17 08:30...", DetailsNotes = "Routine blood count", Status = "Completed", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "JAMES, WILLIAM", PlanName = "Comprehensive Metabolic Panel", Action = "Plan", DetailsDate = "05/28/17 08:30...", DetailsNotes = "Kidney & liver function", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "PATEL, RAHUL", PlanName = "MRI Brain W/O Contrast", Action = "Plan", DetailsDate = "05/28/17 09:15...", DetailsNotes = "Headache evaluation", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "PATEL, RAHUL", PlanName = "Referral to City Neuro Hospital", Action = "Referral", DetailsDate = "05/28/17 09:15...", DetailsNotes = "Transfer for advanced neuro care", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "JOHNSON, MARIA", PlanName = "PT Evaluation", Action = "Plan", DetailsDate = "05/28/17 10:00...", DetailsNotes = "Post op rehab", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "JOHNSON, MARIA", PlanName = "Referral to St. Mary Regional Medical", Action = "Referral", DetailsDate = "05/28/17 10:00...", DetailsNotes = "Transfer for specialty pain mgmt", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "LEE, DAVID", PlanName = "Chest X-Ray", Action = "Plan", DetailsDate = "05/28/17 10:30...", DetailsNotes = "Cough and fever", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "LEE, DAVID", PlanName = "Sputum Culture", Action = "Plan", DetailsDate = "05/28/17 10:30...", DetailsNotes = "Infection workup", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "GARCIA, LUCIA", PlanName = "Echocardiogram", Action = "Plan", DetailsDate = "05/28/17 11:00...", DetailsNotes = "Cardiac evaluation", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "GARCIA, LUCIA", PlanName = "Referral to Metro Heart Institute", Action = "Referral", DetailsDate = "05/28/17 11:00...", DetailsNotes = "Transfer for cardiac surgery eval", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "KIM, JAMES", PlanName = "Hemoglobin A1C", Action = "Plan", DetailsDate = "05/28/17 11:30...", DetailsNotes = "Diabetes monitoring", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "KIM, JAMES", PlanName = "Diabetes Education", Action = "Plan", DetailsDate = "05/28/17 11:30...", DetailsNotes = "Patient education", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "BROWN, ELIZABETH", PlanName = "Urinalysis", Action = "Plan", DetailsDate = "05/28/17 12:00...", DetailsNotes = "UTI symptoms", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "BROWN, ELIZABETH", PlanName = "Urine Culture", Action = "Plan", DetailsDate = "05/28/17 12:00...", DetailsNotes = "Confirm infection", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "THOMAS, MICHAEL", PlanName = "CT Abdomen & Pelvis", Action = "Plan", DetailsDate = "05/28/17 12:30...", DetailsNotes = "Abdominal pain", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "THOMAS, MICHAEL", PlanName = "Referral to General Surgical Center", Action = "Referral", DetailsDate = "05/28/17 12:30...", DetailsNotes = "Transfer for emergency surgery", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "ANDERSON, SUSAN", PlanName = "Lipid Panel", Action = "Plan", DetailsDate = "05/28/17 13:00...", DetailsNotes = "Cholesterol check", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "ANDERSON, SUSAN", PlanName = "Nutrition Consult", Action = "Plan", DetailsDate = "05/28/17 13:00...", DetailsNotes = "Dietary counselling", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "MILLER, ROBERT", PlanName = "Pulmonary Function Test", Action = "Plan", DetailsDate = "05/28/17 13:30...", DetailsNotes = "COPD evaluation", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "MILLER, ROBERT", PlanName = "Referral to Pulmonary Care Hospital", Action = "Referral", DetailsDate = "05/28/17 13:30...", DetailsNotes = "Transfer for advanced COPD care", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "DAVIS, PATRICIA", PlanName = "DEXA Scan", Action = "Plan", DetailsDate = "05/28/17 14:00...", DetailsNotes = "Bone density", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "DAVIS, PATRICIA", PlanName = "Vitamin D Level", Action = "Plan", DetailsDate = "05/28/17 14:00...", DetailsNotes = "Bone health", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "WHITE, CHARLES", PlanName = "Sleep Study", Action = "Plan", DetailsDate = "05/28/17 14:30...", DetailsNotes = "Sleep apnea evaluation", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "WHITE, CHARLES", PlanName = "Referral to Sleep Disorders Clinic", Action = "Referral", DetailsDate = "05/28/17 14:30...", DetailsNotes = "Transfer for sleep study & ENT eval", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" },
            new LabOrderItem { PatientName = "WILSON, BETTY", PlanName = "Mammogram Screening", Action = "Plan", DetailsDate = "05/28/17 15:00...", DetailsNotes = "Breast cancer screening", Status = "Open", StatusColor = "#008000", RowBackground = "#FFFFFF" },
            new LabOrderItem { PatientName = "WILSON, BETTY", PlanName = "Ob/Gyn Annual Exam", Action = "Plan", DetailsDate = "05/28/17 15:00...", DetailsNotes = "Routine exam", Status = "Open", StatusColor = "#008000", RowBackground = "#F4F8FA" }
        };
    }
}
