using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace AxioVital.Desktop.Views;

public sealed partial class PhysicianHandoffView : UserControl
{
    public PhysicianHandoffView()
    {
        this.InitializeComponent();
    }

    private void OnTableRadioPressed(object sender, PointerRoutedEventArgs e)
    {
        SetRadioState("Table");
    }

    private void OnGroupRadioPressed(object sender, PointerRoutedEventArgs e)
    {
        SetRadioState("Group");
    }

    private void OnListRadioPressed(object sender, PointerRoutedEventArgs e)
    {
        SetRadioState("List");
    }

    private void SetRadioState(string selected)
    {
        var activeBlue = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 11, 68, 102));
        var inactiveGray = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 119, 119, 119));
        var darkText = new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 17, 17, 17));

        if (TableRadioDot != null && TableRadioRing != null && TableRadioText != null)
        {
            bool isTable = (selected == "Table");
            TableRadioDot.Visibility = isTable ? Visibility.Visible : Visibility.Collapsed;
            TableRadioRing.Stroke = isTable ? activeBlue : inactiveGray;
            TableRadioRing.StrokeThickness = isTable ? 1.5 : 1.2;
            TableRadioText.Foreground = isTable ? activeBlue : darkText;
            TableRadioText.FontWeight = isTable ? Microsoft.UI.Text.FontWeights.SemiBold : Microsoft.UI.Text.FontWeights.Normal;
        }

        if (GroupRadioDot != null && GroupRadioRing != null && GroupRadioText != null)
        {
            bool isGroup = (selected == "Group");
            GroupRadioDot.Visibility = isGroup ? Visibility.Visible : Visibility.Collapsed;
            GroupRadioRing.Stroke = isGroup ? activeBlue : inactiveGray;
            GroupRadioRing.StrokeThickness = isGroup ? 1.5 : 1.2;
            GroupRadioText.Foreground = isGroup ? activeBlue : darkText;
            GroupRadioText.FontWeight = isGroup ? Microsoft.UI.Text.FontWeights.SemiBold : Microsoft.UI.Text.FontWeights.Normal;
        }

        if (ListRadioDot != null && ListRadioRing != null && ListRadioText != null)
        {
            bool isList = (selected == "List");
            ListRadioDot.Visibility = isList ? Visibility.Visible : Visibility.Collapsed;
            ListRadioRing.Stroke = isList ? activeBlue : inactiveGray;
            ListRadioRing.StrokeThickness = isList ? 1.5 : 1.2;
            ListRadioText.Foreground = isList ? activeBlue : darkText;
            ListRadioText.FontWeight = isList ? Microsoft.UI.Text.FontWeights.SemiBold : Microsoft.UI.Text.FontWeights.Normal;
        }
    }
}
