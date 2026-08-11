namespace AxioVital.Desktop.Navigation;

public class NavigationFrame
{
    public Type PageType { get; set; }
    public object? Parameter { get; set; }

    public NavigationFrame(Type pageType, object? parameter = null)
    {
        PageType = pageType;
        Parameter = parameter;
    }
}
