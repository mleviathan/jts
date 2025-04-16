using Jts.HttpClients;
using Jts.Models.Jira;

namespace Jts.Services;

public class JiraManager : IJiraManager
{
    private readonly IJiraHttpClient jiraHttpClient;

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

        jiraHttpClient = new JiraHttpClient(apiKey, email, null, baseUrl);
    }

    /// <summary>
    /// If you want to pass your custom implementation of IJiraHttpClient, use this constructor.
    /// This is also useful for testing purposes.
    /// </summary>
    /// <param name="httpClient"></param>
    public JiraManager(IJiraHttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        jiraHttpClient = httpClient;
    }

    public async Task<List<JiraIssue>?> GetIssues()
    {
        var jiraResponse = await jiraHttpClient.GetIssues();
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

        return jiraResponse.Issues;
    }
}