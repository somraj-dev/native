using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Windows.Storage.Pickers;

namespace AxioVital.Desktop.Views;

/// <summary>
/// Medical Document and File Profiler with category filtering, search, grid/list views, metadata visualizer, and upload integration.
/// Supports 20+ healthcare file formats (PDF, DICOM, images, spreadsheets, archives, HL7/FHIR data).
/// </summary>
public sealed partial class FileManagerView : UserControl
{
    public event EventHandler? CloseRequested;

    public sealed class FileItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FileName { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public string Category { get; set; } = "Other";
        public string UploadedBy { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = string.Empty;
        public string LocalPath { get; set; } = string.Empty;
    }

    private List<FileItem> _allFiles = new();
    private List<FileItem> _filteredFiles = new();
    private string? _activeCategory = null;
    private string? _searchTerm = null;
    private bool _isGridView = true;
    private FileItem? _selectedFile = null;

    public FileManagerView()
    {
        this.InitializeComponent();
        LoadMockData();
        ApplyFilters();
    }

    private void LoadMockData()
    {
        _allFiles = new List<FileItem>
        {
            // Medical Reports
            new() { FileName = "Discharge_Summary_2024.pdf", FileExtension = ".pdf", ContentType = "application/pdf",
                     FileSizeBytes = 2_450_000, Category = "MedicalReport", UploadedBy = "Dr. Sarah Chen, MD",
                     UploadedAt = DateTime.UtcNow.AddDays(-2), Description = "Patient discharge summary with follow-up outpatient medication reconciliation" },
            new() { FileName = "Progress_Notes_Week12.docx", FileExtension = ".docx", ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                     FileSizeBytes = 890_000, Category = "MedicalReport", UploadedBy = "Dr. Louis P. Krenn, MD",
                     UploadedAt = DateTime.UtcNow.AddDays(-5), Description = "Weekly cardiology progress notes and treatment milestones" },
            new() { FileName = "Clinical_Assessment_Report.pdf", FileExtension = ".pdf", ContentType = "application/pdf",
                     FileSizeBytes = 3_200_000, Category = "MedicalReport", UploadedBy = "Dr. Emily Roberts, MD",
                     UploadedAt = DateTime.UtcNow.AddDays(-8), Description = "Comprehensive outpatient clinical evaluation report" },

            // Lab Results
            new() { FileName = "CBC_Blood_Panel_Results.pdf", FileExtension = ".pdf", ContentType = "application/pdf",
                     FileSizeBytes = 1_200_000, Category = "LabResult", UploadedBy = "Quest Diagnostics",
                     UploadedAt = DateTime.UtcNow.AddDays(-1), Description = "Complete Blood Count with WBC differential and platelet count" },
            new() { FileName = "Metabolic_Panel_Q3.xlsx", FileExtension = ".xlsx", ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                     FileSizeBytes = 445_000, Category = "LabResult", UploadedBy = "PathLab Inc.",
                     UploadedAt = DateTime.UtcNow.AddDays(-3), Description = "Comprehensive metabolic profile data sheet" },
            new() { FileName = "Lipid_Profile_August.csv", FileExtension = ".csv", ContentType = "text/csv",
                     FileSizeBytes = 12_000, Category = "LabResult", UploadedBy = "LabCorp Diagnostics",
                     UploadedAt = DateTime.UtcNow.AddDays(-7), Description = "Raw lipid panel data for telemetry import" },
            new() { FileName = "Urinalysis_Report.pdf", FileExtension = ".pdf", ContentType = "application/pdf",
                     FileSizeBytes = 680_000, Category = "LabResult", UploadedBy = "PathLab Inc.",
                     UploadedAt = DateTime.UtcNow.AddDays(-12), Description = "Routine urinalysis test findings" },

            // Imaging Studies (DICOM / Medical Scans)
            new() { FileName = "Chest_XRay_PA_View.dcm", FileExtension = ".dcm", ContentType = "application/dicom",
                     FileSizeBytes = 18_500_000, Category = "ImagingStudy", UploadedBy = "Mercy Radiology Dept.",
                     UploadedAt = DateTime.UtcNow.AddDays(-4), Description = "DICOM 3.0 PA & Lateral chest radiograph study" },
            new() { FileName = "Brain_MRI_Contrast.dcm", FileExtension = ".dcm", ContentType = "application/dicom",
                     FileSizeBytes = 52_000_000, Category = "ImagingStudy", UploadedBy = "Neuro Imaging Center",
                     UploadedAt = DateTime.UtcNow.AddDays(-10), Description = "T1/T2 axial volumetric MRI series with gadolinium contrast" },
            new() { FileName = "Ultrasound_Abdomen.jpg", FileExtension = ".jpg", ContentType = "image/jpeg",
                     FileSizeBytes = 4_800_000, Category = "ImagingStudy", UploadedBy = "Dr. Lisa Park, MD",
                     UploadedAt = DateTime.UtcNow.AddDays(-6), Description = "High-resolution Doppler abdominal ultrasound scan" },

            // Prescriptions
            new() { FileName = "Prescription_Metformin_500mg.pdf", FileExtension = ".pdf", ContentType = "application/pdf",
                     FileSizeBytes = 340_000, Category = "Prescription", UploadedBy = "Dr. Sarah Chen, MD",
                     UploadedAt = DateTime.UtcNow.AddDays(-1), Description = "Signed e-Prescription for Metformin 500mg oral tablets" },
            new() { FileName = "Rx_Lisinopril_10mg.pdf", FileExtension = ".pdf", ContentType = "application/pdf",
                     FileSizeBytes = 280_000, Category = "Prescription", UploadedBy = "Dr. Louis P. Krenn, MD",
                     UploadedAt = DateTime.UtcNow.AddDays(-14), Description = "Signed e-Prescription for Lisinopril 10mg daily" },

            // Insurance & Claims
            new() { FileName = "Insurance_Card_Front_Back.png", FileExtension = ".png", ContentType = "image/png",
                     FileSizeBytes = 1_100_000, Category = "InsuranceDocument", UploadedBy = "Front Desk Admissions",
                     UploadedAt = DateTime.UtcNow.AddDays(-30), Description = "Scanned Medicare / BCBS subscriber card" },
            new() { FileName = "EOB_Claim_2024_Q2.pdf", FileExtension = ".pdf", ContentType = "application/pdf",
                     FileSizeBytes = 1_800_000, Category = "InsuranceDocument", UploadedBy = "BlueCross BlueShield",
                     UploadedAt = DateTime.UtcNow.AddDays(-20), Description = "Explanation of Benefits for outpatient clinic visit" },

            // Consent Forms
            new() { FileName = "Informed_Consent_Outpatient.pdf", FileExtension = ".pdf", ContentType = "application/pdf",
                     FileSizeBytes = 520_000, Category = "ConsentForm", UploadedBy = "Patient Registration",
                     UploadedAt = DateTime.UtcNow.AddDays(-15), Description = "Signed electronic patient treatment consent & HIPAA disclosure" },

            // Referrals
            new() { FileName = "Referral_Cardiology_Dr_Kim.pdf", FileExtension = ".pdf", ContentType = "application/pdf",
                     FileSizeBytes = 410_000, Category = "Referral", UploadedBy = "Dr. Sarah Chen, MD",
                     UploadedAt = DateTime.UtcNow.AddDays(-9), Description = "Specialist consultation referral for echocardiogram" },

            // Surgical Notes
            new() { FileName = "Operative_Report_Appendectomy.pdf", FileExtension = ".pdf", ContentType = "application/pdf",
                     FileSizeBytes = 1_900_000, Category = "SurgicalNote", UploadedBy = "Dr. Robert Martinez, MD",
                     UploadedAt = DateTime.UtcNow.AddDays(-22), Description = "Detailed postoperative surgical summary and findings" },

            // Other
            new() { FileName = "FHIR_Clinical_Bundle.json", FileExtension = ".json", ContentType = "application/json",
                     FileSizeBytes = 95_000, Category = "Other", UploadedBy = "Interoperability Engine",
                     UploadedAt = DateTime.UtcNow.AddDays(-18), Description = "HL7 FHIR R4 Patient Resource Bundle" },
            new() { FileName = "Patient_History_Archive.zip", FileExtension = ".zip", ContentType = "application/zip",
                     FileSizeBytes = 8_400_000, Category = "Other", UploadedBy = "Medical Records Dept.",
                     UploadedAt = DateTime.UtcNow.AddDays(-45), Description = "Compressed archive of historical paper records" }
        };
    }

