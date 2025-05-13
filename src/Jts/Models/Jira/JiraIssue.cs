namespace Jts.Models.Jira;
public class JiraIssue
{
    public string Key { get; set; } = string.Empty;
    public JiraIssueFields Fields { get; set; } = new JiraIssueFields();
}

public class JiraIssueFields
{
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public JiraIssueStatus Status { get; set; } = new JiraIssueStatus();
    public JiraUser? Assignee { get; set; }
    public JiraIssueType IssueType { get; set; } = new JiraIssueType();
    public JiraProject Project { get; set; } = new JiraProject();
    public List<JiraAttachment>? Attachments { get; set; }
}

public class JiraIssueStatus
{
    public string Name { get; set; } = string.Empty;
}

public class JiraUser
{
    public string DisplayName { get; set; } = string.Empty;
}

public class JiraIssueType
{
    public string Name { get; set; } = string.Empty;
}

public class JiraProject
{
    public string Key { get; set; } = string.Empty;
}

public class JiraAttachment
{
    public string Id { get; set; } = string.Empty;
    public string Filename { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}