using Jts.HttpClients;
using Jts.Models.Jira;

public interface IJiraManager
{
    Task<List<JiraIssue>?> GetIssues();
}