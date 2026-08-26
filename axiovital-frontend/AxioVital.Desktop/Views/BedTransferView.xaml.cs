using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace AxioVital.Desktop.Views;

/// <summary>
/// Bed Transfer modal popup view for patient room and ward transfers.
/// </summary>
public sealed partial class BedTransferView : UserControl
{
    public event EventHandler? CloseRequested;
    public event EventHandler? TransferSaved;

    public BedTransferView()
    {
        this.InitializeComponent();
        SetCurrentDateTime();
    }

    public void SetCurrentDateTime()
    {
        if (TransferDateBox != null)
        {
            TransferDateBox.Text = DateTime.Now.ToString("MM/dd/yyyy");
        }
        if (TransferTimeBox != null)
        {
            TransferTimeBox.Text = DateTime.Now.ToString("HHmm");
        }
    }

    public void SetPatient(string patientId, string patientName, string department = "General Medicine", string currentWard = "General Ward A")
    {
        if (PatientIdBox != null)
        {
            PatientIdBox.Text = string.IsNullOrWhiteSpace(patientId) ? "TTPTEST" : patientId;
        }
        if (PatientNameBox != null)
        {
            PatientNameBox.Text = string.IsNullOrWhiteSpace(patientName) ? "PATIENT02" : patientName;
        }
    }

    private void OnSaveClicked(object sender, RoutedEventArgs e)
    {
        TransferSaved?.Invoke(this, EventArgs.Empty);
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnCancelClicked(object sender, RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
