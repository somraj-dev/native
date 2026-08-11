using AxioVital.Desktop.Views;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Windows.System;

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

    public void ToggleFullScreen()
    {
        if (AppWindow.Presenter.Kind == AppWindowPresenterKind.FullScreen)
        {
            AppWindow.SetPresenter(AppWindowPresenterKind.Default);
        }
        else
        {
            AppWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
        }
    }

    public void ExitFullScreen()
    {
        if (AppWindow.Presenter.Kind == AppWindowPresenterKind.FullScreen)
        {
            AppWindow.SetPresenter(AppWindowPresenterKind.Default);
        }
    }

    private void OnRootGridKeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Escape)
        {
            ExitFullScreen();
        }
    }
}