    private void ApplyFilters()
    {
        var query = _allFiles.AsEnumerable();

        if (!string.IsNullOrEmpty(_activeCategory))
            query = query.Where(f => f.Category.Equals(_activeCategory, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(_searchTerm))
        {
            var term = _searchTerm.Trim();
            query = query.Where(f =>
                f.FileName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                f.Description.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                f.UploadedBy.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                f.FileExtension.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        // Sort
        var sortIndex = SortComboBox?.SelectedIndex ?? 0;
        query = sortIndex switch
        {
            0 => query.OrderByDescending(f => f.UploadedAt),
            1 => query.OrderBy(f => f.UploadedAt),
            2 => query.OrderBy(f => f.FileName),
            3 => query.OrderByDescending(f => f.FileName),
            4 => query.OrderByDescending(f => f.FileSizeBytes),
            5 => query.OrderBy(f => f.FileSizeBytes),
            _ => query.OrderByDescending(f => f.UploadedAt)
        };

        _filteredFiles = query.ToList();

        // Update counts
        if (FileCountText != null) FileCountText.Text = $"{_filteredFiles.Count} file{(_filteredFiles.Count != 1 ? "s" : "")}";
        if (TotalSizeText != null) TotalSizeText.Text = $"{FormatFileSize(_filteredFiles.Sum(f => f.FileSizeBytes))} total";
        if (FilterAllCount != null) FilterAllCount.Text = $"({_allFiles.Count})";

        // Show/hide empty state
        if (EmptyState != null) EmptyState.Visibility = _filteredFiles.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        // Render view
        if (_isGridView)
            RenderGridView();
        else
            RenderListView();
    }

    private void RenderGridView()
    {
        if (FileGridView == null || FileListView == null) return;
        FileGridView.Visibility = Visibility.Visible;
        FileListView.Visibility = Visibility.Collapsed;

        FileGridView.Items.Clear();
        foreach (var file in _filteredFiles)
        {
            FileGridView.Items.Add(CreateFileCard(file));
        }
    }

    private void RenderListView()
    {
        if (FileGridView == null || FileListView == null || FileListItems == null) return;
        FileGridView.Visibility = Visibility.Collapsed;
        FileListView.Visibility = Visibility.Visible;

        FileListItems.Items.Clear();
        foreach (var file in _filteredFiles)
        {
            FileListItems.Items.Add(CreateFileListRow(file));
        }
    }

    private Border CreateFileCard(FileItem file)
    {
        var iconInfo = GetFileTypeIcon(file.FileExtension);

        var card = new Border
        {
            Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)),
            BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 226, 232, 240)),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(8),
            Margin = new Thickness(4),
            Padding = new Thickness(0),
            Width = 186,
            Height = 160,
            Tag = file.Id
        };

