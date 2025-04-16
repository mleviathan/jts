using Jts.Models.Jira;

public interface IJiraHttpClient : IDisposable
{
    Task<GetJiraIssuesResponse?> GetIssues();
}