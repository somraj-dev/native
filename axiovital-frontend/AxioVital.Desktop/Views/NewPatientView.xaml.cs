using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;

namespace AxioVital.Desktop.Views;

public sealed partial class NewPatientView : UserControl
{
    public event EventHandler? CloseRequested;

    public NewPatientView()
    {
        this.InitializeComponent();
    }

    private void OnNameFieldChanged(object sender, TextChangedEventArgs e)
    {
        if (WindowTitleText != null)
        {
            string first = InputFirstName?.Text?.Trim() ?? "";
            string last = InputSurname?.Text?.Trim() ?? "";
            string rec = InputRecordNo?.Text?.Trim() ?? "New";
            string name = string.IsNullOrWhiteSpace(first) && string.IsNullOrWhiteSpace(last) ? "New Patient" : $"{first} {last}".Trim();
            WindowTitleText.Text = $"{name} - {rec} - Patient Details";
        }
    }

    private void OnDobChanged(object sender, TextChangedEventArgs e)
    {
        if (InputDob != null && InputAge != null && DateTime.TryParse(InputDob.Text, out DateTime dob))
        {
            int age = DateTime.Today.Year - dob.Year;
            if (dob.Date > DateTime.Today.AddYears(-age)) age--;
            InputAge.Text = $"{Math.Max(0, age)} years";
        }
    }

    private void OnDeceasedChecked(object sender, RoutedEventArgs e)
    {
        if (InputDeceasedDate != null)
        {
            InputDeceasedDate.IsEnabled = true;
            InputDeceasedDate.Text = DateTime.Today.ToString("d/MM/yyyy");
        }
    }

    private void OnDeceasedUnchecked(object sender, RoutedEventArgs e)
    {
        if (InputDeceasedDate != null)
        {
            InputDeceasedDate.IsEnabled = false;
            InputDeceasedDate.Text = "";
        }
    }

    private void OnAddressTabPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b && b.Tag is string tag)
        {
            if (tag == "res")
            {
                TabResidential.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 236, 233, 216));
                (TabResidential.Child as TextBlock)!.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;
                TabPostal.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 223, 221, 208));
                (TabPostal.Child as TextBlock)!.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
            }
            else
            {
                TabPostal.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 236, 233, 216));
                (TabPostal.Child as TextBlock)!.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;
                TabResidential.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 223, 221, 208));
                (TabResidential.Child as TextBlock)!.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
            }
        }
    }

    private void OnSuburbLookupClick(object sender, RoutedEventArgs e)
    {
        if (InputSuburb != null && InputState != null && InputPostcode != null)
        {
            InputSuburb.Text = "MELBOURNE";
            InputState.Text = "VIC";
            InputPostcode.Text = "3000";
        }
    }

    private void OnVerifyMedicareClick(object sender, RoutedEventArgs e)
    {
        if (MedicareVerificationStatusText != null)
        {
            MedicareVerificationStatusText.Text = "Verified Active (Medicare AU Online).";
            MedicareVerificationStatusText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 22, 163, 74));
        }
    }

    private void OnNewAccountClick(object sender, RoutedEventArgs e)
    {
        // Add or reload accounts
    }

    private void OnAlertsClick(object sender, RoutedEventArgs e)
    {
        // Alerts dialog handler
    }

    private void OnDialogRootPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        // Prevent click bubbling up to backdrop
        e.Handled = true;
    }

    private void OnCloseClick(object sender, PointerRoutedEventArgs e)
    {
        e.Handled = true;
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
