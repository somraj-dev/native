using AxioVital.Desktop.Models;
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
/// Main application view containing patient profile, clinical Message Center, dynamic Chrome-like multi-tab navigation, and workspace views.
/// </summary>
public partial class MainPage : Page
{
    public ObservableCollection<MainTabModel> OpenTabs { get; set; } = new();

    public MainPage()
    {
        this.InitializeComponent();
        this.DataContext = this;

        // Hook up patient selection from patient list
        if (PatientListViewControl != null)
        {
            PatientListViewControl.PatientSelected += (s, e) => ShowPatientProfileByName(e.PatientName);
        }

        // Hook up Patient Details F9 Popup events
        if (PatientDetailsPopupControl != null)
        {
            PatientDetailsPopupControl.CloseRequested += (s, e) => ClosePatientDetailsPopup();
        }

        // Open Patient Profile view directly on launch
        ShowPatientProfileByName("JOHN DOE");

        // Initialize Live Clock in Permanent Footer
        UpdateFooterDateTime();
        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (s, e) => UpdateFooterDateTime();
        timer.Start();

        this.Loaded += (s, e) =>
        {
            SearchBox?.Focus(FocusState.Programmatic);
        };
    }

    private void UpdateFooterDateTime()
    {
        if (FooterDateTimeText != null)
        {
            FooterDateTimeText.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        }
    }

    public void OpenOrActivateTab(string id, string title, string headerTitle, UIElement? targetView, bool isCloseable = true)
    {
        var existing = OpenTabs.FirstOrDefault(t => t.Id == id);
        if (existing == null)
        {
            existing = new MainTabModel
            {
                Id = id,
                Title = title,
                HeaderTitle = headerTitle,
                IsActive = true,
                IsCloseable = isCloseable
            };
            OpenTabs.Add(existing);
        }

        foreach (var tab in OpenTabs)
        {
            tab.IsActive = (tab.Id == id);
        }

        if (HeaderTitleText != null)
        {
            HeaderTitleText.Text = headerTitle;
        }

        if (PatientDemographicBanner != null)
        {
            PatientDemographicBanner.Visibility = (id == "patient_profile") ? Visibility.Visible : Visibility.Collapsed;
        }

        if (PatientProfileViewControl != null) PatientProfileViewControl.Visibility = (targetView == PatientProfileViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (PatientListViewControl != null) PatientListViewControl.Visibility = (targetView == PatientListViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (MessageCenterViewControl != null) MessageCenterViewControl.Visibility = (targetView == MessageCenterViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (LabsViewControl != null) LabsViewControl.Visibility = (targetView == LabsViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (SchedulerViewControl != null) SchedulerViewControl.Visibility = (targetView == SchedulerViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (OngoingActivitiesViewControl != null) OngoingActivitiesViewControl.Visibility = (targetView == OngoingActivitiesViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (CustomisedViewControl != null) CustomisedViewControl.Visibility = (targetView == CustomisedViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (QualityMeasuresViewControl != null) QualityMeasuresViewControl.Visibility = (targetView == QualityMeasuresViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (PhysicianHandoffViewControl != null) PhysicianHandoffViewControl.Visibility = (targetView == PhysicianHandoffViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (UpToDateViewControl != null) UpToDateViewControl.Visibility = (targetView == UpToDateViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (AnalyticsViewControl != null) AnalyticsViewControl.Visibility = (targetView == AnalyticsViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (GrowthChartViewControl != null) GrowthChartViewControl.Visibility = (targetView == GrowthChartViewControl) ? Visibility.Visible : Visibility.Collapsed;
        if (HistoriesChartViewControl != null) HistoriesChartViewControl.Visibility = (targetView == HistoriesChartViewControl) ? Visibility.Visible : Visibility.Collapsed;

        if (targetView is FrameworkElement fe)
        {
            AnimateViewEntrance(fe);
        }
    }

    private void AnimateViewEntrance(FrameworkElement element)
    {
        try
        {
            var storyboard = new Microsoft.UI.Xaml.Media.Animation.Storyboard();

            var opacityAnim = new Microsoft.UI.Xaml.Media.Animation.DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromMilliseconds(180)),
                EasingFunction = new Microsoft.UI.Xaml.Media.Animation.CubicEase { EasingMode = Microsoft.UI.Xaml.Media.Animation.EasingMode.EaseOut }
            };
            Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTarget(opacityAnim, element);
            Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTargetProperty(opacityAnim, "Opacity");
            storyboard.Children.Add(opacityAnim);

            var translate = new TranslateTransform();
            element.RenderTransform = translate;
            var translateAnim = new Microsoft.UI.Xaml.Media.Animation.DoubleAnimation
            {
                From = 6,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(180)),
                EasingFunction = new Microsoft.UI.Xaml.Media.Animation.CubicEase { EasingMode = Microsoft.UI.Xaml.Media.Animation.EasingMode.EaseOut }
            };
            Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTarget(translateAnim, translate);
            Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTargetProperty(translateAnim, "Y");
            storyboard.Children.Add(translateAnim);

            storyboard.Begin();
        }
        catch { }
    }

    private void OnTabStripItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is MainTabModel tab)
        {
            SwitchToTab(tab.Id);
        }
    }

    private void SwitchToTab(string tabId)
    {
        switch (tabId)
        {
            case "patient_list":
                ShowPatientListView();
                break;
            case "patient_profile":
                ShowPatientProfileView();
                break;
            case "message_center":
                ShowMessageCenterView();
                break;
            case "labs":
                ShowLabsView();
                break;
            case "orders":
                ShowOrdersView();
                break;
            case "scheduler":
                ShowSchedulerView();
                break;
            case "ongoing_activities":
                ShowOngoingActivitiesView();
                break;
            case "customised":
                ShowCustomisedView();
                break;
            case "quality_measures":
                ShowQualityMeasuresView();
                break;
            case "physician_handoff":
                ShowPhysicianHandoffView();
                break;
            case "uptodate":
                ShowUpToDateView();
                break;
            case "analytics":
                ShowAnalyticsView();
                break;
            case "care_workflow":
                ShowCareWorkflowView();
                break;
            case "care_pathways":
                ShowCarePathwaysView();
                break;
            case "dashboard":
                ShowDashboardView();
                break;
            case "reports":
                ShowReportsView();
                break;
            case "home":
                ShowHomeView();
                break;
        }
    }

    private void OnTabCloseButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element && element.Tag is string tabId)
        {
            CloseTab(tabId);
        }
    }

    private void OnTabClosePointerPressed(object sender, PointerRoutedEventArgs e)
    {
        e.Handled = true;
        if (sender is FrameworkElement element && element.Tag is string tabId)
        {
            CloseTab(tabId);
        }
    }

    public void CloseTab(string tabId)
    {
        var tabToClose = OpenTabs.FirstOrDefault(t => t.Id == tabId);
        if (tabToClose == null) return;

        int index = OpenTabs.IndexOf(tabToClose);
        bool wasActive = tabToClose.IsActive;

        OpenTabs.Remove(tabToClose);

        if (wasActive && OpenTabs.Count > 0)
        {
            int nextIndex = Math.Min(index, OpenTabs.Count - 1);
            SwitchToTab(OpenTabs[nextIndex].Id);
        }
    }

    private void OnHomeTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowHomeView();
    }

    private void OnMessageCenterTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowMessageCenterView();
    }

    private void OnPatientListTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowPatientListView();
    }

    private void OnPhysicianHandoffTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowPhysicianHandoffView();
    }

