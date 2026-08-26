using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace AxioVital.Desktop.Views;

/// <summary>
/// Medical Record Request - Patient Encounter dialog for View Encounter.
/// </summary>
public sealed partial class ViewEncounterDialog : UserControl
{
    public event EventHandler? CloseClicked;
    public event EventHandler? CloseRequested;
    public event EventHandler? PreviewClicked;
    public event EventHandler? SendClicked;

    public ViewEncounterDialog()
    {
        this.InitializeComponent();
    }

    public void SetPatientInfo(string patientName, string? fromDate = null, string? destination = null)
    {
        if (DialogTitleText != null)
        {
            DialogTitleText.Text = $"Medical Record Request - {patientName.ToUpperInvariant()}";
        }

        if (DateRangeFromBox != null && !string.IsNullOrWhiteSpace(fromDate))
        {
            DateRangeFromBox.Text = fromDate;
        }

        if (DestinationBox != null && !string.IsNullOrWhiteSpace(destination))
        {
            DestinationBox.Text = destination;
        }
    }

    public void SetDefaultValues(string patientName = "JOHN DOE", string destination = "VGH Transplant Clinic")
    {
        SetPatientInfo(patientName, "09-Sep-2023", destination);
    }

    private void OnCloseClicked(object sender, RoutedEventArgs e)
    {
        CloseClicked?.Invoke(this, EventArgs.Empty);
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnPreviewClicked(object sender, RoutedEventArgs e)
    {
        PreviewClicked?.Invoke(this, EventArgs.Empty);
    }

    private void OnSendClicked(object sender, RoutedEventArgs e)
    {
        SendClicked?.Invoke(this, EventArgs.Empty);
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
