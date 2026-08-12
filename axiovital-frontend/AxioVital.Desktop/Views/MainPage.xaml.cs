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
        SchedulerView.Visibility = Visibility.Collapsed;
        MessageCenterView.Visibility = Visibility.Visible;
    }

    
    public void ShowSchedulerView()
    {
        HeaderTitleText.Text = "Appointment Reschedule Requests";

        PatientProfileTabPill.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 230, 238, 242));
        MessageCenterTabPill.Visibility = Visibility.Collapsed;
        SchedulerTabPill.Visibility = Visibility.Visible;
        SchedulerTabPill.Background = new SolidColorBrush(Microsoft.UI.Colors.White);

        PatientDemographicBanner.Visibility = Visibility.Collapsed;

        PatientProfileView.Visibility = Visibility.Collapsed;
        MessageCenterView.Visibility = Visibility.Collapsed;
        SchedulerView.Visibility = Visibility.Visible;
    }

    private void OnSchedulerTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        ShowSchedulerView();
    }

    private void OnCloseSchedulerTabPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        SchedulerTabPill.Visibility = Visibility.Collapsed;
        ShowPatientProfileView();
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
        SchedulerView.Visibility = Visibility.Collapsed;
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

                private void OnPatientActionsMenuButtonClick(object sender, RoutedEventArgs e)
        {
            PatientActionsDropdownOverlay.Visibility = PatientActionsDropdownOverlay.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void OnPatientActionsDropdownOverlayPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed;
        }

        private void OnBedTransferMenuItemClick(object sender, RoutedEventArgs e)
        {
            PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed;
            BedTransferOverlay.Visibility = Visibility.Visible;
        }

        private void OnFacilityTransferMenuItemClick(object sender, RoutedEventArgs e)
        {
            PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed;
            FacilityTransferOverlay.Visibility = Visibility.Visible;
            if (this.Frame != null)
            {
                try
                {
                    this.Frame.Navigate(typeof(FacilityTransferPage));
                }
                catch { }
            }
        }

        private void OnCancelDischargeClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnCancelPendingDischargeClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnCancelPendingTransferClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnCancelTransferClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnClozapineRegistryClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnDischargeEncounterClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnLeaveOfAbsenceClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnModifyDischargeClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnPendingDischargeClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnPendingFacilityTransferClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnPendingTransferClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnPrintLabelsClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnProcessAlertClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnUpdatePatientInformationClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnViewEncounterClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }
        private void OnViewPersonClick(object sender, RoutedEventArgs e) { PatientActionsDropdownOverlay.Visibility = Visibility.Collapsed; }

        private void OnBedTransferCloseClicked(object sender, RoutedEventArgs e)
        {
            BedTransferOverlay.Visibility = Visibility.Collapsed;
        }

        private void OnFacilityTransferCloseClicked(object sender, RoutedEventArgs e)
        {
            FacilityTransferOverlay.Visibility = Visibility.Collapsed;
        }



        // Resizing logic for Bed Transfer Popup Card
        private bool _isResizingBedTransfer = false;
        private string _bedTransferResizeDir = "";
        private Windows.Foundation.Point _startPointerPos;
        private double _startWidth;
        private double _startHeight;

        private void OnResizePointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Microsoft.UI.Xaml.Shapes.Rectangle rect)
            {
                _isResizingBedTransfer = true;
                _bedTransferResizeDir = rect.Tag as string ?? "";
                _startPointerPos = e.GetCurrentPoint(BedTransferOverlay).Position;
                _startWidth = BedTransferCard.Width;
                _startHeight = BedTransferCard.Height;

                rect.CapturePointer(e.Pointer);
                e.Handled = true;
            }
        }

        private void OnResizePointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!_isResizingBedTransfer) return;

            var currentPos = e.GetCurrentPoint(BedTransferOverlay).Position;
            double deltaX = currentPos.X - _startPointerPos.X;
            double deltaY = currentPos.Y - _startPointerPos.Y;

            if (_bedTransferResizeDir.Contains("Right"))
            {
                BedTransferCard.Width = Math.Max(BedTransferCard.MinWidth, _startWidth + deltaX);
            }
            else if (_bedTransferResizeDir.Contains("Left"))
            {
                BedTransferCard.Width = Math.Max(BedTransferCard.MinWidth, _startWidth - deltaX);
            }

            if (_bedTransferResizeDir.Contains("Bottom"))
            {
                BedTransferCard.Height = Math.Max(BedTransferCard.MinHeight, _startHeight + deltaY);
            }
            else if (_bedTransferResizeDir.Contains("Top"))
            {
                BedTransferCard.Height = Math.Max(BedTransferCard.MinHeight, _startHeight - deltaY);
            }

            e.Handled = true;
        }

        private void OnResizePointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (_isResizingBedTransfer)
            {
                if (sender is Microsoft.UI.Xaml.Shapes.Rectangle rect)
                {
                    rect.ReleasePointerCapture(e.Pointer);
                }
                _isResizingBedTransfer = false;
                e.Handled = true;
            }
        }
    }