    private void OnCareWorkflowTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowCareWorkflowView();
    }

    private void OnQualityMeasuresTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowQualityMeasuresView();
    }

    private void OnCustomisedTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowCustomisedView();
    }

    private void OnReportsTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowReportsView();
    }

    private void OnUpToDateTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowUpToDateView();
    }

    private void OnOngoingActivitiesTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowOngoingActivitiesView();
    }

    private void OnDashboardTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowDashboardView();
    }

    private void OnSchedulerTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowSchedulerView();
    }

    private void OnOrderSetsTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowOrdersView();
    }

    private void OnCarePathwaysTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowCarePathwaysView();
    }

    private void OnLabsTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowLabsView();
    }

    private void OnAnalyticsTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowAnalyticsView();
    }

    private void ResetAllRibbonTabHighlights()
    {
        var neutralColor = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 51, 51));
        var catNeutralColor = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 68, 68, 68));

        // Level 2 SubNavBar
        ResetTabItem(MessageCenterTabBorder, MessageCenterTabText, neutralColor);
        ResetTabItem(PatientListTabBorder, PatientListTabText, neutralColor);
        ResetTabItem(PhysicianHandoffTabBorder, PhysicianHandoffTabText, neutralColor);
        ResetTabItem(CareWorkflowTabBorder, CareWorkflowTabText, neutralColor);
        ResetTabItem(QualityMeasuresTabBorder, QualityMeasuresTabText, neutralColor);
        ResetTabItem(CustomisedTabBorder, CustomisedTabText, neutralColor);
        ResetTabItem(ReportsTabBorder, ReportsTabText, neutralColor);
        ResetTabItem(UpToDateTabBorder, UpToDateTabText, neutralColor);
        ResetTabItem(OngoingActivitiesTabBorder, OngoingActivitiesTabText, neutralColor);

        // Level 3 CategoryBar
        ResetTabItem(DashboardCategoryTabBorder, DashboardCategoryTabText, catNeutralColor);
        ResetTabItem(SchedulerCategoryTabBorder, SchedulerCategoryTabText, catNeutralColor);
        ResetTabItem(OrderSetsCategoryTabBorder, OrderSetsCategoryTabText, catNeutralColor);
        ResetTabItem(CarePathwaysCategoryTabBorder, CarePathwaysCategoryTabText, catNeutralColor);
        ResetTabItem(LabsCategoryTabBorder, LabsCategoryTabText, catNeutralColor);
        ResetTabItem(AnalyticsCategoryTabBorder, AnalyticsCategoryTabText, catNeutralColor);
    }

    private void ResetTabItem(Border? tabBorder, TextBlock? tabText, SolidColorBrush defaultColor)
    {
        if (tabBorder != null)
        {
            tabBorder.BorderBrush = null;
            tabBorder.BorderThickness = new Thickness(0);
        }
        if (tabText != null)
        {
            tabText.Foreground = defaultColor;
            tabText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        }
    }

    private void HighlightRibbonTabWithAnimation(Border? tabBorder, TextBlock? tabText)
    {
        ResetAllRibbonTabHighlights();

        if (tabBorder != null)
        {
            tabBorder.BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
            tabBorder.BorderThickness = new Thickness(0, 0, 0, 2.5);

            try
            {
                var storyboard = new Microsoft.UI.Xaml.Media.Animation.Storyboard();
                var anim = new Microsoft.UI.Xaml.Media.Animation.DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                    EasingFunction = new Microsoft.UI.Xaml.Media.Animation.CubicEase { EasingMode = Microsoft.UI.Xaml.Media.Animation.EasingMode.EaseOut }
                };
                Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTarget(anim, tabBorder);
                Microsoft.UI.Xaml.Media.Animation.Storyboard.SetTargetProperty(anim, "Opacity");
                storyboard.Children.Add(anim);
                storyboard.Begin();
            }
            catch { }
        }

        if (tabText != null)
        {
            tabText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
            tabText.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
        }
    }

    public void ShowPatientListView()
    {
        HighlightRibbonTabWithAnimation(PatientListTabBorder, PatientListTabText);
        OpenOrActivateTab("patient_list", "Patient List", "Patient List", PatientListViewControl);
    }

    public void ShowPatientProfileView()
    {
        ShowPatientProfileByName(_currentSelectedPatientName);
    }

    public void ShowHomeView()
    {
        OpenOrActivateTab("home", "Home", "Home", PatientProfileViewControl);
    }

    public void ShowMessageCenterView()
    {
        HighlightRibbonTabWithAnimation(MessageCenterTabBorder, MessageCenterTabText);
        OpenOrActivateTab("message_center", "General Messages: JOHN DOE", "Message Center", MessageCenterViewControl);
    }

    public void ShowSchedulerView()
    {
        HighlightRibbonTabWithAnimation(SchedulerCategoryTabBorder, SchedulerCategoryTabText);
        OpenOrActivateTab("scheduler", "Appointment Reschedule Requests", "Appointment Reschedule Requests", SchedulerViewControl);
    }

    public void ShowOrdersView()
    {
        HighlightRibbonTabWithAnimation(OrderSetsCategoryTabBorder, OrderSetsCategoryTabText);
        OpenOrActivateTab("orders", "Orders", "Orders", LabsViewControl);
    }

    public void ShowLabsView()
    {
        HighlightRibbonTabWithAnimation(LabsCategoryTabBorder, LabsCategoryTabText);
        OpenOrActivateTab("labs", "Labs", "Labs", LabsViewControl);
    }

    public void ShowOngoingActivitiesView()
    {
        HighlightRibbonTabWithAnimation(OngoingActivitiesTabBorder, OngoingActivitiesTabText);
        OpenOrActivateTab("ongoing_activities", "Ongoing Activities", "Ongoing Activities", OngoingActivitiesViewControl);
    }

    public void ShowCustomisedView()
    {
        HighlightRibbonTabWithAnimation(CustomisedTabBorder, CustomisedTabText);
        OpenOrActivateTab("customised", "Customised Organizer", "Customised Organizer", CustomisedViewControl);
    }

    public void ShowQualityMeasuresView()
    {
        HighlightRibbonTabWithAnimation(QualityMeasuresTabBorder, QualityMeasuresTabText);
        OpenOrActivateTab("quality_measures", "Quality Measures", "Quality Measures", QualityMeasuresViewControl);
    }

    public void ShowPhysicianHandoffView()
    {
        HighlightRibbonTabWithAnimation(PhysicianHandoffTabBorder, PhysicianHandoffTabText);
        OpenOrActivateTab("physician_handoff", "Physician Handoff", "Physician Handoff", PhysicianHandoffViewControl);
    }

    public void ShowUpToDateView()
    {
        HighlightRibbonTabWithAnimation(UpToDateTabBorder, UpToDateTabText);
        OpenOrActivateTab("uptodate", "UpToDate", "UpToDate", UpToDateViewControl);
    }

    public void ShowAnalyticsView()
    {
        HighlightRibbonTabWithAnimation(AnalyticsCategoryTabBorder, AnalyticsCategoryTabText);
        OpenOrActivateTab("analytics", "Analytics", "Analytics", AnalyticsViewControl);
    }

    public void ShowCareWorkflowView()
    {
        HighlightRibbonTabWithAnimation(CareWorkflowTabBorder, CareWorkflowTabText);
        OpenOrActivateTab("care_workflow", "Care Workflow", "Care Workflow", PatientProfileViewControl);
    }

    public void ShowReportsView()
    {
        HighlightRibbonTabWithAnimation(ReportsTabBorder, ReportsTabText);
        OpenOrActivateTab("reports", "Clinical Reports", "Reports", UpToDateViewControl);
    }

    public void ShowDashboardView()
    {
        HighlightRibbonTabWithAnimation(DashboardCategoryTabBorder, DashboardCategoryTabText);
        OpenOrActivateTab("dashboard", "Clinical Dashboard", "Dashboard", AnalyticsViewControl);
    }

    public void ShowCarePathwaysView()
    {
        HighlightRibbonTabWithAnimation(CarePathwaysCategoryTabBorder, CarePathwaysCategoryTabText);
        OpenOrActivateTab("care_pathways", "Care Pathways", "Care Pathways", QualityMeasuresViewControl);
    }

    public void ShowGrowthChartView()
    {
        OpenOrActivateTab("growth_chart", "Growth Chart", "Growth Chart", GrowthChartViewControl);
    }

    public void ShowHistoriesChartView()
    {
        OpenOrActivateTab("histories_chart", "Calendar Chart", "Historic Trends: Calendar Chart", HistoriesChartViewControl);
    }

    public bool IsPatientDetailsPopupOpen => PatientDetailsPopupOverlay != null && PatientDetailsPopupOverlay.Visibility == Visibility.Visible;

    public void OpenPatientDetailsPopup()
    {
        if (PatientDetailsPopupOverlay != null)
        {
            PatientDetailsPopupOverlay.Visibility = Visibility.Visible;
            AnimateViewEntrance(PatientDetailsPopupOverlay);
        }
    }

    public void ClosePatientDetailsPopup()
    {
        if (PatientDetailsPopupOverlay != null)
        {
            PatientDetailsPopupOverlay.Visibility = Visibility.Collapsed;
        }
    }

    public void TogglePatientDetailsPopup()
    {
        if (IsPatientDetailsPopupOpen)
        {
            ClosePatientDetailsPopup();
        }
        else
        {
            OpenPatientDetailsPopup();
        }
    }

    private void OnPatientDetailsDialogPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        e.Handled = true;
    }

    private void OnPatientDetailsPopupBackdropPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (ReferenceEquals(e.OriginalSource, PatientDetailsPopupOverlay))
        {
            ClosePatientDetailsPopup();
        }
    }

    private bool _isPageFullScreen = false;

    private void OnFullScreenPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        TogglePageFullScreen();
    }

    public void TogglePageFullScreen()
    {
        _isPageFullScreen = !_isPageFullScreen;

        var vis = _isPageFullScreen ? Visibility.Collapsed : Visibility.Visible;
        if (TopRibbonBarGrid != null) TopRibbonBarGrid.Visibility = vis;
        if (SubNavBarGrid != null) SubNavBarGrid.Visibility = vis;
        if (CategoryBarGrid != null) CategoryBarGrid.Visibility = vis;
        if (DarkHeaderBarGrid != null) DarkHeaderBarGrid.Visibility = vis;
        if (TabStripBarGrid != null) TabStripBarGrid.Visibility = vis;
    }

    public void ExitPageFullScreen()
    {
        if (_isPageFullScreen)
        {
            _isPageFullScreen = false;
            if (TopRibbonBarGrid != null) TopRibbonBarGrid.Visibility = Visibility.Visible;
            if (SubNavBarGrid != null) SubNavBarGrid.Visibility = Visibility.Visible;
            if (CategoryBarGrid != null) CategoryBarGrid.Visibility = Visibility.Visible;
            if (DarkHeaderBarGrid != null) DarkHeaderBarGrid.Visibility = Visibility.Visible;
            if (TabStripBarGrid != null) TabStripBarGrid.Visibility = Visibility.Visible;
        }
    }

    private void OnPatientActionsMenuButtonClick(object sender, RoutedEventArgs e)
    {
    }

    private void OnSearchBarBorderPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        SearchBox?.Focus(FocusState.Programmatic);
    }

    public bool IsQuickPanelOpen => QuickPanelOverlay != null && QuickPanelOverlay.Visibility == Visibility.Visible;

    public void CloseQuickPanel()
    {
        if (QuickPanelOverlay != null)
        {
            QuickPanelOverlay.Visibility = Visibility.Collapsed;
        }
    }

    public void ToggleQuickPanel()
    {
        if (QuickPanelOverlay != null)
        {
            if (QuickPanelOverlay.Visibility == Visibility.Visible)
            {
                QuickPanelOverlay.Visibility = Visibility.Collapsed;
            }
            else
            {
                QuickPanelOverlay.Visibility = Visibility.Visible;
                AnimateViewEntrance(QuickPanelOverlay);
            }
        }
    }


    private void OnQuickPanelBackdropPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (QuickPanelOverlay != null)
        {
            QuickPanelOverlay.Visibility = Visibility.Collapsed;
        }
    }

    private void OnQuickPanelDialogPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        e.Handled = true;
    }

    private void OnQuickPanelCancelClick(object sender, RoutedEventArgs e)
    {
        if (QuickPanelOverlay != null)
        {
            QuickPanelOverlay.Visibility = Visibility.Collapsed;
        }
    }

    private void OnQuickPanelOkClick(object sender, RoutedEventArgs e)
    {
        if (QuickPanelItemsGridView?.SelectedItem is GridViewItem selectedItem && selectedItem.Tag is string actionTag)
        {
            ExecuteQuickPanelAction(actionTag);
        }
        else
        {
            if (QuickPanelOverlay != null)
            {
                QuickPanelOverlay.Visibility = Visibility.Collapsed;
            }
        }
    }

    private void OnQuickPanelItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is GridViewItem item && item.Tag is string actionTag)
        {
            ExecuteQuickPanelAction(actionTag);
        }
    }

    private void ExecuteQuickPanelAction(string actionTag)
    {
        if (QuickPanelOverlay != null)
        {
            QuickPanelOverlay.Visibility = Visibility.Collapsed;
        }

        switch (actionTag)
        {
            case "facility_transfer":
                OpenOrActivateTab("facility_transfer", "Facility Transfer", "Facility Transfer", new FacilityTransferPage());
                break;
            case "add_person":
            case "view_person":
            case "view_encounter":
                ShowPatientProfileView();
                break;
            case "bed_transfer":
            case "pending_transfer":
                ShowSchedulerView();
                break;
            default:
                break;
        }
    }

    private void OnDropdownItemPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b)
        {
            b.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 238, 242, 246));
        }
    }

    private void OnDropdownItemPointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b)
        {
            b.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        }
    }

    private void OnPatientActionDropdownItemPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b && b.Tag is string actionTag)
        {
            if (PatientActionsMenuButton?.Flyout != null)
            {
                PatientActionsMenuButton.Flyout.Hide();
            }

            ExecuteQuickPanelAction(actionTag);
        }
    }

    private void OnTopRibbonHelpFlyoutOpened(object sender, object e)
    {
        if (EditorPlaygroundSubFlyout != null && EditorPlaygroundSubFlyout.IsOpen)
        {
            EditorPlaygroundSubFlyout.Hide();
        }

        if (HelpEditorPlaygroundRow != null)
        {
            HelpEditorPlaygroundRow.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        }
        if (HelpEditorPlaygroundText != null)
        {
            HelpEditorPlaygroundText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59));
        }
        if (HelpEditorPlaygroundIcon != null)
        {
            HelpEditorPlaygroundIcon.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139));
        }
    }

    private void OnHelpItemPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b)
        {
            b.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 6, 72, 117));
            if (b.Child is TextBlock tb)
            {
                tb.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
            }
            else if (b.Child is Grid g)
            {
                foreach (var child in g.Children)
                {
                    if (child is TextBlock gtb) gtb.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
                    if (child is FontIcon fi) fi.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
                }
            }

            // If entering main column item other than playground, close subflyout
            if (b.Tag is string tag && (tag == "help_welcome" || tag == "help_commands" || tag == "help_walkthrough" || 
                tag == "help_feedback" || tag == "help_diagnostics" || tag == "help_license" || tag == "help_devtools" || 
                tag == "help_process_explorer" || tag == "help_updates" || tag == "help_about"))
            {
                if (EditorPlaygroundSubFlyout != null && EditorPlaygroundSubFlyout.IsOpen)
                {
                    EditorPlaygroundSubFlyout.Hide();
                }
                if (HelpEditorPlaygroundRow != null)
                {
                    HelpEditorPlaygroundRow.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
                }
                if (HelpEditorPlaygroundText != null)
                {
                    HelpEditorPlaygroundText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59));
                }
                if (HelpEditorPlaygroundIcon != null)
                {
                    HelpEditorPlaygroundIcon.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139));
                }
            }
        }
    }

    private void OnHelpItemPointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b)
        {
            // If it's the highlighted active item "Open View...", keep its distinct state
            if (b.Tag is string tag && tag == "help_open_view")
            {
                b.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
                if (b.Child is TextBlock otb) otb.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
                return;
            }

            b.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            if (b.Child is TextBlock tb)
            {
                tb.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59));
            }
            else if (b.Child is Grid g)
            {
                if (g.Children.Count > 0 && g.Children[0] is TextBlock tb1)
                {
                    tb1.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59));
                }
                if (g.Children.Count > 1)
                {
                    if (g.Children[1] is TextBlock tb2) tb2.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139));
                    if (g.Children[1] is FontIcon fi) fi.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139));
                }
            }
        }
    }

    private void OnEditorPlaygroundPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        OpenEditorPlaygroundSubmenu();
    }

    private void OnEditorPlaygroundPointerExited(object sender, PointerRoutedEventArgs e)
    {
        // Keep active row state while submenu is visible
    }

    private void OnEditorPlaygroundMenuButtonClick(object sender, RoutedEventArgs e)
    {
        OpenEditorPlaygroundSubmenu();
    }

    private void OpenEditorPlaygroundSubmenu()
    {
        if (HelpEditorPlaygroundRow != null)
        {
            HelpEditorPlaygroundRow.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 6, 72, 117));
        }
        if (HelpEditorPlaygroundText != null)
        {
            HelpEditorPlaygroundText.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
        }
        if (HelpEditorPlaygroundIcon != null)
        {
            HelpEditorPlaygroundIcon.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
        }

        if (EditorPlaygroundSubFlyout != null && EditorPlaygroundMenuButton != null)
        {
            EditorPlaygroundSubFlyout.ShowAt(EditorPlaygroundMenuButton);
        }
    }

    private void OnHelpCustomItemPressed(object sender, PointerRoutedEventArgs e)
    {
        if (EditorPlaygroundSubFlyout != null && EditorPlaygroundSubFlyout.IsOpen)
        {
            EditorPlaygroundSubFlyout.Hide();
        }

        if (TopRibbonHelpButton?.Flyout != null)
        {
            TopRibbonHelpButton.Flyout.Hide();
        }

        if (sender is Border b && b.Tag is string tag)
        {
            switch (tag)
            {
                case "help_welcome":
                case "help_walkthrough":
                    ShowPatientProfileView();
                    break;
                case "help_commands":
                case "help_cmd_palette":
                case "help_search":
                    SearchBox?.Focus(FocusState.Programmatic);
                    break;
                case "help_open_view":
                case "help_explorer":
                    ShowPatientListView();
                    break;
                default:
                    if (PermanentFooterPatientText != null)
                    {
                        PermanentFooterPatientText.Text = $"AxioVital Help: Action executed ({tag})";
                    }
                    break;
            }
        }
    }

    private void OnTopRibbonAmbulatoryFlyoutOpened(object sender, object e)
    {
        if (TopRibbonAmbulatoryButton != null)
        {
            TopRibbonAmbulatoryButton.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 212, 232, 245));
            TopRibbonAmbulatoryButton.BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 160, 196, 232));
        }
        if (TopRibbonAmbulatoryText != null)
        {
            TopRibbonAmbulatoryText.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;
            TopRibbonAmbulatoryText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 37, 64));
        }
    }

    private void OnTopRibbonAmbulatoryFlyoutClosed(object sender, object e)
    {
        if (TopRibbonAmbulatoryButton != null)
        {
            TopRibbonAmbulatoryButton.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            TopRibbonAmbulatoryButton.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        }
        if (TopRibbonAmbulatoryText != null)
        {
            TopRibbonAmbulatoryText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
            TopRibbonAmbulatoryText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 51, 51));
        }
    }

    private void OnAmbulatoryItemPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b)
        {
            b.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 12, 74, 112));
            if (b.Child is TextBlock tb)
            {
                tb.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
            }
        }
    }

    private void OnAmbulatoryItemPointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b)
        {
            b.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            if (b.Child is TextBlock tb)
            {
                tb.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59));
            }
        }
    }

    private void OnAmbulatoryCustomItemPressed(object sender, PointerRoutedEventArgs e)
    {
        if (TopRibbonAmbulatoryButton?.Flyout != null)
        {
            TopRibbonAmbulatoryButton.Flyout.Hide();
        }

        if (sender is Border b && b.Tag is string tag)
        {
            ExecuteAmbulatoryDropdownAction(tag);
        }
    }

    private void ExecuteAmbulatoryDropdownAction(string tag)
    {
        switch (tag)
        {
            case "amb_appointment_request":
                ShowSchedulerView();
                break;
            case "amb_referrals_transfer":
                OpenOrActivateTab("facility_transfer", "Facility Transfer", "Facility Transfer", new FacilityTransferPage());
                break;
            case "amb_discharge_list":
                ShowPatientListView();
                break;
            default:
                break;
        }
    }

    private void OnTopRibbonClinicalFlyoutOpened(object sender, object e)
    {
        if (TopRibbonClinicalButton != null)
        {
            TopRibbonClinicalButton.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 212, 232, 245));
            TopRibbonClinicalButton.BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 160, 196, 232));
        }
        if (TopRibbonClinicalText != null)
        {
            TopRibbonClinicalText.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;
            TopRibbonClinicalText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 37, 64));
        }
    }

    private void OnTopRibbonClinicalFlyoutClosed(object sender, object e)
    {
        if (TopRibbonClinicalButton != null)
        {
            TopRibbonClinicalButton.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            TopRibbonClinicalButton.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        }
        if (TopRibbonClinicalText != null)
        {
            TopRibbonClinicalText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
            TopRibbonClinicalText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 51, 51));
        }
    }

    private void OnClinicalItemPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b)
        {
            b.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 12, 74, 112));
            if (b.Child is TextBlock tb)
            {
                tb.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
            }
        }
    }

    private void OnClinicalItemPointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b)
        {
            b.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
            if (b.Child is TextBlock tb)
            {
                tb.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59));
            }
        }
    }

    private void OnClinicalCustomItemPressed(object sender, PointerRoutedEventArgs e)
    {
        if (TopRibbonClinicalButton?.Flyout != null)
        {
            TopRibbonClinicalButton.Flyout.Hide();
        }

        if (sender is Border b && b.Tag is string tag)
        {
            ExecuteClinicalDropdownAction(tag);
        }
    }

    private void ExecuteClinicalDropdownAction(string tag)
    {
        switch (tag)
        {
            case "clin_provider_view":
                ShowPatientProfileView();
                break;
            case "clin_results_review":
                ShowLabsView();
                break;
            case "clin_orders":
                ShowOrdersView();
                break;
            case "clin_documentation":
                ShowPatientProfileView();
                PatientProfileViewControl?.SelectSection("documentation");
                break;
            case "clin_outside_records":
                ShowPatientProfileView();
                PatientProfileViewControl?.SelectSection("outside_records");
                break;
            case "clin_allergies":
                ShowPatientProfileView();
                PatientProfileViewControl?.SelectSection("allergies");
                break;
            case "clin_clinical_media":
                ShowPatientProfileView();
                PatientProfileViewControl?.SelectSection("clinical_media");
                break;
            case "clin_diagnoses":
                ShowPatientProfileView();
                PatientProfileViewControl?.SelectSection("diagnoses");
                break;
            case "clin_form_browser":
                ShowPatientProfileView();
                PatientProfileViewControl?.SelectSection("form_browser");
                break;
            case "clin_growth_chart":
                ShowGrowthChartView();
                break;
            case "clin_histories":
                ShowHistoriesChartView();
                break;
            case "clin_interactive_view":
                ShowPatientProfileView();
                PatientProfileViewControl?.SelectSection("interactive_view");
                break;
            case "clin_mar_summary":
                ShowPatientProfileView();
                PatientProfileViewControl?.SelectSection("mar_summary");
                break;
            case "clin_medication_list":
                ShowPatientProfileView();
                PatientProfileViewControl?.SelectSection("medication_list");
                break;
            case "clin_patient_info":
                ShowPatientProfileView();
                PatientProfileViewControl?.SelectSection("patient_info");
                break;
            case "clin_recommendations":
                ShowQualityMeasuresView();
                break;
            case "clin_smart_app_validator":
                ShowCareWorkflowView();
                break;
            case "clin_axionote_note":
            case "clin_axionote_edge":
            case "clin_axionote_enterprise":
            case "clin_workflowview_edge":
            case "clin_axionote_dev":
            case "clin_axionote_debug":
                ShowCareWorkflowView();
                break;
            case "clin_calculator":
                ShowAnalyticsView();
                break;
            case "clin_adhoc_charting":
                ShowOngoingActivitiesView();
                break;
            case "clin_view_charges":
                ShowDashboardView();
                break;
            default:
                ShowPatientProfileView();
                break;
        }

        if (PermanentFooterPatientText != null)
        {
            PermanentFooterPatientText.Text = $"AxioVital Clinical: Active Module ({tag})";
        }
    }

    public bool IsPersonSearchOpen => PersonSearchOverlay != null && PersonSearchOverlay.Visibility == Visibility.Visible;

    public void ClosePersonSearch()
    {
        if (PersonSearchOverlay != null)
        {
            PersonSearchOverlay.Visibility = Visibility.Collapsed;
        }
        if (PersonSearchMinimizedBar != null)
        {
            PersonSearchMinimizedBar.Visibility = Visibility.Collapsed;
        }
    }

    public void OpenPersonSearch()
    {
        if (PersonSearchOverlay != null)
        {
            PersonSearchOverlay.Visibility = Visibility.Visible;
            if (PersonSearchDialogContainer != null)
            {
                PersonSearchDialogContainer.Visibility = Visibility.Visible;
                AnimateViewEntrance(PersonSearchDialogContainer);
            }
            if (PersonSearchMinimizedBar != null)
            {
                PersonSearchMinimizedBar.Visibility = Visibility.Collapsed;
            }
            EnsurePersonDatabaseInitialized();
            PerformPersonSearch();
            PersonSearchLastNameBox?.Focus(FocusState.Programmatic);
        }
    }

    public void TogglePersonSearch()
    {
        if (IsPersonSearchOpen && (PersonSearchDialogContainer?.Visibility == Visibility.Visible))
        {
            ClosePersonSearch();
        }
        else
        {
            OpenPersonSearch();
        }
    }

    private bool _isPersonSearchMaximized = false;

    private void OnPersonSearchMinimizeClick(object sender, PointerRoutedEventArgs e)
    {
        if (PersonSearchDialogContainer != null && PersonSearchMinimizedBar != null)
        {
            PersonSearchDialogContainer.Visibility = Visibility.Collapsed;
            PersonSearchMinimizedBar.Visibility = Visibility.Visible;
            AnimateViewEntrance(PersonSearchMinimizedBar);
        }
    }

    private void OnPersonSearchRestoreFromMinClick(object sender, PointerRoutedEventArgs e)
    {
        if (PersonSearchDialogContainer != null && PersonSearchMinimizedBar != null)
        {
            PersonSearchMinimizedBar.Visibility = Visibility.Collapsed;
            PersonSearchDialogContainer.Visibility = Visibility.Visible;
            AnimateViewEntrance(PersonSearchDialogContainer);
        }
    }

    private void OnPersonSearchMaximizeToggleClick(object sender, PointerRoutedEventArgs e)
    {
        if (PersonSearchDialogContainer == null) return;

        _isPersonSearchMaximized = !_isPersonSearchMaximized;

        if (_isPersonSearchMaximized)
        {
            PersonSearchDialogContainer.Width = double.NaN;
            PersonSearchDialogContainer.Height = double.NaN;
            PersonSearchDialogContainer.Margin = new Thickness(14, 14, 14, 14);
            PersonSearchDialogContainer.HorizontalAlignment = HorizontalAlignment.Stretch;
            PersonSearchDialogContainer.VerticalAlignment = VerticalAlignment.Stretch;
            if (PersonSearchMaximizeIcon != null) PersonSearchMaximizeIcon.Text = "🗗";
        }
        else
        {
            PersonSearchDialogContainer.Width = 980;
            PersonSearchDialogContainer.Height = 600;
            PersonSearchDialogContainer.Margin = new Thickness(0);
            PersonSearchDialogContainer.HorizontalAlignment = HorizontalAlignment.Center;
            PersonSearchDialogContainer.VerticalAlignment = VerticalAlignment.Center;
            if (PersonSearchMaximizeIcon != null) PersonSearchMaximizeIcon.Text = "🗖";
        }
    }

    private void OnNoticeBannerExpandPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (NoticeBannerExpandedDetails != null)
        {
            if (NoticeBannerExpandedDetails.Visibility == Visibility.Visible)
            {
                NoticeBannerExpandedDetails.Visibility = Visibility.Collapsed;
                if (NoticeBannerExpandIcon != null) NoticeBannerExpandIcon.Glyph = "\uE740";
            }
            else
            {
                NoticeBannerExpandedDetails.Visibility = Visibility.Visible;
                if (NoticeBannerExpandIcon != null) NoticeBannerExpandIcon.Glyph = "\uE70E";
            }
        }
    }

    private void OnTitleBarButtonPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border border)
        {
            border.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(60, 255, 255, 255));
        }
    }

    private void OnTitleBarButtonPointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border border)
        {
            border.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        }
    }

    private void OnCloseButtonPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border border)
        {
            border.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 239, 68, 68));
        }
    }

    private void OnCloseButtonPointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border border)
        {
            border.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        }
    }

    private void OnPersonSearchBackdropPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (PersonSearchMinimizedBar?.Visibility == Visibility.Visible)
        {
            return;
        }
        ClosePersonSearch();
    }

    private void OnPersonSearchDialogPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        e.Handled = true;
    }

    private void OnPersonSearchCancelClick(object sender, RoutedEventArgs e)
    {
        ClosePersonSearch();
    }

    private void OnPersonSearchCancelClick(object sender, PointerRoutedEventArgs e)
    {
        ClosePersonSearch();
    }

    private void OnPersonSearchClearClick(object sender, RoutedEventArgs e)
    {
        if (PersonSearchLastNameBox != null) PersonSearchLastNameBox.Text = string.Empty;
        if (PersonSearchFirstNameBox != null) PersonSearchFirstNameBox.Text = string.Empty;
        if (PersonSearchDobBox != null) PersonSearchDobBox.Text = string.Empty;
        if (PersonSearchPhoneBox != null) PersonSearchPhoneBox.Text = string.Empty;
        if (PersonSearchEncounterBox != null) PersonSearchEncounterBox.Text = string.Empty;

        PerformPersonSearch();
    }

    private void OnPersonSearchButtonClick(object sender, RoutedEventArgs e)
    {
        PerformPersonSearch();
    }

    private void OnPersonSearchBoxKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            PerformPersonSearch();
            e.Handled = true;
        }
    }

    private void OnLinkBorderPointerEntered(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border border)
        {
            border.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 241, 245, 249));
        }
    }

    private void OnLinkBorderPointerExited(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border border)
        {
            border.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
        }
    }

    public class PersonSearchRecord
    {
        public string Name { get; set; } = string.Empty;
        public string Mrn { get; set; } = string.Empty;
        public string Dob { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<EncounterSearchRecord> Encounters { get; set; } = new();
    }

    public class EncounterSearchRecord
    {
        public string EncounterNumber { get; set; } = string.Empty;
        public string Facility { get; set; } = string.Empty;
        public string EncounterType { get; set; } = string.Empty;
        public string DateOfService { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string Guarantor { get; set; } = string.Empty;
        public string DischargeDate { get; set; } = string.Empty;
    }

    private List<PersonSearchRecord>? _globalPersonDatabase;
    private PersonSearchRecord? _selectedPersonRecord;
    private Border? _selectedPersonRowBorder;

    private void EnsurePersonDatabaseInitialized()
    {
        if (_globalPersonDatabase != null) return;

        _globalPersonDatabase = new List<PersonSearchRecord>
        {
            new PersonSearchRecord
            {
                Name = "AXIO, MOCK",
                Mrn = "1000245689",
                Dob = "08/08/96",
                Sex = "M",
                Age = "30",
                AccountNumber = "98214567",
                Address = "123 Medical Center Way, Vancouver, BC",
                Phone = "09243657795",
                Encounters = new List<EncounterSearchRecord>
                {
                    new() { EncounterNumber = "ENC-849201", Facility = "Axio General Hospital", EncounterType = "Inpatient", DateOfService = "08/12/2026", Resource = "Dr. Robert Chen", Guarantor = "Self", DischargeDate = "--" },
                    new() { EncounterNumber = "ENC-830112", Facility = "Axio West Clinic", EncounterType = "Outpatient", DateOfService = "05/10/2026", Resource = "Dr. Sarah Jenkins", Guarantor = "BlueCross", DischargeDate = "05/10/2026" }
                }
            },
            new PersonSearchRecord
            {
                Name = "JOHN DOE",
                Mrn = "1000245690",
                Dob = "05/14/81",
                Sex = "M",
                Age = "45",
                AccountNumber = "98214568",
                Address = "742 Evergreen Terrace, Seattle, WA",
                Phone = "555-0199",
                Encounters = new List<EncounterSearchRecord>
                {
                    new() { EncounterNumber = "ENC-849202", Facility = "Axio Medical Center", EncounterType = "Outpatient", DateOfService = "08/20/2026", Resource = "Dr. Sarah Jenkins", Guarantor = "BlueCross", DischargeDate = "--" }
                }
            },
            new PersonSearchRecord
            {
                Name = "TEST, NEWMERGE ONE",
                Mrn = "64802090",
                Dob = "01/01/51",
                Sex = "F",
                Age = "75",
                AccountNumber = "98214569",
                Address = "456 Elm St, Portland, OR",
                Phone = "555-0144",
                Encounters = new List<EncounterSearchRecord>
                {
                    new() { EncounterNumber = "ENC-648021", Facility = "Axio Community Clinic", EncounterType = "Ambulatory", DateOfService = "07/15/2026", Resource = "Dr. Alan Grant", Guarantor = "Medicare", DischargeDate = "07/15/2026" }
                }
            },
            new PersonSearchRecord
            {
                Name = "PHARMDRC, EIGHTMONTH",
                Mrn = "64802042",
                Dob = "09/22/16",
                Sex = "M",
                Age = "9",
                AccountNumber = "98214570",
                Address = "789 Oak Ave, Bellevue, WA",
                Phone = "555-0177",
                Encounters = new List<EncounterSearchRecord>
                {
                    new() { EncounterNumber = "ENC-648022", Facility = "Axio Pediatric Center", EncounterType = "Emergency", DateOfService = "06/10/2026", Resource = "Dr. Emily Stone", Guarantor = "Medicaid", DischargeDate = "06/11/2026" }
                }
            },
            new PersonSearchRecord
            {
                Name = "UCTEST, CPABBLINGCOMB",
                Mrn = "64801201",
                Dob = "03/15/74",
                Sex = "F",
                Age = "52",
                AccountNumber = "98214571",
                Address = "321 Pine Rd, Tacoma, WA",
                Phone = "555-0188",
                Encounters = new List<EncounterSearchRecord>
                {
                    new() { EncounterNumber = "ENC-648023", Facility = "Axio Urgent Care", EncounterType = "Urgent Care", DateOfService = "05/29/2026", Resource = "Dr. James Wilson", Guarantor = "Aetna", DischargeDate = "05/29/2026" }
                }
            },
            new PersonSearchRecord
            {
                Name = "PHARMDRC, EIGHTYEAR",
                Mrn = "64802043",
                Dob = "04/18/83",
                Sex = "F",
                Age = "43",
                AccountNumber = "98214572",
                Address = "654 Maple Dr, Spokane, WA",
                Phone = "555-0122",
                Encounters = new List<EncounterSearchRecord>
                {
                    new() { EncounterNumber = "ENC-648024", Facility = "Axio Medical Center", EncounterType = "Inpatient", DateOfService = "04/05/2026", Resource = "Dr. Marcus Brody", Guarantor = "UnitedHealth", DischargeDate = "04/12/2026" }
                }
            },
            new PersonSearchRecord
            {
                Name = "TESTRODNEY, INPATIENT",
                Mrn = "64802647",
                Dob = "11/14/79",
                Sex = "F",
                Age = "46",
                AccountNumber = "98214573",
                Address = "987 Cedar Blvd, Everett, WA",
                Phone = "555-0166",
                Encounters = new List<EncounterSearchRecord>
                {
                    new() { EncounterNumber = "ENC-648025", Facility = "Axio General Hospital", EncounterType = "Inpatient", DateOfService = "08/01/2026", Resource = "Dr. Lisa Cuddy", Guarantor = "Cigna", DischargeDate = "--" }
                }
            },
            new PersonSearchRecord
            {
                Name = "MEDTEST, JR",
                Mrn = "64801006",
                Dob = "12/01/87",
                Sex = "M",
                Age = "38",
                AccountNumber = "98214574",
                Address = "159 Birch Ln, Redmond, WA",
                Phone = "555-0133",
                Encounters = new List<EncounterSearchRecord>
                {
                    new() { EncounterNumber = "ENC-648026", Facility = "Axio Specialty Clinic", EncounterType = "Cardiology", DateOfService = "08/18/2026", Resource = "Dr. Gregory House", Guarantor = "Kaiser", DischargeDate = "--" }
                }
            }
        };
    }

    private void PerformPersonSearch()
    {
        EnsurePersonDatabaseInitialized();
        if (_globalPersonDatabase == null || PersonSearchResultsPanel == null) return;

        string last = PersonSearchLastNameBox?.Text?.Trim() ?? string.Empty;
        string first = PersonSearchFirstNameBox?.Text?.Trim() ?? string.Empty;
        string dob = PersonSearchDobBox?.Text?.Trim() ?? string.Empty;
        string phone = PersonSearchPhoneBox?.Text?.Trim() ?? string.Empty;
        string enc = PersonSearchEncounterBox?.Text?.Trim() ?? string.Empty;

        var results = _globalPersonDatabase.Where(p =>
        {
            if (!string.IsNullOrEmpty(last) && !p.Name.Contains(last, StringComparison.OrdinalIgnoreCase)) return false;
            if (!string.IsNullOrEmpty(first) && !p.Name.Contains(first, StringComparison.OrdinalIgnoreCase)) return false;
            if (!string.IsNullOrEmpty(dob) && !p.Dob.Contains(dob, StringComparison.OrdinalIgnoreCase)) return false;
            if (!string.IsNullOrEmpty(phone) && !p.Phone.Contains(phone, StringComparison.OrdinalIgnoreCase)) return false;
            if (!string.IsNullOrEmpty(enc) && !p.Encounters.Any(e => e.EncounterNumber.Contains(enc, StringComparison.OrdinalIgnoreCase))) return false;
            return true;
        }).ToList();

        PersonSearchResultsPanel.Children.Clear();
        _selectedPersonRecord = null;
        _selectedPersonRowBorder = null;

        if (EncounterSearchResultsPanel != null)
        {
            EncounterSearchResultsPanel.Children.Clear();
        }
        if (EncounterSearchEmptyPromptText != null)
        {
            EncounterSearchEmptyPromptText.Visibility = Visibility.Visible;
        }
        if (PersonSearchSelectButton != null)
        {
            PersonSearchSelectButton.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240));
            PersonSearchSelectButton.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 100, 116, 139));
        }

        if (results.Count == 0)
        {
            if (PersonSearchEmptyPromptText != null)
            {
                PersonSearchEmptyPromptText.Text = "No search results matching the specified criteria.";
                PersonSearchEmptyPromptText.Visibility = Visibility.Visible;
            }
            return;
        }

        if (PersonSearchEmptyPromptText != null)
        {
            PersonSearchEmptyPromptText.Visibility = Visibility.Collapsed;
        }

        int index = 0;
        foreach (var person in results)
        {
            var rowBorder = CreatePersonRow(person, index++);
            PersonSearchResultsPanel.Children.Add(rowBorder);
        }
    }

    private Border CreatePersonRow(PersonSearchRecord person, int index)
    {
        var rowBorder = new Border
        {
            Height = 22,
            Background = new SolidColorBrush(index % 2 == 0 ? Microsoft.UI.ColorHelper.FromArgb(255, 255, 255, 255) : Microsoft.UI.ColorHelper.FromArgb(255, 248, 250, 252)),
            BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)),
            BorderThickness = new Thickness(0, 0, 0, 1),
            Tag = person
        };

        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(35) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        var nameBlock = new TextBlock { Text = person.Name, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142)), FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, FontSize = 10, VerticalAlignment = VerticalAlignment.Center };
        var mrnBlock = new TextBlock { Text = person.Mrn, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center };
        var dobBlock = new TextBlock { Text = person.Dob, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center };
        var sexBlock = new TextBlock { Text = person.Sex, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center };
        var ageBlock = new TextBlock { Text = person.Age, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center };
        var accBlock = new TextBlock { Text = person.AccountNumber, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center };
        var addrBlock = new TextBlock { Text = person.Address, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 71, 85, 105)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center, TextTrimming = TextTrimming.CharacterEllipsis };

        var b0 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = nameBlock };
        var b1 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = mrnBlock };
        var b2 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = dobBlock };
        var b3 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = sexBlock };
        var b4 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = ageBlock };
        var b5 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = accBlock };
        var b6 = new Border { Padding = new Thickness(4, 1, 4, 1), Child = addrBlock };

        Grid.SetColumn(b0, 0); Grid.SetColumn(b1, 1); Grid.SetColumn(b2, 2); Grid.SetColumn(b3, 3);
        Grid.SetColumn(b4, 4); Grid.SetColumn(b5, 5); Grid.SetColumn(b6, 6);

        grid.Children.Add(b0); grid.Children.Add(b1); grid.Children.Add(b2); grid.Children.Add(b3);
        grid.Children.Add(b4); grid.Children.Add(b5); grid.Children.Add(b6);

        rowBorder.Child = grid;

        rowBorder.PointerPressed += (s, e) =>
        {
            SelectPersonRecord(person, rowBorder);
        };

        rowBorder.DoubleTapped += (s, e) =>
        {
            SelectPersonRecord(person, rowBorder);
            SelectAndOpenPerson(person);
        };

        return rowBorder;
    }

    private void SelectPersonRecord(PersonSearchRecord person, Border rowBorder)
    {
        if (_selectedPersonRowBorder != null)
        {
            _selectedPersonRowBorder.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 255, 255, 255));
        }

        _selectedPersonRecord = person;
        _selectedPersonRowBorder = rowBorder;
        rowBorder.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 186, 230, 253));

        if (PersonSearchSelectButton != null)
        {
            PersonSearchSelectButton.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
            PersonSearchSelectButton.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 255, 255, 255));
        }

        // Render encounters
        if (EncounterSearchResultsPanel != null)
        {
            EncounterSearchResultsPanel.Children.Clear();

            if (person.Encounters.Count > 0)
            {
                if (EncounterSearchEmptyPromptText != null) EncounterSearchEmptyPromptText.Visibility = Visibility.Collapsed;

                int encIdx = 0;
                foreach (var enc in person.Encounters)
                {
                    var encBorder = new Border
                    {
                        Height = 22,
                        Background = new SolidColorBrush(encIdx % 2 == 0 ? Microsoft.UI.ColorHelper.FromArgb(255, 255, 255, 255) : Microsoft.UI.ColorHelper.FromArgb(255, 248, 250, 252)),
                        BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)),
                        BorderThickness = new Thickness(0, 0, 0, 1)
                    };

                    var encGrid = new Grid();
                    encGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
                    encGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
                    encGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(95) });
                    encGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(85) });
                    encGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(105) });
                    encGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
                    encGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var eb0 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = new TextBlock { Text = enc.EncounterNumber, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142)), FontWeight = Microsoft.UI.Text.FontWeights.SemiBold, FontSize = 10, VerticalAlignment = VerticalAlignment.Center } };
                    var eb1 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = new TextBlock { Text = enc.Facility, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center } };
                    var eb2 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = new TextBlock { Text = enc.EncounterType, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center } };
                    var eb3 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = new TextBlock { Text = enc.DateOfService, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center } };
                    var eb4 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = new TextBlock { Text = enc.Resource, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center } };
                    var eb5 = new Border { Padding = new Thickness(4, 1, 4, 1), BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 226, 232, 240)), BorderThickness = new Thickness(0, 0, 1, 0), Child = new TextBlock { Text = enc.Guarantor, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 41, 59)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center } };
                    var eb6 = new Border { Padding = new Thickness(4, 1, 4, 1), Child = new TextBlock { Text = enc.DischargeDate, Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 71, 85, 105)), FontSize = 10, VerticalAlignment = VerticalAlignment.Center } };

                    Grid.SetColumn(eb0, 0); Grid.SetColumn(eb1, 1); Grid.SetColumn(eb2, 2); Grid.SetColumn(eb3, 3);
                    Grid.SetColumn(eb4, 4); Grid.SetColumn(eb5, 5); Grid.SetColumn(eb6, 6);

                    encGrid.Children.Add(eb0); encGrid.Children.Add(eb1); encGrid.Children.Add(eb2); encGrid.Children.Add(eb3);
                    encGrid.Children.Add(eb4); encGrid.Children.Add(eb5); encGrid.Children.Add(eb6);

                    encBorder.Child = encGrid;
                    EncounterSearchResultsPanel.Children.Add(encBorder);
                    encIdx++;
                }
            }
            else
            {
                if (EncounterSearchEmptyPromptText != null)
                {
                    EncounterSearchEmptyPromptText.Text = "No encounters found for selected person.";
                    EncounterSearchEmptyPromptText.Visibility = Visibility.Visible;
                }
            }
        }
    }

    private void OnPersonSearchSelectClick(object sender, RoutedEventArgs e)
    {
        if (_selectedPersonRecord != null)
        {
            SelectAndOpenPerson(_selectedPersonRecord);
        }
    }

    public void UpdatePatientBanner(string name, string mrn = "AVX-SL-A1H4XE", string dob = "3/22/1984", string age = "39 years", string sex = "Female", string allergies = "Allergies: shellfish", string doseWt = "Dose Wt: 80.000 kg (04/25/2021)", string healthLife = "HealtheLife: No", string ltd = "LTD:", string clinic = "Clinic: 01XTQ", string codeStatus = "Code Status: Active", string loc = "Loc: GLW-01", string nominee = "Nominee: Mr. Ajan singh", string fin = "Inpatient FIN: 00095526415 [Admit Dt: 10/6/2020 3:14:12 PM CDT]")
    {
        if (BannerPatientNameText != null) BannerPatientNameText.Text = name;
        if (BannerAllergiesText != null) BannerAllergiesText.Text = allergies;
        if (BannerDobText != null) BannerDobText.Text = dob.StartsWith("DOB:") ? dob : $"DOB: {dob}";
        if (BannerDoseWeightText != null) BannerDoseWeightText.Text = doseWt;
        if (BannerHealtheLifeText != null) BannerHealtheLifeText.Text = healthLife;
        if (BannerAgeText != null) BannerAgeText.Text = age.StartsWith("Age:") ? age : $"Age: {age}";
        if (BannerLtdText != null) BannerLtdText.Text = ltd;
        if (BannerClinicText != null) BannerClinicText.Text = clinic;
        if (BannerSexText != null) BannerSexText.Text = sex.StartsWith("Sex:") ? sex : $"Sex: {sex}";
        if (BannerCodeStatusText != null) BannerCodeStatusText.Text = codeStatus;
        if (BannerLocText != null) BannerLocText.Text = loc;
        if (BannerMrnText != null) BannerMrnText.Text = mrn.StartsWith("MRN:") ? mrn : $"MRN: {mrn}";
        if (BannerNomineeText != null) BannerNomineeText.Text = nominee;
        if (BannerFinText != null) BannerFinText.Text = fin;
    }

    private string _currentSelectedPatientName = "JOHN DOE";

    public void ShowPatientProfileByName(string patientName)
    {
        var profile = GetClinicalProfileForPatient(patientName);
        _currentSelectedPatientName = profile.Name;

        // Update Top Demographic Banner
        UpdatePatientBanner(
            name: profile.Name,
            mrn: profile.Mrn,
            dob: profile.Dob,
            age: profile.Age,
            sex: profile.Gender,
            allergies: profile.Allergies,
            doseWt: profile.DoseWt,
            healthLife: profile.HealtheLife,
            clinic: profile.Clinic,
            codeStatus: profile.CodeStatus,
            loc: profile.Location,
            nominee: profile.Nominee,
            fin: profile.Fin
        );

        // Update Permanent Bottom Status Bar
        if (PermanentFooterPatientText != null)
        {
            PermanentFooterPatientText.Text = $"Patient: {profile.Name} ( MRN: {profile.Mrn} )";
        }

        // Update Patient Profile View content
        if (PatientProfileViewControl != null)
        {
            PatientProfileViewControl.SetPatient(profile);
        }

        // Switch to Patient Profile Tab
        ResetAllRibbonTabHighlights();
        string tabTitle = $"Patient Profile: {profile.Name}";
        OpenOrActivateTab("patient_profile", tabTitle, "Patient Profile", PatientProfileViewControl, isCloseable: false);
    }

    public PatientClinicalProfile GetClinicalProfileForPatient(string query)
    {
        string norm = (query ?? "JOHN DOE").Trim().ToUpperInvariant();

        if (norm.Contains("MOCK") || norm.Contains("AXIO"))
        {
            return new PatientClinicalProfile
            {
                Name = "AXIO, MOCK",
                Mrn = "AVX-SL-A1H4XE",
                Dob = "08/08/1996",
                Age = "30",
                Gender = "Male",
                BloodType = "O+",
                Phone = "(604) 924-3657",
                Address = "123 Medical Center Way, Vancouver, BC",
                Status = "Active Inpatient",
                AttendingMd = "Dr. Robert Axio, MD",
                Allergies = "Allergies: Penicillin, Shellfish",
                DoseWt = "Dose Wt: 76.500 kg (05/10/2024)",
                HealtheLife = "HealtheLife: Yes",
                Clinic = "Clinic: 01XTQ",
                CodeStatus = "Code Status: Full Code",
                Location = "Loc: GLW-01",
                Nominee = "Nominee: Mr. Ajan Singh (Father)",
                Fin = "Inpatient FIN: 00095526415 [Admit Dt: 10/6/2025 3:14:12 PM CDT]",
                ImplantLot = "LOT-AXIO-77291-MED",
                DermalMatrix = "No",
                OpDescription = "Endoscopic exploration & diagnostic tissue biopsy",
                Comments = "Procedure completed successfully without complications.",
                OtherComments = "I am signing this report at the direction of administration, in the absence of Cerner Surgeon 1.",
                Notes = new List<(string, string, string, string)>
                {
                    ("Admission Clinical Assessment", "Today 09:00 AM", "Dr. Robert Axio, MD", "Patient admitted for routine diagnostic evaluation. Vital signs stable, afebrile."),
                    ("Pharmacy Consult Note", "Yesterday 02:30 PM", "Lisa Wong, PharmD", "Penicillin allergy confirmed. Ciprofloxacin prescribed with renal adjustment.")
                },
                AllergyList = new List<(string, string, string, string)>
                {
                    ("Penicillin", "Anaphylactic shock", "Severe", "Drug"),
                    ("Shellfish", "Urticaria / Hives", "Moderate", "Food")
                },
                Diagnoses = new List<(string, string, string, string, string)>
                {
                    ("R10.9", "Abdominal pain, unspecified", "Principal", "10/05/2025", "Active"),
                    ("K21.9", "Gastro-esophageal reflux disease without esophagitis", "Secondary", "02/11/2022", "Active")
                },
                Medications = new List<(string, string, string, string, string)>
                {
                    ("Pantoprazole PO", "40 mg", "Oral", "Daily (Morning AC)", "Active"),
                    ("Ciprofloxacin IV", "400 mg", "Intravenous", "Q12H", "Active"),
                    ("Acetaminophen PO", "500 mg", "Oral", "Q4H PRN Pain", "Active")
                },
                Histories = new List<(string, string)>
                {
                    ("Past Medical History", "GERD, Seasonal allergic rhinitis."),
                    ("Past Surgical History", "Tonsillectomy (2008)."),
                    ("Family History", "No family history of coronary artery disease or malignancy.")
                },
                Insurance = ("Pacific Blue Cross", "PBC-882910-01", "GRP-94821", "Self (Mock Axio)", "Comprehensive Platinum Plan")
            };
        }
        else if (norm.Contains("NEWMERGE") || norm.Contains("TEST, NEW"))
        {
            return new PatientClinicalProfile
            {
                Name = "TEST, NEWMERGE ONE",
                Mrn = "64802090",
                Dob = "01/01/1951",
                Age = "75",
                Gender = "Female",
                BloodType = "B+",
                Phone = "(514) 888-2931",
                Address = "45 Rue Sainte-Catherine, Montreal, QC",
                Status = "Observation",
                AttendingMd = "Dr. Jean-Luc Girard, MD",
                Allergies = "Allergies: Latex, Sulfa",
                DoseWt = "Dose Wt: 62.000 kg (01/15/2025)",
                HealtheLife = "HealtheLife: No",
                Clinic = "Clinic: SURG-03",
                CodeStatus = "Code Status: DNR",
                Location = "Loc: 3W-302-B",
                Nominee = "Nominee: Marie Tremblay (Daughter)",
                Fin = "Inpatient FIN: 00094118274 [Admit Dt: 11/12/2025 08:30:00 AM EST]",
                ImplantLot = "LOT-MRG-99214",
                DermalMatrix = "Yes",
                OpDescription = "Bilateral knee arthroplasty tissue stabilization",
                Comments = "Post-operative recovery smooth. Physical therapy initiated.",
                OtherComments = "Verified by Dr. Girard.",
                Notes = new List<(string, string, string, string)>
                {
                    ("Surgical Discharge Summary", "11/14/2025", "Dr. Jean-Luc Girard, MD", "Wound healing well. Home health nursing and PT scheduled."),
                    ("Cardiology Clearance", "11/11/2025", "Dr. Marc Dupont, MD", "Cleared for elective orthopedic procedure. Echo LVEF 55%.")
                },
                AllergyList = new List<(string, string, string, string)>
                {
                    ("Latex", "Contact dermatitis & bronchospasm", "Severe", "Environmental"),
                    ("Sulfa Drugs", "Diffuse Erythema", "Moderate", "Drug")
                },
                Diagnoses = new List<(string, string, string, string, string)>
                {
                    ("M17.0", "Bilateral primary osteoarthritis of knee", "Principal", "11/12/2025", "Active"),
                    ("I10", "Essential hypertension", "Chronic", "03/04/2010", "Active")
                },
                Medications = new List<(string, string, string, string, string)>
                {
                    ("Amlodipine PO", "5 mg", "Oral", "Daily", "Active"),
                    ("Oxycodone PO", "5 mg", "Oral", "Q4H PRN severe pain", "Active")
                },
                Histories = new List<(string, string)>
                {
                    ("Past Medical History", "Osteoarthritis, Osteoporosis, Hypertension."),
                    ("Surgical History", "Cholecystectomy (1998), Total Hip Arthroplasty (2016).")
                },
                Insurance = ("RAMQ / Medavie Blue Cross", "MED-491028-11", "GRP-3310", "Marie Tremblay", "Senior Extended Care")
            };
        }
        else if (norm.Contains("PHARMDRC") || norm.Contains("EIGHTMONTH"))
        {
            return new PatientClinicalProfile
            {
                Name = "PHARMDRC, EIGHTMONTH",
                Mrn = "64802042",
                Dob = "09/22/2023",
                Age = "8 mos",
                Gender = "Male",
                BloodType = "O+",
                Phone = "(604) 333-7890",
                Address = "888 Burrard St, Vancouver, BC",
                Status = "Active Inpatient",
                AttendingMd = "Dr. Lisa Wang, MD (Pediatrics)",
                Allergies = "Allergies: NKA",
                DoseWt = "Dose Wt: 8.400 kg (02/14/2024)",
                HealtheLife = "HealtheLife: Yes",
                Clinic = "Clinic: PEDS-01",
                CodeStatus = "Code Status: Full Code",
                Location = "Loc: PEDS-104",
                Nominee = "Nominee: Elena Chen (Mother)",
                Fin = "Inpatient FIN: 00095882190 [Admit Dt: 01/15/2026 10:15:00 AM PST]",
                ImplantLot = "N/A",
                DermalMatrix = "No",
                OpDescription = "Pediatric developmental screening & bronchiolitis monitoring",
                Comments = "Infant feeding well, respiratory rate normal on room air.",
                OtherComments = "Discharge anticipated within 24 hours.",
                Notes = new List<(string, string, string, string)>
                {
                    ("Pediatric Daily Progress Note", "Today 07:45 AM", "Dr. Lisa Wang, MD", "Lungs clear bilaterally. Hydration status optimal.")
                },
                AllergyList = new List<(string, string, string, string)>
                {
                    ("No Known Allergies", "None reported", "None", "General")
                },
                Diagnoses = new List<(string, string, string, string, string)>
                {
                    ("J21.0", "Acute bronchiolitis due to respiratory syncytial virus", "Principal", "01/15/2026", "Active")
                },
                Medications = new List<(string, string, string, string, string)>
                {
                    ("Normal Saline Nasal Drops", "2 drops", "Nasal", "Q4H PRN congestion", "Active"),
                    ("Infant Acetaminophen PO", "100 mg", "Oral", "Q4-6H PRN fever > 38.5C", "Active")
                },
                Histories = new List<(string, string)>
                {
                    ("Birth History", "Born full-term (39 weeks) via spontaneous vaginal delivery, APGAR 9/10."),
                    ("Immunization History", "Up to date for 6-month vaccinations.")
                },
                Insurance = ("BC Medical Services Plan", "MSP-8391024-9", "GRP-PROV", "Elena Chen", "Provincial Infant Coverage")
            };
        }
        else if (norm.Contains("ADAMS") || norm.Contains("ELEANOR"))
        {
            return new PatientClinicalProfile
            {
                Name = "ADAMS, ELEANOR",
                Mrn = "1000245690",
                Dob = "04/18/1958",
                Age = "67",
                Gender = "Female",
                BloodType = "A-",
                Phone = "(250) 412-8823",
                Address = "1020 Victoria Ave, Victoria, BC",
                Status = "Active Inpatient",
                AttendingMd = "Dr. Marcus Sterling, MD",
                Allergies = "Allergies: Aspirin, NSAIDs",
                DoseWt = "Dose Wt: 68.200 kg (02/01/2026)",
                HealtheLife = "HealtheLife: Yes",
                Clinic = "Clinic: CARD-04",
                CodeStatus = "Code Status: Full Code",
                Location = "Loc: CARD-04",
                Nominee = "Nominee: James Adams (Spouse)",
                Fin = "Inpatient FIN: 00097120391 [Admit Dt: 02/10/2026 09:45:00 AM PST]",
                ImplantLot = "LOT-STENT-88392",
                DermalMatrix = "No",
                OpDescription = "Percutaneous coronary intervention with drug-eluting stent",
                Comments = "Good hemostasis achieved via radial access band.",
                OtherComments = "Transfer to telemetry ward complete.",
                Notes = new List<(string, string, string, string)>
                {
                    ("Cardiology Post-Cath Note", "02/10/2026 02:00 PM", "Dr. Marcus Sterling, MD", "Successful PCI to LAD with 3.0x18mm DES. TIMI 3 flow restored.")
                },
                AllergyList = new List<(string, string, string, string)>
                {
                    ("Aspirin", "Bronchospasm / Angioedema", "Severe", "Drug")
                },
                Diagnoses = new List<(string, string, string, string, string)>
                {
                    ("I21.0", "ST elevation (STEMI) myocardial infarction of anterior wall", "Principal", "02/10/2026", "Active"),
                    ("E11.9", "Type 2 diabetes mellitus without complications", "Secondary", "05/14/2012", "Active")
                },
                Medications = new List<(string, string, string, string, string)>
                {
                    ("Clopidogrel PO", "75 mg", "Oral", "Daily", "Active"),
                    ("Atorvastatin PO", "80 mg", "Oral", "Nightly", "Active"),
                    ("Metoprolol Tartrate PO", "25 mg", "Oral", "BID", "Active")
                },
                Histories = new List<(string, string)>
                {
                    ("Medical History", "Type 2 Diabetes, Coronary Artery Disease, Dyslipidemia."),
                    ("Family History", "Mother deceased age 70 from stroke.")
                },
                Insurance = ("Great-West Life", "GWL-774910-02", "GRP-1829", "James Adams", "Cardiology Preferred Provider")
            };
        }

        // Generic Dynamic Clinical Profile Generator for any Patient
        string cleanName = string.IsNullOrWhiteSpace(query) ? "JOHN DOE" : query.Trim();
        int hash = Math.Abs(cleanName.GetHashCode());
        int ageNum = 20 + (hash % 60);
        string gender = (hash % 2 == 0) ? "Female" : "Male";
        string mrnNum = $"AVX-{(100000 + (hash % 899999))}";
        string locRoom = $"GLW-0{(1 + (hash % 8))}";

        return new PatientClinicalProfile
        {
            Name = cleanName,
            Mrn = mrnNum,
            Dob = $"{1 + (hash % 12):D2}/{1 + (hash % 28):D2}/{DateTime.Now.Year - ageNum}",
            Age = $"{ageNum}",
            Gender = gender,
            BloodType = (hash % 4 == 0) ? "O+" : (hash % 4 == 1) ? "A+" : (hash % 4 == 2) ? "B+" : "AB+",
            Phone = $"({200 + (hash % 700)}) {300 + (hash % 600):D3}-{1000 + (hash % 8999):D4}",
            Address = $"{100 + (hash % 900)} Metro Healthcare Blvd, Suite {10 + (hash % 90)}",
            Status = "Active Inpatient",
            AttendingMd = "Dr. Sarah Miller, MD",
            Allergies = (hash % 3 == 0) ? "Allergies: shellfish" : (hash % 3 == 1) ? "Allergies: Penicillin, Sulfa" : "Allergies: NKA",
            DoseWt = $"Dose Wt: {55.0 + (hash % 40):F3} kg ({DateTime.Now.AddDays(-15):MM/dd/yyyy})",
            HealtheLife = (hash % 2 == 0) ? "HealtheLife: Yes" : "HealtheLife: No",
            Clinic = $"Clinic: 01XTQ",
            CodeStatus = "Code Status: Active",
            Location = $"Loc: {locRoom}",
            Nominee = $"Nominee: Mr. Ajan singh",
            Fin = $"Inpatient FIN: {hash % 10000000000:D11} [Admit Dt: 10/6/2020 3:14:12 PM CDT]",
            ImplantLot = $"LOT-{hash % 100000:D5}-CERNER",
            DermalMatrix = "No",
            OpDescription = "Routine clinical examination & procedure",
            Comments = "Patient comfortable, vitals stable, regular diet tolerated.",
            OtherComments = "I am signing this report at the direction of administration, in the absence of Cerner Surgeon 1.",
            Notes = new List<(string, string, string, string)>
            {
                ("Clinical Progress Note", "Today 08:30 AM", "Dr. Sarah Miller, MD", $"Patient {cleanName} is alert and oriented x4. Treatment plan progressing on schedule."),
                ("Nursing Shift Note", "Today 06:00 AM", "Jane Doe, RN", "Vitals within normal limits overnight. No acute complaints.")
            },
            AllergyList = new List<(string, string, string, string)>
            {
                ("Shellfish", "Urticaria", "Mild", "Food"),
                ("Penicillin", "Rash", "Moderate", "Drug")
            },
            Diagnoses = new List<(string, string, string, string, string)>
            {
                ("R10.9", "Abdominal discomfort, unspecified", "Principal", DateTime.Now.AddDays(-3).ToString("MM/dd/yyyy"), "Active"),
                ("I10", "Essential hypertension", "Secondary", "01/10/2019", "Active")
            },
            Medications = new List<(string, string, string, string, string)>
            {
                ("Acetaminophen PO", "650 mg", "Oral", "Q6H PRN Pain", "Active"),
                ("Normal Saline IV", "1000 mL", "Intravenous", "75 mL/hr", "Active")
            },
            Histories = new List<(string, string)>
            {
                ("Past Medical History", "Hypertension, Seasonal allergies."),
                ("Family History", "Non-contributory.")
            },
            Insurance = ("AxioVital Standard Health Plan", $"POL-{hash % 100000:D6}", "GRP-8819", $"Self ({cleanName})", "Comprehensive Inpatient")
        };
    }

    private void OnHeaderSearchBoxKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter && sender is TextBox tb && !string.IsNullOrWhiteSpace(tb.Text))
        {
            e.Handled = true;
            string query = tb.Text.Trim();
            ShowPatientProfileByName(query);
        }
    }

    private void SelectAndOpenPerson(PersonSearchRecord person)
    {
        ClosePersonSearch();
        ShowPatientProfileByName(person.Name);
    }
}
