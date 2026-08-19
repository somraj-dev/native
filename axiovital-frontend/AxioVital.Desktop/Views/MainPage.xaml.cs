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
            PatientListViewControl.PatientSelected += (s, e) => ShowPatientProfileView();
        }

        // Open Patient List view on launch
        ShowPatientListView();

        // Initialize Live Clock in Permanent Footer
        UpdateFooterDateTime();
        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (s, e) => UpdateFooterDateTime();
        timer.Start();
    }

    private void UpdateFooterDateTime()
    {
        if (FooterDateTimeText != null)
        {
            FooterDateTimeText.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        }
    }

    public void OpenOrActivateTab(string id, string title, string headerTitle, UIElement targetView, bool isCloseable = true)
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

    private void OnSchedulerTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowSchedulerView();
    }

    private void OnOrderSetsTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowOrdersView();
    }

    private void OnOngoingActivitiesTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowOngoingActivitiesView();
    }

    private void OnCustomisedTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowCustomisedView();
    }

    private void OnLabsTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowLabsView();
    }

    public void ShowPatientListView()
    {
        if (MessageCenterTabBorder != null)
        {
            MessageCenterTabBorder.BorderBrush = null;
            MessageCenterTabBorder.BorderThickness = new Thickness(0);
            MessageCenterTabText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 51, 51));
            MessageCenterTabText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        }

        if (PatientListTabBorder != null)
        {
            PatientListTabBorder.BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
            PatientListTabBorder.BorderThickness = new Thickness(0, 0, 0, 2);
            PatientListTabText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
            PatientListTabText.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
        }

        OpenOrActivateTab("patient_list", "Patient Profile: JOHN DOE", "Patient List", PatientListViewControl);
    }

    public void ShowPatientProfileView()
    {
        if (MessageCenterTabBorder != null)
        {
            MessageCenterTabBorder.BorderBrush = null;
            MessageCenterTabBorder.BorderThickness = new Thickness(0);
            MessageCenterTabText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 51, 51));
            MessageCenterTabText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        }

        if (PatientListTabBorder != null)
        {
            PatientListTabBorder.BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
            PatientListTabBorder.BorderThickness = new Thickness(0, 0, 0, 2);
            PatientListTabText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
            PatientListTabText.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
        }

        OpenOrActivateTab("patient_profile", "Patient Profile: JOHN DOE", "Patient Profile", PatientProfileViewControl, isCloseable: false);
    }

    public void ShowHomeView()
    {
        OpenOrActivateTab("home", "Home", "Home", PatientProfileViewControl);
    }

    public void ShowMessageCenterView()
    {
        if (MessageCenterTabBorder != null)
        {
            MessageCenterTabBorder.BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
            MessageCenterTabBorder.BorderThickness = new Thickness(0, 0, 0, 2);
            MessageCenterTabText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
            MessageCenterTabText.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
        }

        if (PatientListTabBorder != null)
        {
            PatientListTabBorder.BorderBrush = null;
            PatientListTabBorder.BorderThickness = new Thickness(0);
            PatientListTabText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 51, 51));
            PatientListTabText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        }

        OpenOrActivateTab("message_center", "General Messages: JOHN DOE", "Message Center", MessageCenterViewControl);
    }

    public void ShowSchedulerView()
    {
        OpenOrActivateTab("scheduler", "Appointment Reschedule Requests", "Appointment Reschedule Requests", SchedulerViewControl);
    }

    public void ShowOrdersView()
    {
        OpenOrActivateTab("orders", "Orders", "Orders", LabsViewControl);
    }

    public void ShowLabsView()
    {
        OpenOrActivateTab("labs", "Results Review", "Results Review", LabsViewControl);
    }

    public void ShowOngoingActivitiesView()
    {
        OpenOrActivateTab("ongoing_activities", "Ongoing Activities", "Ongoing Activities", OngoingActivitiesViewControl);
    }

    public void ShowCustomisedView()
    {
        OpenOrActivateTab("customised", "Customised Organizer", "Customised Organizer", CustomisedViewControl);
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

    public void ToggleQuickPanel()
    {
    }
}
