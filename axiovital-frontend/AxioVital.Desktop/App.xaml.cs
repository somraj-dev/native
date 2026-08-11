using AxioVital.Desktop.Services;
using AxioVital.Desktop.ViewModels;
using AxioVital.Desktop.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AxioVital.Desktop;

/// <summary>
/// Application entry point. Configures DI, logging, and global exception handling.
/// </summary>
public partial class App : Application
{
    private Window? _mainWindow;

    /// <summary>
    /// Gets the service provider for dependency injection.
    /// </summary>
    public static IServiceProvider Services { get; private set; } = null!;

    /// <summary>
    /// Gets the application configuration.
    /// </summary>
    public static IConfiguration Configuration { get; private set; } = null!;

    public App()
    {
        this.InitializeComponent();

        // Global unhandled exception handling
        this.UnhandledException += OnUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
        TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedTaskException;
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        try
        {
            // Build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.File(
                    Path.Combine(AppContext.BaseDirectory, "logs", "axiovital-desktop-.log"),
                    rollingInterval: Serilog.RollingInterval.Day,
                    retainedFileCountLimit: 14)
                .CreateLogger();

            Log.Information("AxioVital Desktop application starting up...");

            // Build DI container
            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();

            // Launch main window
            _mainWindow = new MainWindow();
            _mainWindow.Activate();
            Log.Information("AxioVital Desktop main window activated successfully.");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Critical failure during AxioVital Desktop startup.");
            Log.CloseAndFlush();
            throw;
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Logging
        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(Log.Logger);
        });

        // Configuration
        services.AddSingleton(Configuration);

        // HTTP Client for API communication
        services.AddHttpClient("AxioVitalApi", client =>
        {
            var apiBaseUrl = Configuration["Api:BaseUrl"] ?? "https://localhost:5001";
            client.BaseAddress = new Uri(apiBaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        // Services
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IApiClient, ApiClient>();
        services.AddSingleton<IAuthenticationService, AuthenticationService>();

        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<HomeViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<LoginPageViewModel>();
    }

    private void OnUnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        Log.Fatal(e.Exception, "Unhandled XAML exception in AxioVital Desktop");
        Log.CloseAndFlush();
        e.Handled = true;
    }

    private void OnAppDomainUnhandledException(object sender, System.UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            Log.Fatal(ex, "Unhandled AppDomain exception in AxioVital Desktop");
            Log.CloseAndFlush();
        }
    }

    private void OnTaskSchedulerUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        Log.Error(e.Exception, "Unobserved Task Exception in AxioVital Desktop");
        e.SetObserved();
    }
}
