namespace Jts.Models;

public class JiraManagerOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Username => Email.Split('@')[0];

    public JiraManagerOptions(string apiKey, string baseUrl, string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));
        ArgumentException.ThrowIfNullOrEmpty(baseUrl, nameof(baseUrl));
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        ApiKey = apiKey;
        BaseUrl = baseUrl;
        Email = email;
    }
}