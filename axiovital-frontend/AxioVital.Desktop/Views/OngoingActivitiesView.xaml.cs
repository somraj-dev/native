using AxioVital.Desktop.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace AxioVital.Desktop.Views;

public sealed partial class OngoingActivitiesView : UserControl
{
    public OngoingActivitiesView()
    {
        this.InitializeComponent();
        OngoingActivitiesItemsControl.ItemsSource = GetSampleActivities();
    }

    private static List<OngoingActivityItem> GetSampleActivities()
    {
        return new List<OngoingActivityItem>
        {
            new OngoingActivityItem { Location = "HPAR OLL 940 01", PatientName = "CERNER, DTTESTONE", PatientSubInfo = "62 Years Male  FIN NBR: 991...", PhysicianContact = "Gupta_Test , Hemant", Diagnosis = "Pneumonia", RowBackground = "#FFFFFF" },
            new OngoingActivityItem { Location = "HPAR OLL 940 02", PatientName = "CERNER, ESMHCTONE", PatientSubInfo = "50 Years FEMALE  FIN NBR:...", PhysicianContact = "Gupta_Test , Hemant", Diagnosis = "Add", IsDiagnosisAddLink = true, RowBackground = "#F4F8FA" },
            new OngoingActivityItem { Location = "HPAR OLL 940 03", PatientName = "CERNER, HEMTRNFOUR", PatientSubInfo = "14 Years FEMALE  FIN NBR:...", PhysicianContact = "TEST , ABSP2", Diagnosis = "Add", IsDiagnosisAddLink = true, RowBackground = "#FFFFFF" },
            new OngoingActivityItem { Location = "HPAR OLL 940 04", PatientName = "CERNER, FEMALETWOMON", PatientSubInfo = "4 Years FEMALE  FIN...", PhysicianContact = "TEST , ABSP2", Diagnosis = "Add", IsDiagnosisAddLink = true, RowBackground = "#F4F8FA" },
            new OngoingActivityItem { Location = "HPAR OLL 940 05", PatientName = "CERNER, MALEFOURYEAR", PatientSubInfo = "8 Years Male  FIN NBR:...", PhysicianContact = "Assign", IsPhysicianAssignLink = true, Diagnosis = "Add", IsDiagnosisAddLink = true, RowBackground = "#FFFFFF" },
            new OngoingActivityItem { Location = "HPAR OLL 940 06", PatientName = "CERNER, MOB", PatientSubInfo = "51 Years Male  FIN NBR: 9914544714", PhysicianContact = "Gupta_Test , Hemant", Diagnosis = "Add", IsDiagnosisAddLink = true, RowBackground = "#F4F8FA" },
            new OngoingActivityItem { Location = "HPAR OLL 940 08", PatientName = "Name,;", PatientSubInfo = "34 Years Male  FIN NBR: PHARMACYONLY2", PhysicianContact = "Gupta_Test , Hemant", Diagnosis = "Add", IsDiagnosisAddLink = true, RowBackground = "#FFFFFF" },
            new OngoingActivityItem { Location = "HPAR OLL 940", PatientName = "CERNER, PREADMITMB", PatientSubInfo = "51 Years Male  FIN NBR: 99...", PhysicianContact = "Hardi , Umar M, MD", Diagnosis = "Aspiration pneumonia...", RowBackground = "#F4F8FA" },
            new OngoingActivityItem { Location = "HPAR OLL 940", PatientName = "cerner, motestnew", PatientSubInfo = "32 Years Male  FIN NBR: 9000062...", PhysicianContact = "Comeno_Test , Catherine", Diagnosis = "Add", IsDiagnosisAddLink = true, RowBackground = "#FFFFFF" }
        };
    }
}
