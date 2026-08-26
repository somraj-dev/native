using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.ObjectModel;

namespace AxioVital.Desktop.Views;

/// <summary>
/// 1:1 Replica of AxioVital Developer Panel / Component Manager with expandable/collapsible sidebar menu.
/// </summary>
public sealed partial class DeveloperPanelView : UserControl
{
    public event EventHandler? ReturnToAxioVitalRequested;

    public DeveloperPanelView()
    {
        this.InitializeComponent();
    }

    private void OnReturnToAxioVitalClicked(object sender, RoutedEventArgs e)
    {
        ReturnToAxioVitalRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnReturnToAxioVitalPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ReturnToAxioVitalRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnTreeTogglePointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.Tag is string tag)
        {
            ToggleTreeSection(tag);
        }
    }

    private void ToggleTreeSection(string sectionTag)
    {
        switch (sectionTag)
        {
            case "admin":
                if (AdminChildrenPanel != null && AdminToggleGlyph != null)
                {
                    bool isVisible = AdminChildrenPanel.Visibility == Visibility.Visible;
                    AdminChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    AdminToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "logfiles":
                if (LogFilesChildrenPanel != null && LogFilesToggleGlyph != null)
                {
                    bool isVisible = LogFilesChildrenPanel.Visibility == Visibility.Visible;
                    LogFilesChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    LogFilesToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "mycontent":
                if (MyContentChildrenPanel != null && MyContentToggleGlyph != null)
                {
                    bool isVisible = MyContentChildrenPanel.Visibility == Visibility.Visible;
                    MyContentChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    MyContentToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "browse":
                if (BrowseChildrenPanel != null && BrowseToggleGlyph != null)
                {
                    bool isVisible = BrowseChildrenPanel.Visibility == Visibility.Visible;
                    BrowseChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    BrowseToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "search":
                if (SearchChildrenPanel != null && SearchToggleGlyph != null)
                {
                    bool isVisible = SearchChildrenPanel.Visibility == Visibility.Visible;
                    SearchChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    SearchToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "contentmgmt":
                if (ContentMgmtChildrenPanel != null && ContentMgmtToggleGlyph != null)
                {
                    bool isVisible = ContentMgmtChildrenPanel.Visibility == Visibility.Visible;
                    ContentMgmtChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    ContentMgmtToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "refinery":
                if (RefineryChildrenPanel != null && RefineryToggleGlyph != null)
                {
                    bool isVisible = RefineryChildrenPanel.Visibility == Visibility.Visible;
                    RefineryChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    RefineryToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "scheduledjobs":
                if (ScheduledJobsChildrenPanel != null && ScheduledJobsToggleGlyph != null)
                {
                    bool isVisible = ScheduledJobsChildrenPanel.Visibility == Visibility.Visible;
                    ScheduledJobsChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    ScheduledJobsToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "adminserver":
                if (AdminServerChildrenPanel != null && AdminServerToggleGlyph != null)
                {
                    bool isVisible = AdminServerChildrenPanel.Visibility == Visibility.Visible;
                    AdminServerChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    AdminServerToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "frameworkfolders":
                if (FrameworkFoldersChildrenPanel != null && FrameworkFoldersToggleGlyph != null)
                {
                    bool isVisible = FrameworkFoldersChildrenPanel.Visibility == Visibility.Visible;
                    FrameworkFoldersChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    FrameworkFoldersToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "imagingmigration":
                if (ImagingMigrationChildrenPanel != null && ImagingMigrationToggleGlyph != null)
                {
                    bool isVisible = ImagingMigrationChildrenPanel.Visibility == Visibility.Visible;
                    ImagingMigrationChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    ImagingMigrationToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "foldersretention":
                if (FoldersRetentionChildrenPanel != null && FoldersRetentionToggleGlyph != null)
                {
                    bool isVisible = FoldersRetentionChildrenPanel.Visibility == Visibility.Visible;
                    FoldersRetentionChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    FoldersRetentionToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "smartcontent":
                if (SmartContentChildrenPanel != null && SmartContentToggleGlyph != null)
                {
                    bool isVisible = SmartContentChildrenPanel.Visibility == Visibility.Visible;
                    SmartContentChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    SmartContentToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
            case "sitestudio":
                if (SiteStudioChildrenPanel != null && SiteStudioToggleGlyph != null)
                {
                    bool isVisible = SiteStudioChildrenPanel.Visibility == Visibility.Visible;
                    SiteStudioChildrenPanel.Visibility = isVisible ? Visibility.Collapsed : Visibility.Visible;
                    SiteStudioToggleGlyph.Text = isVisible ? "+" : "-";
                }
                break;
        }
    }

    private void OnMenuItemSelected(object sender, PointerRoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.Tag is string tag)
        {
            SelectMenuItem(tag);
        }
    }

    private void SelectMenuItem(string menuTag)
    {
        // Reset highlights
        if (ComponentManagerBorder != null) ComponentManagerBorder.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
        if (GeneralConfigBorder != null) GeneralConfigBorder.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
        if (ContentSecurityBorder != null) ContentSecurityBorder.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
        if (InternetConfigBorder != null) InternetConfigBorder.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);

        var highlightBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 220, 232, 244));

        switch (menuTag)
        {
            case "component_manager":
                if (ComponentManagerBorder != null) ComponentManagerBorder.Background = highlightBrush;
                if (CardTitleText != null) CardTitleText.Text = "Component Manager - Core Application Settings";
                break;
            case "general_config":
                if (GeneralConfigBorder != null) GeneralConfigBorder.Background = highlightBrush;
                if (CardTitleText != null) CardTitleText.Text = "General Configuration - Application Parameters";
                break;
            case "content_security":
                if (ContentSecurityBorder != null) ContentSecurityBorder.Background = highlightBrush;
                if (CardTitleText != null) CardTitleText.Text = "Content Security - Access Control & Roles";
                break;
            case "internet_config":
                if (InternetConfigBorder != null) InternetConfigBorder.Background = highlightBrush;
                if (CardTitleText != null) CardTitleText.Text = "Internet Configuration - Network & HTTP Settings";
                break;
        }
    }

    private void OnRefreshSettingsClicked(object sender, RoutedEventArgs e)
    {
        // Refresh settings logic
    }

    private void OnSaveChangesClicked(object sender, RoutedEventArgs e)
    {
        // Save changes notification or confirmation
    }
}
