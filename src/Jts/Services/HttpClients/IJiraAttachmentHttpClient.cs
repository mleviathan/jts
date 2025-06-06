namespace Jts.Services.HttpClients;

public interface IJiraAttachmentHttpClient : IBaseHttpClient
{
    /// <summary>
    /// Downloads an attachment from a Jira issue.
    /// </summary>
    /// <param name="attachmentContentUri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<byte[]?> DownloadAttachmentAsync(string attachmentContentUri, CancellationToken cancellationToken = default);
}