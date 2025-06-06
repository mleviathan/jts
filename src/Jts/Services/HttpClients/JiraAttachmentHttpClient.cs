namespace Jts.Services.HttpClients;

public class JiraAttachmentHttpClient : BaseHttpClient, IJiraAttachmentHttpClient
{
    public JiraAttachmentHttpClient(string apiKey, string email, HttpClient? client = null)
        : base(apiKey, email, null, client)
    {
    }


    public async Task<byte[]?> DownloadAttachmentAsync(string attachmentContentUri, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(attachmentContentUri, nameof(attachmentContentUri));

        try
        {
            var response = await client.GetAsync(attachmentContentUri, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);
                Console.WriteLine("Received valid attachment from Jira.");

                return content;
            }

            Console.WriteLine($"Error retrieving issues from Jira, received status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }

        return null;
    }
}