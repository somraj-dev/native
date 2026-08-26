using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace AxioVital.Desktop.Views;

public class PatientSelectedEventArgs : EventArgs
{
    public string PatientName { get; set; } = string.Empty;
}

public sealed partial class PatientListView : UserControl
{
    public event EventHandler<PatientSelectedEventArgs>? PatientSelected;

    public PatientListView()
    {
        this.InitializeComponent();
    }

    private void OnPatientNamePointerPressed(object sender, PointerRoutedEventArgs e)
    {
        string patientName = "JOHN DOE";
        if (sender is TextBlock tb && !string.IsNullOrWhiteSpace(tb.Text))
        {
            patientName = tb.Text.Trim();
        }
        else if (sender is FrameworkElement fe && fe.DataContext is string nameStr)
        {
            patientName = nameStr;
        }

        PatientSelected?.Invoke(this, new PatientSelectedEventArgs { PatientName = patientName });
    }
}
