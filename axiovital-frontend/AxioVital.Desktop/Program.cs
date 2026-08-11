using System;
using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.DynamicDependency;

namespace AxioVital.Desktop;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        WinRT.ComWrappersSupport.InitializeComWrappers();

        // Initialize Windows App SDK Bootstrap for unpackaged WinUI 3 execution
        try
        {
            Bootstrap.Initialize(0x00010006);
        }
        catch
        {
            // Fallback for self-contained runtime environment
        }

        Application.Start((p) =>
        {
            var context = new Microsoft.UI.Dispatching.DispatcherQueueSynchronizationContext(
                Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
            System.Threading.SynchronizationContext.SetSynchronizationContext(context);
            _ = new App();
        });
    }
}
