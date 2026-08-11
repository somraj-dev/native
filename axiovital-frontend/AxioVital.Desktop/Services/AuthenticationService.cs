using AxioVital.Contracts.Requests;
using AxioVital.Contracts.Responses;
using System.Threading.Tasks;

namespace AxioVital.Desktop.Services;

/// <summary>
/// Authentication service handling login, token storage abstraction, and session state.
/// </summary>
public interface IAuthenticationService
{
    bool IsAuthenticated { get; }
    string? CurrentToken { get; }
    AuthResponse? CurrentUser { get; }

    Task<bool> LoginAsync(string email, string password);
    void Logout();
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IApiClient _apiClient;

    public bool IsAuthenticated { get; private set; }
    public string? CurrentToken { get; private set; }
    public AuthResponse? CurrentUser { get; private set; }

    public AuthenticationService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var request = new LoginRequest { Email = email, Password = password };
        var result = await _apiClient.PostAsync<LoginRequest, AuthResponse>("api/v1/auth/login", request);

        if (result != null && !string.IsNullOrEmpty(result.AccessToken))
        {
            CurrentToken = result.AccessToken;
            CurrentUser = result;
            IsAuthenticated = true;
            _apiClient.SetAuthToken(result.AccessToken);
            return true;
        }

        return false;
    }

    public void Logout()
    {
        CurrentToken = null;
        CurrentUser = null;
        IsAuthenticated = false;
        _apiClient.ClearAuthToken();
    }
}
