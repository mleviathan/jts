using Jts.Models.Jira;

namespace Jts.Models;

public class Issue
{
    public string Key { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Assignee { get; set; } = string.Empty;

    public Issue()
    {
    }

    public Issue(JiraIssue issue)
    {
        Key = issue.Key;
        Summary = issue.Fields.Summary;
        Description = issue.Fields.Description;
        Status = issue.Fields.Status?.Name ?? string.Empty;
        Assignee = issue.Fields.Assignee?.DisplayName ?? string.Empty;
    }
}