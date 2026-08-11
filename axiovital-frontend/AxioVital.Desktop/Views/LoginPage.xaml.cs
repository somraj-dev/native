using AxioVital.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace AxioVital.Desktop.Views;

/// <summary>
/// Login screen page for AxioVital Environment.
/// </summary>
public sealed partial class LoginPage : Page
{
    public LoginPageViewModel ViewModel { get; }

    public LoginPage()
    {
        this.InitializeComponent();

        // Resolve ViewModel from Dependency Injection
        ViewModel = App.Services.GetRequiredService<LoginPageViewModel>();
        this.DataContext = ViewModel;

        ViewModel.OnLoginSuccess = () =>
        {
            // Navigate to main application page (MainPage) upon successful login
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        };
    }

    private void PasswordInput_PasswordChanged(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (sender is PasswordBox pBox && ViewModel != null)
        {
            ViewModel.Password = pBox.Password;
        }
    }
}
