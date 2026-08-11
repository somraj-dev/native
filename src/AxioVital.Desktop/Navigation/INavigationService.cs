namespace AxioVital.Desktop.Navigation;

public interface INavigationService
{
    void NavigateTo(Type pageType, object? parameter = null);
    void NavigateTo<TPage>(object? parameter = null) where TPage : class;
    bool GoBack();
    bool CanGoBack { get; }
}
