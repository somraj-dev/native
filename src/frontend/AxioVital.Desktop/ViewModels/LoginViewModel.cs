using AxioVital.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace AxioVital.Desktop.ViewModels;

/// <summary>
/// View model for Login page.
/// </summary>
public partial class LoginViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public LoginViewModel(IAuthenticationService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
        Title = "Login";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Email and password are required.";
            return;
        }

        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            var success = await _authService.LoginAsync(Email, Password);
            if (success)
            {
                _navigationService.NavigateTo("Home");
            }
            else
            {
                ErrorMessage = "Invalid credentials or server unavailable.";
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