        var mainGrid = new Grid();
        mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(90) });
        mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        // Icon area
        var iconArea = new Border
        {
            Background = new SolidColorBrush(iconInfo.bgColor),
            CornerRadius = new CornerRadius(8, 8, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        var iconText = new TextBlock
        {
            Text = iconInfo.icon,
            FontSize = 34,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        iconArea.Child = iconText;
        Grid.SetRow(iconArea, 0);

        // Extension badge
        var extBadge = new Border
        {
            Background = new SolidColorBrush(iconInfo.badgeColor),
            CornerRadius = new CornerRadius(3),
            Padding = new Thickness(5, 1, 5, 1),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 6, 6, 0)
        };
        extBadge.Child = new TextBlock
        {
            Text = file.FileExtension.TrimStart('.').ToUpperInvariant(),
            FontSize = 8.5,
            FontWeight = Microsoft.UI.Text.FontWeights.Bold,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255))
        };
        Grid.SetRow(extBadge, 0);

        // File name
        var nameBlock = new TextBlock
        {
            Text = file.FileName,
            FontSize = 11,
            FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 30, 41, 59)),
            TextTrimming = TextTrimming.CharacterEllipsis,
            Margin = new Thickness(8, 6, 8, 2),
            MaxLines = 1
        };
        Grid.SetRow(nameBlock, 1);

        // File info
        var infoPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(8, 0, 8, 6), Spacing = 4 };
        infoPanel.Children.Add(new TextBlock
        {
            Text = FormatFileSize(file.FileSizeBytes),
            FontSize = 9.5,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 148, 163, 184))
        });
        infoPanel.Children.Add(new TextBlock
        {
            Text = "·",
            FontSize = 9.5,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 148, 163, 184))
        });
        infoPanel.Children.Add(new TextBlock
        {
            Text = FormatDate(file.UploadedAt),
            FontSize = 9.5,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 148, 163, 184))
        });
        Grid.SetRow(infoPanel, 2);

        mainGrid.Children.Add(iconArea);
        mainGrid.Children.Add(extBadge);
        mainGrid.Children.Add(nameBlock);
        mainGrid.Children.Add(infoPanel);

        card.Child = mainGrid;

        card.PointerPressed += (s, e) =>
        {
            _selectedFile = file;
            ShowPreview(file);
            e.Handled = true;
        };

        card.PointerEntered += (s, e) =>
        {
            card.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 8, 60, 100));
            card.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 248, 250, 255));
        };
        card.PointerExited += (s, e) =>
        {
            card.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 226, 232, 240));
            card.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
        };

        return card;
    }

    private Border CreateFileListRow(FileItem file)
    {
        var iconInfo = GetFileTypeIcon(file.FileExtension);

        var row = new Border
        {
            Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255)),
            BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 241, 245, 249)),
            BorderThickness = new Thickness(0, 0, 0, 1),
            Padding = new Thickness(14, 8, 14, 8),
            Tag = file.Id
        };

        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(38) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(140) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(160) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });

        // Icon
        var icon = new TextBlock { Text = iconInfo.icon, FontSize = 18, VerticalAlignment = VerticalAlignment.Center };
        Grid.SetColumn(icon, 0);

        // Name
        var name = new TextBlock
        {
            Text = file.FileName, FontSize = 11.5, FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 30, 41, 59)),
            VerticalAlignment = VerticalAlignment.Center, TextTrimming = TextTrimming.CharacterEllipsis
        };
        Grid.SetColumn(name, 1);

        // Type
        var type = new TextBlock
        {
            Text = file.FileExtension.TrimStart('.').ToUpperInvariant(), FontSize = 10.5,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 100, 116, 139)),
            VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(type, 2);

        // Size
        var size = new TextBlock
        {
            Text = FormatFileSize(file.FileSizeBytes), FontSize = 10.5,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 100, 116, 139)),
            VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(size, 3);

        // Category
        var cat = new TextBlock
        {
            Text = FormatCategory(file.Category), FontSize = 10.5,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 100, 116, 139)),
            VerticalAlignment = VerticalAlignment.Center
        };
        Grid.SetColumn(cat, 4);

        // Uploaded
        var uploaded = new TextBlock
        {
            Text = $"{FormatDate(file.UploadedAt)} by {file.UploadedBy}", FontSize = 10,
            Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 148, 163, 184)),
            VerticalAlignment = VerticalAlignment.Center, TextTrimming = TextTrimming.CharacterEllipsis
        };
        Grid.SetColumn(uploaded, 5);

        // Actions button
        var actionsBtn = new Button
        {
            Content = "👁 View",
            Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 241, 245, 249)),
            BorderThickness = new Thickness(0),
            Padding = new Thickness(8, 3, 8, 3),
            MinWidth = 0, MinHeight = 0, FontSize = 10.5,
            CornerRadius = new CornerRadius(3),
            VerticalAlignment = VerticalAlignment.Center
        };
        actionsBtn.Click += (s, e) =>
        {
            _selectedFile = file;
            ShowPreview(file);
        };
        Grid.SetColumn(actionsBtn, 6);

        grid.Children.Add(icon);
        grid.Children.Add(name);
        grid.Children.Add(type);
        grid.Children.Add(size);
        grid.Children.Add(cat);
        grid.Children.Add(uploaded);
        grid.Children.Add(actionsBtn);

        row.Child = grid;

        row.PointerPressed += (s, e) =>
        {
            _selectedFile = file;
            ShowPreview(file);
            e.Handled = true;
        };

        row.PointerEntered += (s, e) =>
        {
            row.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 248, 250, 255));
        };
        row.PointerExited += (s, e) =>
        {
            row.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
        };

        return row;
    }

    #region Preview Panel

    private void ShowPreview(FileItem file)
    {
        if (PreviewPanel == null || PreviewColumn == null) return;

        PreviewColumn.Width = new GridLength(330);
        PreviewPanel.Visibility = Visibility.Visible;

        if (PreviewFileName != null) PreviewFileName.Text = file.FileName;
        if (PreviewIcon != null) PreviewIcon.Text = GetFileTypeIcon(file.FileExtension).icon;
        if (PreviewFileType != null) PreviewFileType.Text = $"{file.FileExtension.TrimStart('.').ToUpperInvariant()} DOCUMENT";
        if (PreviewCategoryText != null) PreviewCategoryText.Text = FormatCategory(file.Category);
        if (PreviewFileSize != null) PreviewFileSize.Text = FormatFileSize(file.FileSizeBytes);
        if (PreviewUploadedBy != null) PreviewUploadedBy.Text = file.UploadedBy;
        if (PreviewTimestamp != null) PreviewTimestamp.Text = file.UploadedAt.ToLocalTime().ToString("MMM dd, yyyy hh:mm tt");
        if (PreviewDescription != null) PreviewDescription.Text = string.IsNullOrEmpty(file.Description) ? "No additional notes provided." : file.Description;
    }

    private void OnClosePreviewClicked(object sender, RoutedEventArgs e)
    {
        if (PreviewPanel != null) PreviewPanel.Visibility = Visibility.Collapsed;
        if (PreviewColumn != null) PreviewColumn.Width = new GridLength(0);
        _selectedFile = null;
    }

    #endregion

    #region Category Filters

    private void SetActiveFilter(Button activeBtn, string? category)
    {
        var allButtons = new[] { FilterAll, FilterMedicalReport, FilterLabResult, FilterImaging,
                                  FilterPrescription, FilterInsurance, FilterConsent, FilterReferral,
                                  FilterSurgical, FilterOther };

        foreach (var btn in allButtons)
        {
            if (btn != null)
            {
                btn.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
            }
        }

        if (activeBtn != null)
        {
            activeBtn.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 232, 240, 254));
        }

        if (FilterAllText != null)
        {
            FilterAllText.Foreground = new SolidColorBrush(
                category == null
                    ? Windows.UI.Color.FromArgb(255, 26, 115, 232)
                    : Windows.UI.Color.FromArgb(255, 74, 85, 104));
            FilterAllText.FontWeight = category == null ? Microsoft.UI.Text.FontWeights.SemiBold : Microsoft.UI.Text.FontWeights.Normal;
        }

        _activeCategory = category;
        ApplyFilters();
    }

    private void OnFilterAllClicked(object sender, RoutedEventArgs e) => SetActiveFilter(FilterAll, null);
    private void OnFilterMedicalReportClicked(object sender, RoutedEventArgs e) => SetActiveFilter(FilterMedicalReport, "MedicalReport");
    private void OnFilterLabResultClicked(object sender, RoutedEventArgs e) => SetActiveFilter(FilterLabResult, "LabResult");
    private void OnFilterImagingClicked(object sender, RoutedEventArgs e) => SetActiveFilter(FilterImaging, "ImagingStudy");
    private void OnFilterPrescriptionClicked(object sender, RoutedEventArgs e) => SetActiveFilter(FilterPrescription, "Prescription");
    private void OnFilterInsuranceClicked(object sender, RoutedEventArgs e) => SetActiveFilter(FilterInsurance, "InsuranceDocument");
    private void OnFilterConsentClicked(object sender, RoutedEventArgs e) => SetActiveFilter(FilterConsent, "ConsentForm");
    private void OnFilterReferralClicked(object sender, RoutedEventArgs e) => SetActiveFilter(FilterReferral, "Referral");
    private void OnFilterSurgicalClicked(object sender, RoutedEventArgs e) => SetActiveFilter(FilterSurgical, "SurgicalNote");
    private void OnFilterOtherClicked(object sender, RoutedEventArgs e) => SetActiveFilter(FilterOther, "Other");

    #endregion

    #region Search, Sort, View Toggle

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        _searchTerm = SearchBox?.Text;
        ApplyFilters();
    }

    private void OnSortChanged(object sender, SelectionChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void OnGridViewClicked(object sender, RoutedEventArgs e)
    {
        _isGridView = true;
        if (GridViewBtn != null)
        {
            GridViewBtn.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 232, 240, 254));
            if (GridViewBtn.Content is TextBlock tb) tb.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 26, 115, 232));
        }
        if (ListViewBtn != null)
        {
            ListViewBtn.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
            if (ListViewBtn.Content is TextBlock tb) tb.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 107, 114, 128));
        }
        ApplyFilters();
    }

    private void OnListViewClicked(object sender, RoutedEventArgs e)
    {
        _isGridView = false;
        if (GridViewBtn != null)
        {
            GridViewBtn.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
            if (GridViewBtn.Content is TextBlock tb) tb.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 107, 114, 128));
        }
        if (ListViewBtn != null)
        {
            ListViewBtn.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 232, 240, 254));
            if (ListViewBtn.Content is TextBlock tb) tb.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 26, 115, 232));
        }
        ApplyFilters();
    }

    #endregion

    #region Actions: Upload, Download, View, Delete, Back

    private async void OnUploadClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            
            // Allow all medical report extensions
            picker.FileTypeFilter.Add("*");

            // Retrieve HWND for WinUI 3 Window
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Current);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var props = await file.GetBasicPropertiesAsync();
                var ext = file.FileType.ToLowerInvariant();
                var cat = DetectCategory(ext);

                var newItem = new FileItem
                {
                    FileName = file.Name,
                    FileExtension = ext,
                    ContentType = DetectMimeType(ext),
                    FileSizeBytes = (long)props.Size,
                    Category = cat,
                    UploadedBy = "Current User (Dr. Louis P. Krenn, MD)",
                    UploadedAt = DateTime.UtcNow,
                    Description = $"Uploaded via AxioVital Document Profiler ({file.Path})",
                    LocalPath = file.Path
                };

                _allFiles.Insert(0, newItem);
                ApplyFilters();
                ShowPreview(newItem);
            }
        }
        catch
        {
            // Fallback simulated upload if picker window handle isn't attached
            var ext = ".pdf";
            var newItem = new FileItem
            {
                FileName = $"Clinical_Upload_{DateTime.Now:yyyyMMdd_HHmm}.pdf",
                FileExtension = ext,
                ContentType = "application/pdf",
                FileSizeBytes = 1_450_000,
                Category = "MedicalReport",
                UploadedBy = "Dr. Louis P. Krenn, MD",
                UploadedAt = DateTime.UtcNow,
                Description = "New clinical attachment uploaded to patient chart"
            };

            _allFiles.Insert(0, newItem);
            ApplyFilters();
            ShowPreview(newItem);
        }
    }

    private void OnDownloadClicked(object sender, RoutedEventArgs e)
    {
        if (_selectedFile != null)
        {
            // Download / Export notification
        }
    }

    private void OnViewInViewerClicked(object sender, RoutedEventArgs e)
    {
        if (_selectedFile != null && !string.IsNullOrEmpty(_selectedFile.LocalPath))
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = _selectedFile.LocalPath,
                    UseShellExecute = true
                });
            }
            catch { }
        }
    }

    private void OnDeleteClicked(object sender, RoutedEventArgs e)
    {
        if (_selectedFile != null)
        {
            _allFiles.Remove(_selectedFile);
            _selectedFile = null;
            OnClosePreviewClicked(sender, e);
            ApplyFilters();
        }
    }

    private void OnBackToChartClicked(object sender, RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Helpers

    private static string DetectCategory(string ext) => ext switch
    {
        ".pdf" or ".doc" or ".docx" or ".txt" or ".rtf" or ".odt" => "MedicalReport",
        ".xls" or ".xlsx" or ".csv" or ".ods" => "LabResult",
        ".dcm" or ".dicom" or ".nii" or ".jpg" or ".jpeg" or ".png" or ".tiff" or ".tif" or ".bmp" => "ImagingStudy",
        ".hl7" or ".json" or ".xml" => "Other",
        _ => "Other"
    };

    private static string DetectMimeType(string ext) => ext switch
    {
        ".pdf" => "application/pdf",
        ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        ".doc" => "application/msword",
        ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        ".xls" => "application/vnd.ms-excel",
        ".csv" => "text/csv",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".dcm" or ".dicom" => "application/dicom",
        ".json" => "application/json",
        ".xml" => "application/xml",
        ".zip" => "application/zip",
        _ => "application/octet-stream"
    };

    private static (string icon, Windows.UI.Color bgColor, Windows.UI.Color badgeColor) GetFileTypeIcon(string ext)
    {
        return ext.ToLowerInvariant() switch
        {
            ".pdf" => ("📕", Windows.UI.Color.FromArgb(255, 254, 226, 226), Windows.UI.Color.FromArgb(255, 220, 38, 38)),
            ".doc" or ".docx" => ("📘", Windows.UI.Color.FromArgb(255, 219, 234, 254), Windows.UI.Color.FromArgb(255, 37, 99, 235)),
            ".txt" or ".rtf" or ".odt" => ("📝", Windows.UI.Color.FromArgb(255, 243, 244, 246), Windows.UI.Color.FromArgb(255, 107, 114, 128)),
            ".xls" or ".xlsx" or ".csv" or ".ods" => ("📊", Windows.UI.Color.FromArgb(255, 209, 250, 229), Windows.UI.Color.FromArgb(255, 22, 163, 74)),
            ".jpg" or ".jpeg" or ".png" or ".bmp" => ("🖼️", Windows.UI.Color.FromArgb(255, 254, 243, 199), Windows.UI.Color.FromArgb(255, 217, 119, 6)),
            ".tiff" or ".tif" => ("📷", Windows.UI.Color.FromArgb(255, 254, 243, 199), Windows.UI.Color.FromArgb(255, 180, 83, 9)),
            ".dcm" or ".dicom" => ("🩻", Windows.UI.Color.FromArgb(255, 204, 251, 241), Windows.UI.Color.FromArgb(255, 13, 148, 136)),
            ".nii" => ("🧠", Windows.UI.Color.FromArgb(255, 237, 233, 254), Windows.UI.Color.FromArgb(255, 124, 58, 237)),
            ".xml" or ".json" or ".hl7" or ".cda" => ("🔗", Windows.UI.Color.FromArgb(255, 237, 233, 254), Windows.UI.Color.FromArgb(255, 109, 40, 217)),
            ".zip" or ".rar" or ".7z" => ("📦", Windows.UI.Color.FromArgb(255, 255, 237, 213), Windows.UI.Color.FromArgb(255, 234, 88, 12)),
            _ => ("📄", Windows.UI.Color.FromArgb(255, 243, 244, 246), Windows.UI.Color.FromArgb(255, 107, 114, 128))
        };
    }

    private static string FormatFileSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        if (bytes < 1024L * 1024 * 1024) return $"{bytes / (1024.0 * 1024):F1} MB";
        return $"{bytes / (1024.0 * 1024 * 1024):F1} GB";
    }

    private static string FormatDate(DateTime utcDate)
    {
        var local = utcDate.ToLocalTime();
        var diff = DateTime.Now - local;

        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes}m ago";
        if (diff.TotalHours < 24) return $"{(int)diff.TotalHours}h ago";
        if (diff.TotalDays < 7) return $"{(int)diff.TotalDays}d ago";
        return local.ToString("MMM d, yyyy");
    }

    private static string FormatCategory(string category) => category switch
    {
        "MedicalReport" => "Medical Report",
        "LabResult" => "Lab Result",
        "ImagingStudy" => "Imaging Study (DICOM)",
        "InsuranceDocument" => "Insurance & Claims",
        "ConsentForm" => "Consent Form",
        "SurgicalNote" => "Surgical Note",
        "Prescription" => "Prescription",
        "Referral" => "Referral",
        _ => category
    };

    #endregion
}
