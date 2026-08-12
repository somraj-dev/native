using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AxioVital.Desktop.Views;

/// <summary>
/// A full-screen page for filling out facility transfer information.
/// </summary>
public sealed partial class FacilityTransferPage : Page
{
    public FacilityTransferPage()
    {
        this.InitializeComponent();
    }

    private void OnBackClicked(object sender, RoutedEventArgs e)
    {
        if (this.Frame != null && this.Frame.CanGoBack)
        {
            this.Frame.GoBack();
        }
        else
        {
            this.Frame?.Navigate(typeof(MainPage));
        }
    }

    private void OnSaveClicked(object sender, RoutedEventArgs e)
    {
        // Navigate back on save for mock demonstration
        OnBackClicked(sender, e);
    }
}
