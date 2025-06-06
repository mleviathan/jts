namespace Jts.Models.Jira;

public class PostAddAttachmentResponse
{
    public List<JiraAttachment> Attachments { get; set; } = new List<JiraAttachment>();
}