using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("Jts.Tests")]
namespace Jts.Services.HttpClients;

public class BaseHttpClient
{
    internal protected readonly HttpClient client;

    private protected JsonSerializerOptions Options { get; } = new JsonSerializerOptions(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public BaseHttpClient(string apiKey,
        string email,
        string baseUrl,
        HttpClient? client = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));
        ArgumentException.ThrowIfNullOrEmpty(baseUrl, nameof(baseUrl));

        _ = Uri.TryCreate(baseUrl, UriKind.Absolute, out var uri);
        if (uri is null)
        {
            throw new UriFormatException("Invalid baseUrl: + " + uri);
        }

        client ??= new HttpClient();
        client.BaseAddress = uri;
        client.DefaultRequestHeaders.Authorization = CreateBasicAuthHeader(apiKey, email);
        this.client = client;
    }

    internal static AuthenticationHeaderValue CreateBasicAuthHeader(string apiKey, string email)
    {
        var byteArray = System.Text.Encoding.ASCII.GetBytes(email + ":" + apiKey);
        return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }
}