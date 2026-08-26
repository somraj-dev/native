using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;

namespace AxioVital.Desktop.Views;

/// <summary>
/// 1:1 Replica Notification View / Message Centre Reminders workspace for Level 1 Top Ribbon navigation.
/// </summary>
public sealed partial class NotificationsView : UserControl
{
    private Border? _selectedRowBorder;
    private Border? _selectedTreeBorder;

    public NotificationsView()
    {
        this.InitializeComponent();
        _selectedRowBorder = Row1Border;
        _selectedTreeBorder = TreeRemindersBorder;
    }



    private void OnSidebarTabInboxPressed(object sender, PointerRoutedEventArgs e)
    {
        SelectSidebarTab(TabInboxBorder, TabInboxText);
        UnselectSidebarTab(TabProxiesBorder, TabProxiesText);
        UnselectSidebarTab(TabPoolsBorder, TabPoolsText);
    }

    private void OnSidebarTabProxiesPressed(object sender, PointerRoutedEventArgs e)
    {
        SelectSidebarTab(TabProxiesBorder, TabProxiesText);
        UnselectSidebarTab(TabInboxBorder, TabInboxText);
        UnselectSidebarTab(TabPoolsBorder, TabPoolsText);
    }

    private void OnSidebarTabPoolsPressed(object sender, PointerRoutedEventArgs e)
    {
        SelectSidebarTab(TabPoolsBorder, TabPoolsText);
        UnselectSidebarTab(TabInboxBorder, TabInboxText);
        UnselectSidebarTab(TabProxiesBorder, TabProxiesText);
    }

    private void SelectSidebarTab(Border border, TextBlock text)
    {
        border.Background = new SolidColorBrush(Colors.White);
        text.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;
        text.Foreground = new SolidColorBrush(ColorHelper.FromArgb(255, 15, 23, 42));
    }

    private void UnselectSidebarTab(Border border, TextBlock text)
    {
        border.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 226, 232, 240));
        text.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
        text.Foreground = new SolidColorBrush(ColorHelper.FromArgb(255, 71, 85, 105));
    }

    private void OnDisplayRangeChanged(object sender, SelectionChangedEventArgs e)
    {
        // Filter by date range
    }

    private void OnFilterDetailsClicked(object sender, RoutedEventArgs e)
    {
        // Filter dialog
    }

    private void OnTreeCategoryPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border border)
        {
            // Reset previous selected tree border
            if (_selectedTreeBorder != null)
            {
                _selectedTreeBorder.Background = new SolidColorBrush(Colors.Transparent);
                if (_selectedTreeBorder.Child is TextBlock prevTb)
                {
                    prevTb.Foreground = new SolidColorBrush(ColorHelper.FromArgb(255, 30, 41, 59));
                    prevTb.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
                }
                else if (_selectedTreeBorder.Child is StackPanel sp && sp.Children.Count > 1 && sp.Children[1] is TextBlock spTb)
                {
                    spTb.Foreground = new SolidColorBrush(ColorHelper.FromArgb(255, 30, 41, 59));
                    spTb.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
                }
            }

            _selectedTreeBorder = border;
            border.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 0, 120, 215)); // #0078D7
            if (border.Child is TextBlock tb)
            {
                tb.Foreground = new SolidColorBrush(Colors.White);
                tb.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;
            }
            else if (border.Child is StackPanel sp2 && sp2.Children.Count > 1 && sp2.Children[1] is TextBlock spTb2)
            {
                spTb2.Foreground = new SolidColorBrush(Colors.White);
                spTb2.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;
            }

            var category = border.Tag?.ToString() ?? "Reminders";
            if (ActiveTabTitleText != null)
            {
                ActiveTabTitleText.Text = category;
            }
        }
    }

    private void OnRowPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border row)
        {
            // Clear previous row highlight
            if (_selectedRowBorder != null && _selectedRowBorder != row)
            {
                _selectedRowBorder.BorderBrush = new SolidColorBrush(ColorHelper.FromArgb(255, 241, 245, 249));
                _selectedRowBorder.BorderThickness = new Thickness(0, 0, 0, 1);
                var prevTag = _selectedRowBorder.Tag?.ToString();
                _selectedRowBorder.Background = (prevTag == "4")
                    ? new SolidColorBrush(ColorHelper.FromArgb(255, 244, 247, 251))
                    : new SolidColorBrush(Colors.White);
            }

            _selectedRowBorder = row;
            row.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 229, 241, 251)); // #E5F1FB
            row.BorderBrush = new SolidColorBrush(ColorHelper.FromArgb(255, 0, 120, 215));  // #0078D7
            row.BorderThickness = new Thickness(1);

            // Enable action toolbar buttons for selected item
            EnableActionButtons(true);
        }
    }

    private void EnableActionButtons(bool enable)
    {
        if (OpenBtn != null) OpenBtn.IsEnabled = enable;
        if (ReplyBtn != null) ReplyBtn.IsEnabled = enable;
        if (ReplyAllBtn != null) ReplyAllBtn.IsEnabled = enable;
        if (RedirectBtn != null) RedirectBtn.IsEnabled = enable;
        if (RescheduleBtn != null) RescheduleBtn.IsEnabled = enable;
        if (CompleteBtn != null) CompleteBtn.IsEnabled = enable;
        if (SelectPatientBtn != null) SelectPatientBtn.IsEnabled = enable;
    }

    private void OnCommunicateClicked(object sender, RoutedEventArgs e)
    {
    }

    private void OnNewNotificationClicked(object sender, RoutedEventArgs e)
    {
    }

    private void OnSendNotificationClicked(object sender, RoutedEventArgs e)
    {
    }

    private void OnSendReminderClicked(object sender, RoutedEventArgs e)
    {
    }

    private void OnNotificationJournalClicked(object sender, RoutedEventArgs e)
    {
    }

    private void OnSelectAllClicked(object sender, RoutedEventArgs e)
    {
        EnableActionButtons(true);
    }
}
