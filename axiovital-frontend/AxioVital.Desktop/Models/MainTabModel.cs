using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AxioVital.Desktop.Models;

public class MainTabModel : INotifyPropertyChanged
{
    private string _id = string.Empty;
    private string _title = string.Empty;
    private string _headerTitle = string.Empty;
    private bool _isActive = false;
    private bool _isCloseable = true;

    public string Id
    {
        get => _id;
        set { _id = value; OnPropertyChanged(); }
    }

    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); }
    }

    public string HeaderTitle
    {
        get => _headerTitle;
        set { _headerTitle = value; OnPropertyChanged(); }
    }

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TabBackground));
            OnPropertyChanged(nameof(TextColor));
            OnPropertyChanged(nameof(TextWeight));
        }
    }

    public bool IsCloseable
    {
        get => _isCloseable;
        set
        {
            _isCloseable = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CloseButtonVisibility));
        }
    }

    public Brush TabBackground => IsActive
        ? new SolidColorBrush(Microsoft.UI.Colors.White)
        : new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 230, 238, 242));

    public Brush TextColor => new SolidColorBrush(Microsoft.UI.ColorHelper.FromArgb(255, 10, 63, 92));

    public Windows.UI.Text.FontWeight TextWeight => IsActive
        ? Microsoft.UI.Text.FontWeights.Bold
        : Microsoft.UI.Text.FontWeights.Normal;

    public Visibility CloseButtonVisibility => IsCloseable ? Visibility.Visible : Visibility.Collapsed;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
