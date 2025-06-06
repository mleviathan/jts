namespace Jts.Services.HttpClients;

public class PostTemporaryFileResult
{
    public List<TemporaryAttachment> TemporaryAttachments { get; set; } = [];
}

public class TemporaryAttachment
{
    public required string TemporaryAttachmentId { get; set; }
    public required string FileName { get; set; }
}