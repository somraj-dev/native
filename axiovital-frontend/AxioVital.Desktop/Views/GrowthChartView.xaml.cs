using System;
using System.Collections.Generic;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace AxioVital.Desktop.Views;

public sealed partial class GrowthChartView : UserControl
{
    private string _activePortfolio = "All";
    private string _activeCurrency = "Original";

    public GrowthChartView()
    {
        this.InitializeComponent();
    }

    private void OnPortfolioPillPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b && b.Tag is string tag)
        {
            _activePortfolio = tag;
            ResetPortfolioPills();

            // Set active pill
            b.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 22, 91, 140));
            b.BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 67, 104));
            if (b.Child is TextBlock tb)
            {
                tb.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
                tb.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;
            }
        }
    }

    private void ResetPortfolioPills()
    {
        var inactiveBg = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 203, 220, 235));
        var inactiveBorder = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 157, 186, 214));
        var inactiveText = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 58, 95));

        var pills = new[] { PillHers, PillHersTax, PillHis, PillHisCdn, PillHisTax, PillJoint };
        foreach (var pill in pills)
        {
            if (pill != null)
            {
                pill.Background = inactiveBg;
                pill.BorderBrush = inactiveBorder;
                if (pill.Child is TextBlock tb)
                {
                    tb.Foreground = inactiveText;
                    tb.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
                }
            }
        }
    }

    private void OnCurrencyPillPressed(object sender, PointerRoutedEventArgs e)
    {
        if (sender is Border b && b.Tag is string tag)
        {
            _activeCurrency = tag;
            ResetCurrencyPills();

            // Set active pill
            b.Background = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 22, 91, 140));
            b.BorderBrush = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 15, 67, 104));
            if (b.Child is TextBlock tb)
            {
                tb.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
                tb.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;
            }
        }
    }

    private void ResetCurrencyPills()
    {
        var inactiveBg = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 203, 220, 235));
        var inactiveBorder = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 157, 186, 214));
        var inactiveText = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 30, 58, 95));

        var pills = new[] { PillOrigCurrency, PillCadCurrency, PillUsdCurrency };
        foreach (var pill in pills)
        {
            if (pill != null)
            {
                pill.Background = inactiveBg;
                pill.BorderBrush = inactiveBorder;
                if (pill.Child is TextBlock tb)
                {
                    tb.Foreground = inactiveText;
                    tb.FontWeight = Microsoft.UI.Text.FontWeights.Normal;
                }
            }
        }
    }
}
