using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace AxioVital.Desktop.Views;

public sealed partial class AnalyticsView : UserControl
{
    private Border? _currentlySelectedItem;

    public AnalyticsView()
    {
        this.InitializeComponent();
        _currentlySelectedItem = OverviewItemBorder;
    }

    private void ToggleSection(StackPanel contentPanel, TextBlock arrowBlock)
    {
        if (contentPanel.Visibility == Visibility.Visible)
        {
            contentPanel.Visibility = Visibility.Collapsed;
            arrowBlock.Text = "▴";
        }
        else
        {
            contentPanel.Visibility = Visibility.Visible;
            arrowBlock.Text = "▾";
        }
    }

    private void OnDashboardsHeaderClicked(object sender, PointerRoutedEventArgs e)
    {
        ToggleSection(DashboardsContent, DashboardsArrow);
    }

    private void OnClinicalAnalyticsHeaderClicked(object sender, PointerRoutedEventArgs e)
    {
        ToggleSection(ClinicalAnalyticsContent, ClinicalAnalyticsArrow);
    }

    private void OnOperationalAnalyticsHeaderClicked(object sender, PointerRoutedEventArgs e)
    {
        ToggleSection(OperationalAnalyticsContent, OperationalAnalyticsArrow);
    }

    private void OnFinancialAnalyticsHeaderClicked(object sender, PointerRoutedEventArgs e)
    {
        ToggleSection(FinancialAnalyticsContent, FinancialAnalyticsArrow);
    }

    private void OnCustomReportsHeaderClicked(object sender, PointerRoutedEventArgs e)
    {
        ToggleSection(CustomReportsContent, CustomReportsArrow);
    }

    private void OnDataManagementHeaderClicked(object sender, PointerRoutedEventArgs e)
    {
        ToggleSection(DataManagementContent, DataManagementArrow);
    }

    private void OnSidebarItemClicked(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border clickedBorder)
        {
            if (_currentlySelectedItem != null && _currentlySelectedItem != clickedBorder)
            {
                _currentlySelectedItem.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
                if (_currentlySelectedItem.Child is TextBlock prevText)
                {
                    prevText.Foreground = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 34, 34, 34));
                    prevText.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
                }
            }

            clickedBorder.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 101, 142));
            if (clickedBorder.Child is TextBlock newText)
            {
                newText.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
                newText.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
            }

            _currentlySelectedItem = clickedBorder;
        }
    }
}
