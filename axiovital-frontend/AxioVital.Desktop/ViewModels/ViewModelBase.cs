using CommunityToolkit.Mvvm.ComponentModel;

namespace AxioVital.Desktop.ViewModels;

/// <summary>
/// Base view model providing INotifyPropertyChanged via CommunityToolkit.Mvvm.
/// </summary>
public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;
}
