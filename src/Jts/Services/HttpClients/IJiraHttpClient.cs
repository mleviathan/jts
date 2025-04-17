using Jts.Models.Jira;

namespace Jts.Services.HttpClients;

public interface IJiraHttpClient : IDisposable
{
    /// <summary>
    /// Gets the issues from Jira.
    /// Currently it gets the first 50 issues.
    /// </summary>
    /// <returns></returns>
    Task<GetJiraIssuesResponse?> GetIssues();
}