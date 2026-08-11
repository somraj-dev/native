namespace AxioVital.Api.Authentication;

public class ApiKeyAuthenticationHandler
{
    public const string HeaderName = "X-Api-Key";

    public bool ValidateApiKey(string apiKey)
    {
        return !string.IsNullOrWhiteSpace(apiKey);
    }
}
