using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace AxioVital.Desktop.Views;

public sealed partial class PatientListView : UserControl
{
    public event RoutedEventHandler? PatientSelected;

    public PatientListView()
    {
        this.InitializeComponent();
    }

    private void OnPatientNamePointerPressed(object sender, PointerRoutedEventArgs e)
    {
        PatientSelected?.Invoke(this, new RoutedEventArgs());
    }
}
