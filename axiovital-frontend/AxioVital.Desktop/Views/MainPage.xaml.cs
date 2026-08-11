using AxioVital.Desktop.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;

namespace AxioVital.Desktop.Views;

/// <summary>
/// Main application view containing patient profile and clinical Message Center environment.
/// </summary>
public partial class MainPage : Page
{
    public List<MessageCenterItem> MessageItems { get; }

    public MainPage()
    {
        this.InitializeComponent();

        MessageItems = GetSampleMessageItems();
        MessageCenterItemsControl.ItemsSource = MessageItems;
    }

    private void OnMessageCenterTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowMessageCenterView();
    }

    private void OnPatientListTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowPatientProfileView();
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
        TopRibbonBarGrid.Visibility = vis;
        SubNavBarGrid.Visibility = vis;
        CategoryBarGrid.Visibility = vis;
        DarkHeaderBarGrid.Visibility = vis;
        TabStripBarGrid.Visibility = vis;
    }

    public void ExitPageFullScreen()
    {
        if (_isPageFullScreen)
        {
            _isPageFullScreen = false;
            TopRibbonBarGrid.Visibility = Visibility.Visible;
            SubNavBarGrid.Visibility = Visibility.Visible;
            CategoryBarGrid.Visibility = Visibility.Visible;
            DarkHeaderBarGrid.Visibility = Visibility.Visible;
            TabStripBarGrid.Visibility = Visibility.Visible;
        }
    }

    public void ShowMessageCenterView()
    {
        // Update Sub Nav 2 Highlight
        MessageCenterTabBorder.BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
        MessageCenterTabBorder.BorderThickness = new Thickness(0, 0, 0, 2);
        MessageCenterTabText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
        MessageCenterTabText.FontWeight = Microsoft.UI.Text.FontWeights.Bold;

        PatientListTabBorder.BorderBrush = null;
        PatientListTabBorder.BorderThickness = new Thickness(0);
        PatientListTabText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 51, 51));
        PatientListTabText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;

        // Update Header Bar 4
        HeaderTitleText.Text = "Message Center";

        // Update Tab Strip Bar 5
        PatientProfileTabPill.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 230, 238, 242));
        MessageCenterTabPill.Visibility = Visibility.Visible;
        MessageCenterTabPill.Background = new SolidColorBrush(Microsoft.UI.Colors.White);

        // Hide Patient Demographic Banner for full Message Center grid view
        PatientDemographicBanner.Visibility = Visibility.Collapsed;

        // Toggle Content Views
        PatientProfileView.Visibility = Visibility.Collapsed;
        MessageCenterView.Visibility = Visibility.Visible;
    }

    public void ShowPatientProfileView()
    {
        // Update Sub Nav 2 Highlight
        PatientListTabBorder.BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
        PatientListTabBorder.BorderThickness = new Thickness(0, 0, 0, 2);
        PatientListTabText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
        PatientListTabText.FontWeight = Microsoft.UI.Text.FontWeights.Bold;

        MessageCenterTabBorder.BorderBrush = null;
        MessageCenterTabBorder.BorderThickness = new Thickness(0);
        MessageCenterTabText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 51, 51, 51));
        MessageCenterTabText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;

        // Update Header Bar 4
        HeaderTitleText.Text = "Patient Profile";

        // Update Tab Strip Bar 5
        PatientProfileTabPill.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 230, 238, 242));
        MessageCenterTabPill.Visibility = Visibility.Collapsed;

        // Show Patient Demographic Banner
        PatientDemographicBanner.Visibility = Visibility.Visible;

        // Toggle Content Views
        MessageCenterView.Visibility = Visibility.Collapsed;
        PatientProfileView.Visibility = Visibility.Visible;
    }

    private static List<MessageCenterItem> GetSampleMessageItems()
    {
        return new List<MessageCenterItem>
        {
            new() { PatientName = "JAMES, WILLIAM", PlanName = "CBC with Differential", Action = "Plan", DetailsDate = "05/28/17 08:30...", DetailsDesc = "Routine blood test", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 08:30", StopDate = "05/28/2017 08:30", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "JAMES, WILLIAM", PlanName = "Comprehensive Metabolic Panel", Action = "Plan", DetailsDate = "05/28/17 08:30...", DetailsDesc = "Kidney & liver function", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 08:30", StopDate = "05/28/2017 08:30", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "PATEL, RAHUL", PlanName = "MRI Brain W/O Contrast", Action = "Plan", DetailsDate = "05/28/17 09:15...", DetailsDesc = "Headache evaluation", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 09:15", StopDate = "05/28/2017 09:15", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "PATEL, RAHUL", PlanName = "Referral to City Neuro Hospital", Action = "Referral", DetailsDate = "05/28/17 09:15...", DetailsDesc = "Transfer for advanced neuro care", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 09:15", StopDate = "05/28/2017 09:15", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "JOHNSON, MARIA", PlanName = "PT Evaluation", Action = "Plan", DetailsDate = "05/28/17 10:00...", DetailsDesc = "Post-op rehab", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 10:00", StopDate = "05/28/2017 10:00", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "JOHNSON, MARIA", PlanName = "Referral to St. Mary Regional Medical", Action = "Referral", DetailsDate = "05/28/17 10:00...", DetailsDesc = "Transfer for specialty pain mgmt", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 10:00", StopDate = "05/28/2017 10:00", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "LEE, DAVID", PlanName = "Chest X-Ray", Action = "Plan", DetailsDate = "05/28/17 10:30...", DetailsDesc = "Cough and fever", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 10:30", StopDate = "05/28/2017 10:30", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "LEE, DAVID", PlanName = "Sputum Culture", Action = "Plan", DetailsDate = "05/28/17 10:30...", DetailsDesc = "Infection workup", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 10:30", StopDate = "05/28/2017 10:30", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "GARCIA, LUCIA", PlanName = "Echocardiogram", Action = "Plan", DetailsDate = "05/28/17 11:00...", DetailsDesc = "Cardiac evaluation", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 11:00", StopDate = "05/28/2017 11:00", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "GARCIA, LUCIA", PlanName = "Referral to Metro Heart Institute", Action = "Referral", DetailsDate = "05/28/17 11:00...", DetailsDesc = "Transfer for cardiac surgery eval", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 11:00", StopDate = "05/28/2017 11:00", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "KIM, JAMES", PlanName = "Hemoglobin A1C", Action = "Plan", DetailsDate = "05/28/17 11:30...", DetailsDesc = "Diabetes monitoring", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 11:30", StopDate = "05/28/2017 11:30", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "KIM, JAMES", PlanName = "Diabetes Education", Action = "Plan", DetailsDate = "05/28/17 11:30...", DetailsDesc = "Patient education", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 11:30", StopDate = "05/28/2017 11:30", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "BROWN, ELIZABETH", PlanName = "Urinalysis", Action = "Plan", DetailsDate = "05/28/17 12:00...", DetailsDesc = "UTI symptoms", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 12:00", StopDate = "05/28/2017 12:00", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "BROWN, ELIZABETH", PlanName = "Urine Culture", Action = "Plan", DetailsDate = "05/28/17 12:00...", DetailsDesc = "Confirm infection", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 12:00", StopDate = "05/28/2017 12:00", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "THOMAS, MICHAEL", PlanName = "CT Abdomen & Pelvis", Action = "Plan", DetailsDate = "05/28/17 12:30...", DetailsDesc = "Abdominal pain", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 12:30", StopDate = "05/28/2017 12:30", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "THOMAS, MICHAEL", PlanName = "Referral to General Surgical Center", Action = "Referral", DetailsDate = "05/28/17 12:30...", DetailsDesc = "Transfer for emergency surgery", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 12:30", StopDate = "05/28/2017 12:30", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "ANDERSON, SUSAN", PlanName = "Lipid Panel", Action = "Plan", DetailsDate = "05/28/17 13:00...", DetailsDesc = "Cholesterol check", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 13:00", StopDate = "05/28/2017 13:00", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "ANDERSON, SUSAN", PlanName = "Nutrition Consult", Action = "Plan", DetailsDate = "05/28/17 13:00...", DetailsDesc = "Dietary counseling", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 13:00", StopDate = "05/28/2017 13:00", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "MILLER, ROBERT", PlanName = "Pulmonary Function Test", Action = "Plan", DetailsDate = "05/28/17 13:30...", DetailsDesc = "COPD evaluation", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 13:30", StopDate = "05/28/2017 13:30", StopType = "Physician Stop", Status = "Open" },
            new() { PatientName = "MILLER, ROBERT", PlanName = "Referral to Pulmonary Care Hospital", Action = "Referral", DetailsDate = "05/28/17 13:30...", DetailsDesc = "Transfer for advanced COPD care", Comment = "AXIO, MD", OriginatorName = "AXIO, MD", CreateDate = "05/28/2017 13:30", StopDate = "05/28/2017 13:30", StopType = "Physician Stop", Status = "Open" }
        };
    }
    
    public void ToggleQuickPanel()
        {
            if (QuickPanelOverlay.Visibility == Visibility.Visible)
            {
                QuickPanelOverlay.Visibility = Visibility.Collapsed;
            }
            else
            {
                QuickPanelOverlay.Visibility = Visibility.Visible;
            }
        }

        private void OnQuickPanelCloseClicked(object sender, RoutedEventArgs e)
        {
            QuickPanelOverlay.Visibility = Visibility.Collapsed;
        }
    }
