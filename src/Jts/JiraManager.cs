using System.Runtime.CompilerServices;
using Jts.Models;
using Jts.Services;
using Jts.Services.HttpClients;

[assembly: InternalsVisibleTo("Jts.Tests")]
namespace Jts;

public class JiraManager : IJiraManager
{
    private readonly JiraManagerOptions _options; 
    internal IJiraHttpClient JiraHttpClient { get; init; }

    /// <summary>
    /// Creates a new instance of the JiraManager class.
    /// </summary>
    /// <param name="apiKey">The API key for Jira.</param>
    /// <param name="baseUrl">The base URL for Jira.</param>
    /// <param name="email">The email address associated with the Jira account.</param>
    /// <exception cref="ArgumentException">Thrown when any of the arguments are null or empty.</exception>
    public JiraManager(JiraManagerOptions jiraManagerOptions)
    {
        ArgumentNullException.ThrowIfNull(jiraManagerOptions);

        var apiKey = jiraManagerOptions.ApiKey;
        var baseUrl = jiraManagerOptions.BaseUrl;
        var email = jiraManagerOptions.Email;

        ArgumentException.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));
        ArgumentException.ThrowIfNullOrEmpty(baseUrl, nameof(baseUrl));
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        JiraHttpClient = new JiraHttpClient(apiKey, email, baseUrl, null);
        _options = jiraManagerOptions;
    }

    public async Task<List<Issue>?> GetIssues()
    {
        var jiraResponse = await JiraHttpClient.GetIssues(_options.Username);
        if (jiraResponse == null)
        {
            Console.WriteLine("No issues found.");
            return null;
        }

        if (jiraResponse.Issues.Count == 0)
        {
            Console.WriteLine("No issues found.");
            return null;
        }

        return [.. jiraResponse.Issues.Select(_ => new Issue(_))];
    }

    public async Task<Issue?> CloneIssue(string issueKey, string projectKey)
    {
        try 
        {
            var issueCreator = new JiraIssueCreator(JiraHttpClient, _options.Username);
            await issueCreator.Initialize(issueKey, projectKey);
            return await issueCreator.CreateIssue(projectKey);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cloning issue: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CheckConnection()
    {
        var status = await JiraHttpClient.HeadIssues();

        switch (status)
        {
            case JiraConnectionStatusEnum.Connected:
                Console.WriteLine("Connection to Jira is working.");
                return true;
            case JiraConnectionStatusEnum.Unauthorized:
                Console.WriteLine("Unauthorized, switching to Bearer token.");
                JiraHttpClient.SetAuthenticationHeaderAsBearer(_options.ApiKey);
                status = await JiraHttpClient.HeadIssues();
                if (status == JiraConnectionStatusEnum.Unauthorized)
                {
                    Console.WriteLine("Still unauthorized, please check your credentials.");
                }
                break;
            default:
                Console.WriteLine("Unknown error, please check your credentials.");
                break;
        }

        return false;
    }

}