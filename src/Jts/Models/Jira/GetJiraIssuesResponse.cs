namespace Jts.Models.Jira;

public class GetJiraIssuesResponse
{
    public int StartAt { get; set; }
    public int MaxResults { get; set; }
    public int Total { get; set; }
    public List<JiraIssue> Issues { get; set; } = [];
}