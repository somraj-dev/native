using System;

namespace AxioVital.Desktop.Services;

/// <summary>
/// Desktop navigation service interface.
/// </summary>
public interface INavigationService
{
    void NavigateTo(string pageKey, object? parameter = null);
    bool CanGoBack { get; }
    void GoBack();
}

public class NavigationService : INavigationService
{
    public bool CanGoBack => false;

    public void NavigateTo(string pageKey, object? parameter = null)
    {
        // Navigation frame orchestration handled by MainWindow frame
    }

    public void GoBack()
    {
    }
}
