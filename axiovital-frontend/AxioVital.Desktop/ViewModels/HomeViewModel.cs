using CommunityToolkit.Mvvm.ComponentModel;

namespace AxioVital.Desktop.ViewModels;

/// <summary>
/// View model for Home page.
/// </summary>
public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _welcomeMessage = "Welcome to AxioVital Native";

    public HomeViewModel()
    {
        Title = "Home";
    }
}
