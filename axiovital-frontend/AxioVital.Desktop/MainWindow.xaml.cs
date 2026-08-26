using AxioVital.Desktop.Views;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.System;
using WinRT.Interop;

namespace AxioVital.Desktop;

/// <summary>
/// Main application window host with global keyboard hook for Ctrl+Q Quick Panel.
/// Uses SetWindowsHookEx with WH_KEYBOARD_LL for guaranteed interception from any page/control.
/// </summary>
public sealed partial class MainWindow : Window
{
    private readonly IntPtr _hwnd;
    private IntPtr _keyboardHookId = IntPtr.Zero;
    private readonly LowLevelKeyboardProc _hookProc;

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;
    private const int VK_Q = 0x51;
    private const int VK_ESCAPE = 0x1B;
    private const int VK_F9 = 0x78;
    private const int VK_F10 = 0x79;

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [StructLayout(LayoutKind.Sequential)]
    private struct KBDLLHOOKSTRUCT
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public IntPtr dwExtraInfo;
    }

    public MainWindow()
    {
        this.InitializeComponent();

        _hwnd = WindowNative.GetWindowHandle(this);

        try
        {
            var iconPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "Assets", "app.ico");
            if (System.IO.File.Exists(iconPath))
            {
                this.AppWindow.SetIcon(iconPath);
            }
        }
        catch { }

        // Install global low-level keyboard hook
        _hookProc = HookCallback;
        _keyboardHookId = SetHook(_hookProc);

        // Cleanup hook on window close
        this.Closed += (s, e) =>
        {
            if (_keyboardHookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_keyboardHookId);
                _keyboardHookId = IntPtr.Zero;
            }
        };

        // Launch initial full-screen AxioVital Environment Main View directly
        RootFrame.Navigate(typeof(MainPage));
    }

    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using var curProcess = Process.GetCurrentProcess();
        var curModule = curProcess.MainModule;
        var moduleHandle = curModule != null ? GetModuleHandle(curModule.ModuleName!) : IntPtr.Zero;
        return SetWindowsHookEx(WH_KEYBOARD_LL, proc, moduleHandle, 0);
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
        {
            // Only process when our window is the foreground window
            var foreground = GetForegroundWindow();
            if (foreground == _hwnd)
            {
                var hookStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);

                if (hookStruct.vkCode == VK_Q && (GetAsyncKeyState(0x11) & 0x8000) != 0) // 0x11 = VK_CONTROL
                {
                    this.DispatcherQueue.TryEnqueue(() =>
                    {
                        if (RootFrame.Content is MainPage mainPage)
                        {
                            mainPage.ToggleQuickPanel();
                        }
                    });
                    return (IntPtr)1; // Suppress keystroke
                }
                else if (hookStruct.vkCode == VK_F9)
                {
                    this.DispatcherQueue.TryEnqueue(() =>
                    {
                        if (RootFrame.Content is MainPage mainPage)
                        {
                            mainPage.TogglePatientDetailsPopup();
                        }
                    });
                    return (IntPtr)1; // Suppress default action
                }
                else if (hookStruct.vkCode == VK_F10)
                {
                    this.DispatcherQueue.TryEnqueue(() =>
                    {
                        if (RootFrame.Content is MainPage mainPage)
                        {
                            mainPage.TogglePersonSearch();
                        }
                    });
                    return (IntPtr)1; // Suppress default Windows menu activation
                }
                else if (hookStruct.vkCode == VK_ESCAPE)
                {
                    this.DispatcherQueue.TryEnqueue(() =>
                    {
                        if (RootFrame.Content is MainPage mainPage)
                        {
                            if (mainPage.IsPatientDetailsPopupOpen)
                            {
                                mainPage.ClosePatientDetailsPopup();
                                return;
                            }
                            if (mainPage.IsPersonSearchOpen)
                            {
                                mainPage.ClosePersonSearch();
                                return;
                            }
                            if (mainPage.IsQuickPanelOpen)
                            {
                                mainPage.CloseQuickPanel();
                                return;
                            }
                        }
                    });
                }
            }
        }

        return CallNextHookEx(_keyboardHookId, nCode, wParam, lParam);
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
            if (RootFrame.Content is MainPage mainPage)
            {
                if (mainPage.IsPatientDetailsPopupOpen)
                {
                    mainPage.ClosePatientDetailsPopup();
                    e.Handled = true;
                    return;
                }
                if (mainPage.IsPersonSearchOpen)
                {
                    mainPage.ClosePersonSearch();
                    e.Handled = true;
                    return;
                }
                if (mainPage.IsQuickPanelOpen)
                {
                    mainPage.CloseQuickPanel();
                    e.Handled = true;
                    return;
                }
            }

            ExitFullScreen();

            if (RootFrame.Content is MainPage mp)
            {
                mp.ExitPageFullScreen();
            }
        }
        else if (e.Key == VirtualKey.F9)
        {
            if (RootFrame.Content is MainPage mainPage)
            {
                mainPage.TogglePatientDetailsPopup();
                e.Handled = true;
            }
        }
        else if (e.Key == VirtualKey.F10)
        {
            if (RootFrame.Content is MainPage mainPage)
            {
                mainPage.TogglePersonSearch();
                e.Handled = true;
            }
        }
        else if (e.Key == VirtualKey.Q && Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down))
        {
            if (RootFrame.Content is MainPage mainPage)
            {
                mainPage.ToggleQuickPanel();
                e.Handled = true;
            }
        }
    }
}
