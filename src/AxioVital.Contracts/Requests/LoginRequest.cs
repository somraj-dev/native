namespace AxioVital.Contracts.Requests;

/// <summary>
/// Request to authenticate a user with email and password.
/// </summary>
public sealed class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
