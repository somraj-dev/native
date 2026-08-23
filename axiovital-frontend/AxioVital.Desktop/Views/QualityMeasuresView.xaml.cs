using AxioVital.Desktop.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;

namespace AxioVital.Desktop.Views;

public sealed partial class QualityMeasuresView : UserControl
{
    public ObservableCollection<QualityMeasureRecord> Attendances { get; set; } = new();

    private bool _isSidebarCollapsed = false;

    public QualityMeasuresView()
    {
        this.InitializeComponent();
        LoadSampleData();
    }

    private void LoadSampleData()
    {
        Attendances.Clear();

        // Exact clinical data records matching reference image
        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759556",
            GenderDesc = "Female",
            Diagnosis1 = "Alleged victim of physical assault",
            Diagnosis2 = "Soft tissue injury",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759562",
            GenderDesc = "Male",
            Diagnosis1 = "Laceration of head",
            Diagnosis2 = "Soft tissue injury",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759565",
            GenderDesc = "Male",
            Diagnosis1 = "Laceration of hand",
            Diagnosis2 = "",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759570",
            GenderDesc = "Female",
            Diagnosis1 = "Fall from height",
            Diagnosis2 = "Fracture of radius",
            Diagnosis3 = "Soft tissue injury"
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759572",
            GenderDesc = "Female",
            Diagnosis1 = "Assault with blunt object",
            Diagnosis2 = "Contusion of face",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759578",
            GenderDesc = "Male",
            Diagnosis1 = "Accidental puncture",
            Diagnosis2 = "Open wound of finger",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759583",
            GenderDesc = "Female",
            Diagnosis1 = "Laceration of scalp",
            Diagnosis2 = "Head injury",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759591",
            GenderDesc = "Male",
            Diagnosis1 = "Fracture of clavicle",
            Diagnosis2 = "Contusion of shoulder",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759599",
            GenderDesc = "Male",
            Diagnosis1 = "Sprain of ankle",
            Diagnosis2 = "Soft tissue injury",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759604",
            GenderDesc = "Female",
            Diagnosis1 = "Burn of second degree",
            Diagnosis2 = "Blister of hand",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759612",
            GenderDesc = "Female",
            Diagnosis1 = "Contusion of knee",
            Diagnosis2 = "",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759620",
            GenderDesc = "Male",
            Diagnosis1 = "Dislocation of finger",
            Diagnosis2 = "Joint injury",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759635",
            GenderDesc = "Male",
            Diagnosis1 = "Foreign body in eye",
            Diagnosis2 = "Corneal abrasion",
            Diagnosis3 = ""
        });

        Attendances.Add(new QualityMeasureRecord
        {
            AttendanceNumber = "759642",
            GenderDesc = "Female",
            Diagnosis1 = "Animal bite",
            Diagnosis2 = "Puncture wound",
            Diagnosis3 = "Tetanus prophylaxis"
        });

        if (AttendanceItemsList != null)
        {
            AttendanceItemsList.ItemsSource = Attendances;
        }
    }

    private void OnToggleSidebarClick(object sender, RoutedEventArgs e)
    {
        _isSidebarCollapsed = !_isSidebarCollapsed;
        if (LeftPaneCol != null)
        {
            LeftPaneCol.Width = _isSidebarCollapsed ? new GridLength(0) : new GridLength(250);
        }
    }

    private void OnNewFilterClick(object sender, RoutedEventArgs e)
    {
        ResetFilters();
    }

    private void OnResetFilterClick(object sender, RoutedEventArgs e)
    {
        ResetFilters();
    }

    private void ResetFilters()
    {
        if (DiagnosisParentCombo != null) DiagnosisParentCombo.SelectedIndex = 0;
        if (DrugRelatedCombo != null) DrugRelatedCombo.SelectedIndex = 0;
        if (RestrictedDiagnosisCombo != null) RestrictedDiagnosisCombo.SelectedIndex = 0;
        if (HeadFindingCombo != null) HeadFindingCombo.SelectedIndex = 0;
    }

    private void OnTabAttendancesPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        SetSubTabActive(TabAttendancesBorder, true);
        SetSubTabActive(TabDiagnosisBorder, false);
        SetSubTabActive(TabSummaryBorder, false);
    }

    private void OnTabDiagnosisPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        SetSubTabActive(TabAttendancesBorder, false);
        SetSubTabActive(TabDiagnosisBorder, true);
        SetSubTabActive(TabSummaryBorder, false);
    }

    private void OnTabSummaryPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        SetSubTabActive(TabAttendancesBorder, false);
        SetSubTabActive(TabDiagnosisBorder, false);
        SetSubTabActive(TabSummaryBorder, true);
    }

    private void SetSubTabActive(Border tabBorder, bool isActive)
    {
        if (tabBorder == null) return;

        if (isActive)
        {
            tabBorder.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
            tabBorder.BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 186, 195, 204));
            tabBorder.BorderThickness = new Thickness(1, 1, 1, 0);
        }
        else
        {
            tabBorder.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            tabBorder.BorderBrush = null;
            tabBorder.BorderThickness = new Thickness(0);
        }
    }
}
