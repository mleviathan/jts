namespace Jts.Models.Jira;

public class PostAddAttachmentRequest(List<string> temporaryAttachmentIds, bool isPublic = true)
{
    public List<string> TemporaryAttachmentIds { get; set; } = temporaryAttachmentIds;
    public bool Public { get; set; } = isPublic;
}
