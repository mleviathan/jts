using System.Net.Http.Headers;
using System.Text.Json;
using Jts.Models.Jira;

namespace Jts.Services.HttpClients;

public class JiraHttpClient : IJiraHttpClient
{
    private readonly HttpClient client;
    private readonly JsonSerializerOptions options;

    public JiraHttpClient(string apiKey,
        string email,
        HttpClient? client = null,
        string baseUrl = "https://leviathancode.atlassian.net")
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
        this.options = new JsonSerializerOptions(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
        this.client = client;
    }

    /// <inheritdoc/>
    public async Task<GetJiraIssuesResponse?> GetIssues()
    {
        try
        {
            var response = await client.GetAsync("/rest/api/2/search?startAt=0&maxResults=50");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                var issues = JsonSerializer.Deserialize<GetJiraIssuesResponse>(content, options);
                Console.WriteLine("Received valid issues from Jira.");

                return issues;
            }

            Console.WriteLine($"Error retrieving issues from Jira, received status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }


        return null;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    private AuthenticationHeaderValue CreateBasicAuthHeader(string apiKey, string email)
    {
        var byteArray = System.Text.Encoding.ASCII.GetBytes(email + ":" + apiKey);
        return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }
}