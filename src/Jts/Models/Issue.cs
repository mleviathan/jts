using Jts.Models.Jira;

namespace Jts.Models;

public class Issue
{
    public string Key { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Assignee { get; set; } = string.Empty;
    public string IssueType { get; set; } = string.Empty;
    public string ProjectKey { get; set; } = string.Empty;
    public List<string>? AttachmentsContentUris { get; set; }

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
        ProjectKey = issue.Fields.Project?.Key ?? string.Empty;
        AttachmentsContentUris = issue.Fields.Attachment?.Select(a => a.Content).ToList();
    }

    public Issue(PostServiceDeskResponse postCreateServiceDeskRequest)
    {
        Key = postCreateServiceDeskRequest.IssueKey;
        Summary = postCreateServiceDeskRequest.RequestFieldValues.FirstOrDefault(r => r.FieldId == "summary")?.Value?.ToString() ?? string.Empty;
        Description = postCreateServiceDeskRequest.RequestFieldValues.FirstOrDefault(r => r.FieldId == "description")?.Value?.ToString() ?? string.Empty;
        Status = postCreateServiceDeskRequest.CurrentStatus.Status ?? string.Empty;
        Assignee = postCreateServiceDeskRequest.Reporter?.DisplayName ?? string.Empty;
        ProjectKey = postCreateServiceDeskRequest.IssueId; // Assuming IssueId is the project key
        AttachmentsContentUris = null; // Attachments are not included in this model
    }
}