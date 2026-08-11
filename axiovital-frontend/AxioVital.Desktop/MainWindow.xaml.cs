using AxioVital.Desktop.Views;
using Microsoft.UI.Xaml;

namespace AxioVital.Desktop;

/// <summary>
/// Main application window host.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();

        // Launch initial full-screen AxioVital Environment Login Screen
        RootFrame.Navigate(typeof(LoginPage));
    }
}
