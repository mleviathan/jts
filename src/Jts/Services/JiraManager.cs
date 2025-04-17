using Jts.Models;
using Jts.Services.HttpClients;

namespace Jts.Services;

public class JiraManager : IJiraManager
{
    public IJiraHttpClient JiraHttpClient { get; init; }

    /// <summary>
    /// Creates a new instance of the JiraManager class.
    /// </summary>
    /// <param name="apiKey">The API key for Jira.</param>
    /// <param name="baseUrl">The base URL for Jira.</param>
    /// <param name="email">The email address associated with the Jira account.</param>
    /// <exception cref="ArgumentException">Thrown when any of the arguments are null or empty.</exception>
    public JiraManager(string apiKey, string baseUrl, string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));
        ArgumentException.ThrowIfNullOrEmpty(baseUrl, nameof(baseUrl));
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        JiraHttpClient = new JiraHttpClient(apiKey, email, null, baseUrl);
    }

    /// <summary>
    /// If you want to pass your custom implementation of IJiraHttpClient, use this constructor.
    /// This is also useful for testing purposes.
    /// </summary>
    /// <param name="httpClient"></param>
    public JiraManager(IJiraHttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        JiraHttpClient = httpClient;
    }

    public async Task<List<Issue>?> GetIssues()
    {
        var jiraResponse = await JiraHttpClient.GetIssues();
        if (jiraResponse == null)
        {
            Console.WriteLine("No issues found.");
            return null;
        }

        if (jiraResponse.Issues == null || jiraResponse.Issues.Count == 0)
        {
            Console.WriteLine("No issues found.");
            return null;
        }

        return [.. jiraResponse.Issues.Select(_ => new Issue(_))];
    }
}