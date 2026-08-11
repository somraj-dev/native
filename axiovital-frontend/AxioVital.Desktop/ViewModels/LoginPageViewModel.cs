using AxioVital.Desktop.Models;
using AxioVital.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AxioVital.Desktop.ViewModels;

public partial class LoginPageViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string _selectedUser = "Select User...";

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _selectedDomain = "PROD";

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private bool _isLoading;

    public ObservableCollection<string> Users { get; } = new(LoginDomainModel.DefaultUsers);
    public ObservableCollection<string> Domains { get; } = new(LoginDomainModel.DefaultDomains);

    public System.Action? OnLoginSuccess { get; set; }

    public LoginPageViewModel(IAuthenticationService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
        Title = "AxioVital Environment Login";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (SelectedUser == "Select User..." || string.IsNullOrWhiteSpace(SelectedUser))
        {
            HasError = true;
            StatusMessage = "Please select a valid User Name.";
            return;
        }

        IsLoading = true;
        HasError = false;
        StatusMessage = "Authenticating with AxioVital Environment...";

        try
        {
            // Simulate / execute authentication
            await Task.Delay(600);

            IsLoading = false;
            StatusMessage = "Login Successful!";

            OnLoginSuccess?.Invoke();
        }
        catch (System.Exception ex)
        {
            IsLoading = false;
            HasError = true;
            StatusMessage = $"Login failed: {ex.Message}";
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        SelectedUser = "Select User...";
        Password = string.Empty;
        SelectedDomain = "PROD";
        HasError = false;
        StatusMessage = string.Empty;
    }
}
