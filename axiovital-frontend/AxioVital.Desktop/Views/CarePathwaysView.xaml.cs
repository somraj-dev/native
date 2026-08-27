using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AxioVital.Desktop.Views;

/// <summary>
/// 1:1 Replica of Cerner Imaging: Online Work List for AxioVital Care Pathways full screen mode.
/// </summary>
public sealed partial class CarePathwaysView : UserControl
{
    public event EventHandler? ExitRequested;

    private readonly List<WorkListItem> _allCases = new();
    private readonly ObservableCollection<WorkListItem> _displayedCases = new();
    private bool _isSortAscending = true;

    public CarePathwaysView()
    {
        this.InitializeComponent();
        this.IsTabStop = true;
        InitializeWorkListData();
        ApplyFilter();
        this.Loaded += (s, e) => this.Focus(FocusState.Programmatic);
        this.KeyDown += OnCarePathwaysKeyDown;
    }

    private void OnCarePathwaysKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Escape)
        {
            if (CaseDetailsModalOverlay != null && CaseDetailsModalOverlay.Visibility == Visibility.Visible)
            {
                CaseDetailsModalOverlay.Visibility = Visibility.Collapsed;
                e.Handled = true;
                return;
            }

            ExitRequested?.Invoke(this, EventArgs.Empty);
            e.Handled = true;
        }
    }

    private void InitializeWorkListData()
    {
        _allCases.Clear();

        // Exact cases matching screenshot
        var rawData = new[]
        {
            ("SNIRNSURG15, Ella", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000064", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600015", "003600015"),
            ("SNIRNSURG15, Ella", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000063", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600015", "003600015"),
            ("SNIRNSURG14, Chloe", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000062", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600014", "003600014"),
            ("SNIRNSURG14, Chloe", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000061", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600014", "003600014"),
            ("SNIRNSURG13, Sofia", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000060", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600013", "003600013"),
            ("SNIRNSURG13, Sofia", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000059", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600013", "003600013"),
            ("SNIRNSURG12, Avery", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000058", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600012", "003600012"),
            ("SNIRNSURG12, Avery", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000057", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600012", "003600012"),
            ("SNIRNSURG11, Charlotte", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000055", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600011", "003600011"),
            ("SNIRNSURG11, Charlotte", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000056", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600011", "003600011"),
            ("SNIRNSURG10, Elizabeth", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000054", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "VMC IntraOp ENDO", "003600010", "003600010"),
            ("SNIRNSURG10, Elizabeth", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000053", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "VMC IntraOp ENDO", "003600010", "003600010"),
            ("SNIRNSURG09, Madison", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000052", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600009", "003600009"),
            ("SNIRNSURG09, Madison", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000051", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600009", "003600009"),
            ("SNIRNSURG08, Abigail", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000050", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600008", "003600008"),
            ("SNIRNSURG08, Abigail", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000049", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600008", "003600008"),
            ("SNIRNSURG07, Emily", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000048", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600007", "003600007"),
            ("SNIRNSURG07, Emily", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000047", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600007", "003600007"),
            ("SNIRNSURG06, Mia", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000046", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600006", "003600006"),
            ("SNIRNSURG06, Mia", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000045", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600006", "003600006"),
            ("SNIRNSURG05, Ava", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000044", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600005", "003600005"),
            ("SNIRNSURG05, Ava", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000043", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600005", "003600005"),
            ("SNIRNSURG04, Isabella", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000041", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600004", "003600004"),
            ("SNIRNSURG04, Isabella", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000042", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600004", "003600004"),
            ("SNIRNSURG03, Olivia", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000039", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600003", "003600003"),
            ("SNIRNSURG03, Olivia", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000040", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600003", "003600003"),
            ("SNIRNSURG02, Emma", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000037", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600002", "003600002"),
            ("SNIRNSURG02, Emma", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000038", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600002", "003600002"),
            ("SNIRNSURG01, Sophia", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000014", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "RSDS", "003600001", "003600001"),
            ("SNIRNSURG00, PeriopTEST", true, "Inpatient", "XR Knee Complete 4+ Views", "XR-21-0000010", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "VMC OR", "003600000", "003600000"),
            ("SNIRNSURG00, PeriopTEST", true, "Inpatient", "XR Chest 2 Views", "XR-21-0000011", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "VMC OR", "003600000", "003600000"),
            ("RADTECH13, Sofia", true, "Outpatient", "MRI Knee w/o Contrast Left", "MR-21-0000014", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "V Radiology", "003400013", "003400013"),
            ("RADTECH13, Sofia", true, "Outpatient", "CT Thorax w/o Contrast", "CT-21-0000015", "Routine", "6/1/2023 1:00 AM", "4/5/2022 9:12 AM", "Replaced", "V Radiology", "003400013", "003400013"),
            ("RADTECH13, Sofia", true, "Outpatient", "XR Chest 1 View", "XR-21-0000141", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "V Radiology", "003400013", "003400013"),
            ("RADTECH13, Sofia", true, "Outpatient", "XR Foot 2 Views Right", "XR-22-0000043", "Routine", "6/1/2023 1:00 AM", "6/1/2022 10:12 AM", "Canceled", "V Radiology", "003400013", "003400013"),
            ("RADTECH13, Sofia", true, "Outpatient", "CT Thorax w/o Contrast", "CT-21-0000015", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "V Radiology", "003400013", "003400013"),
            ("RADTECH13, Sofia", true, "Outpatient", "XR Elbow 2 Views Left", "XR-21-0000142", "Routine", "6/1/2023 1:00 AM", "6/1/2023 1:00 AM", "Ordered", "V Radiology", "003400013", "003400013"),
            ("RADTECH13, Sofia", true, "Outpatient", "XR Hand 2 Views Left", "XR-21-0000143", "Routine", "6/1/2023 1:00 AM", "12/13/2021 8:37 AM", "Final", "V Radiology", "003400013", "003400013")
        };

        foreach (var item in rawData)
        {
            _allCases.Add(new WorkListItem
            {
                PatientName = item.Item1,
                HasAllergies = item.Item2,
                PatientType = item.Item3,
                ProcedureName = item.Item4,
                AccessionNumber = item.Item5,
                Priority = item.Item6,
                RequestedDateTime = item.Item7,
                StatusDateTime = item.Item8,
                Status = item.Item9,
                NurseUnit = item.Item10,
                MRN = item.Item11,
                FIN = item.Item12
            });
        }

        // Generate additional cases to achieve the exact 196 total cases shown in the screenshot
        var sampleProcedures = new[]
        {
            "XR Pelvis 1-2 Views", "CT Abdomen/Pelvis w/ Contrast", "US Abdomen Complete", "XR Spine Lumbar 2-3 Views",
            "MRI Brain w/ & w/o Contrast", "XR Shoulder Complete 2+ Views", "CT Head w/o Contrast", "US Lower Extremity Venous",
            "XR Ankle 3 Views Right", "XR Wrist 3 Views Left", "MRI Spine Lumbar w/o Contrast", "NM Bone Scan Whole Body"
        };
        var sampleUnits = new[] { "RSDS", "VMC OR", "VMC IntraOp ENDO", "V Radiology", "ICU-EAST", "3-NORTH", "ED-TRAUMA" };
        var sampleStatuses = new[] { "Ordered", "Ordered", "Ordered", "In Progress", "Replaced", "Canceled", "Final" };
        var sampleTypes = new[] { "Inpatient", "Inpatient", "Outpatient" };

        int seedCount = _allCases.Count;
        for (int i = seedCount; i < 196; i++)
        {
            int pNum = (i - seedCount) + 16;
            string pName = $"SNIRNSURG{pNum:D2}, Patient{pNum}";
            string proc = sampleProcedures[i % sampleProcedures.Length];
            string unit = sampleUnits[i % sampleUnits.Length];
            string stat = sampleStatuses[i % sampleStatuses.Length];
            string pType = sampleTypes[i % sampleTypes.Length];
            string mrn = $"00360{i:D4}";
            string acc = $"XR-21-0000{i + 50:D3}";

            _allCases.Add(new WorkListItem
            {
                PatientName = pName,
                HasAllergies = true,
                PatientType = pType,
                ProcedureName = proc,
                AccessionNumber = acc,
                Priority = (i % 8 == 0) ? "STAT" : "Routine",
                RequestedDateTime = "6/1/2023 1:00 AM",
                StatusDateTime = (stat == "Final" || stat == "Replaced") ? "5/12/2023 2:15 PM" : "6/1/2023 1:00 AM",
                Status = stat,
                NurseUnit = unit,
                MRN = mrn,
                FIN = mrn
            });
        }

        WorkListListView.ItemsSource = _displayedCases;
    }

    private void ApplyFilter()
    {
        string selectedStatus = "All";
        if (ExamStatusComboBox?.SelectedItem is ComboBoxItem cbiStatus && cbiStatus.Content is string sText)
        {
            selectedStatus = sText;
        }

        string searchText = FilterSearchBox?.Text?.Trim() ?? string.Empty;

        var query = _allCases.AsEnumerable();

        if (!string.Equals(selectedStatus, "All", StringComparison.OrdinalIgnoreCase))
        {
            query = query.Where(c => string.Equals(c.Status, selectedStatus, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(c =>
                c.PatientName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                c.ProcedureName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                c.AccessionNumber.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                c.MRN.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                c.NurseUnit.Contains(searchText, StringComparison.OrdinalIgnoreCase));
        }

        if (_isSortAscending)
        {
            query = query.OrderBy(c => c.PatientName);
        }
        else
        {
            query = query.OrderByDescending(c => c.PatientName);
        }

        _displayedCases.Clear();
        int rowIndex = 0;
        foreach (var item in query)
        {
            item.BackgroundBrush = (rowIndex % 2 == 0) ? new SolidColorBrush(Microsoft.UI.Colors.White) : new SolidColorBrush(Windows.UI.Color.FromArgb(255, 246, 250, 254));
            _displayedCases.Add(item);
            rowIndex++;
        }
    }

    private void OnExamStatusSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ApplyFilter();
    }

    private void OnScheduleIndicatorSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ApplyFilter();
    }

    private void OnFilterSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }

    private void OnSortByPatientNameClicked(object sender, PointerRoutedEventArgs e)
    {
        _isSortAscending = !_isSortAscending;
        ApplyFilter();
    }

    private void OnTableScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
        if (HeaderScrollViewer != null && TableScrollViewer != null)
        {
            HeaderScrollViewer.ChangeView(TableScrollViewer.HorizontalOffset, null, null, true);
        }
    }

    private void OnWorkListSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    private void OnWorkListDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        ShowSelectedCaseDetails();
    }

    private void ShowSelectedCaseDetails()
    {
        var selected = WorkListListView.SelectedItem as WorkListItem ?? _displayedCases.FirstOrDefault();
        if (selected != null && CaseDetailsModalOverlay != null)
        {
            ModalPatientNameText.Text = selected.PatientName;
            ModalMrnFinText.Text = $"{selected.MRN} / {selected.FIN}";
            ModalProcedureText.Text = selected.ProcedureName;
            ModalAccessionText.Text = selected.AccessionNumber;
            ModalPatientTypeText.Text = selected.PatientType;
            ModalPriorityText.Text = selected.Priority;
            ModalStatusText.Text = selected.Status;
            ModalNurseUnitText.Text = selected.NurseUnit;
            ModalDateText.Text = $"{selected.RequestedDateTime} / {selected.StatusDateTime}";

            CaseDetailsModalOverlay.Visibility = Visibility.Visible;
        }
    }

    private void OnCloseCaseDetailsClicked(object sender, RoutedEventArgs e)
    {
        if (CaseDetailsModalOverlay != null)
        {
            CaseDetailsModalOverlay.Visibility = Visibility.Collapsed;
        }
    }

    private void OnCaseDetailsBackdropPressed(object sender, PointerRoutedEventArgs e)
    {
        if (ReferenceEquals(e.OriginalSource, CaseDetailsModalOverlay))
        {
            CaseDetailsModalOverlay.Visibility = Visibility.Collapsed;
        }
    }

    private void OnCaseDetailsDialogPressed(object sender, PointerRoutedEventArgs e)
    {
        e.Handled = true;
    }

    private void OnExamTabClicked(object sender, PointerRoutedEventArgs e)
    {
        if (ExamTabBorder != null && TranscriptionTabBorder != null && ExamTabText != null && TranscriptionTabText != null)
        {
            ExamTabBorder.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
            ExamTabText.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
            ExamTabText.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 10, 60, 107));

            TranscriptionTabBorder.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 213, 227, 240));
            TranscriptionTabText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
            TranscriptionTabText.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 51, 65, 85));
        }
    }

    private void OnTranscriptionTabClicked(object sender, PointerRoutedEventArgs e)
    {
        if (ExamTabBorder != null && TranscriptionTabBorder != null && ExamTabText != null && TranscriptionTabText != null)
        {
            TranscriptionTabBorder.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
            TranscriptionTabText.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
            TranscriptionTabText.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 10, 60, 107));

            ExamTabBorder.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 213, 227, 240));
            ExamTabText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
            ExamTabText.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 51, 65, 85));
        }
    }

    private void OnRefreshClicked(object sender, RoutedEventArgs e)
    {
        if (AsOfTimestampText != null)
        {
            AsOfTimestampText.Text = $"As of: {DateTime.Now:h:mm tt}";
        }
        ApplyFilter();
    }

    private void OnExitClicked(object sender, RoutedEventArgs e)
    {
        ExitRequested?.Invoke(this, EventArgs.Empty);
    }
}

public class WorkListItem
{
    public string PatientName { get; set; } = string.Empty;
    public bool HasAllergies { get; set; } = true;
    public string PatientType { get; set; } = "Inpatient";
    public string ProcedureName { get; set; } = string.Empty;
    public string AccessionNumber { get; set; } = string.Empty;
    public string Priority { get; set; } = "Routine";
    public string RequestedDateTime { get; set; } = string.Empty;
    public string StatusDateTime { get; set; } = string.Empty;
    public string Status { get; set; } = "Ordered";
    public string NurseUnit { get; set; } = string.Empty;
    public string MRN { get; set; } = string.Empty;
    public string FIN { get; set; } = string.Empty;
    public SolidColorBrush BackgroundBrush { get; set; } = new SolidColorBrush(Microsoft.UI.Colors.White);
}
