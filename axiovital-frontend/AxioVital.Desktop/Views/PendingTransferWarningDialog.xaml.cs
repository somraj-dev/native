using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace AxioVital.Desktop.Views;

/// <summary>
/// Warning dialog for Cancel Pending Transfer, Facility Transfer, and Transfer workflows.
/// </summary>
public sealed partial class PendingTransferWarningDialog : UserControl
{
    public event EventHandler? YesClicked;
    public event EventHandler? NoClicked;
    public event EventHandler? CancelClicked;
    public event EventHandler? CloseRequested;

    public PendingTransferWarningDialog()
    {
        this.InitializeComponent();
    }

    public void SetDialogMode(string title, string? message = null)
    {
        if (DialogTitleText != null)
        {
            DialogTitleText.Text = title;
        }

        if (WarningMessageText != null && !string.IsNullOrWhiteSpace(message))
        {
            WarningMessageText.Text = message;
        }
    }

    private void OnYesClicked(object sender, RoutedEventArgs e)
    {
        YesClicked?.Invoke(this, EventArgs.Empty);
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnNoClicked(object sender, RoutedEventArgs e)
    {
        NoClicked?.Invoke(this, EventArgs.Empty);
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnCancelClicked(object sender, RoutedEventArgs e)
    {
        CancelClicked?.Invoke(this, EventArgs.Empty);
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }
}
