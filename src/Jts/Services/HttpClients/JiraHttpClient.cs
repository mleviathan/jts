using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Jts.Models;
using Jts.Models.Jira;

namespace Jts.Services.HttpClients;

public class JiraHttpClient : BaseHttpClient, IJiraHttpClient
{
    public JiraHttpClient(string apiKey,
        string email,
        string baseUrl,
        HttpClient? client = null
        ) : base(apiKey, email, baseUrl, client)
    {
    }

    /// <inheritdoc/>
    public async Task<JiraIssue?> GetIssue(string key)
    {
        try
        {
            var response = await client.GetAsync("/rest/api/2/issue/" + key);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                var issues = JsonSerializer.Deserialize<JiraIssue>(content, Options);
                Console.WriteLine("Received valid issues from Jira.");

                return issues;
            }

            Console.WriteLine($"Error retrieving issues from Jira, received status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<GetJiraIssuesResponse?> GetIssues(string username)
    {
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));

        try
        {
            var response = await client.GetAsync($"/rest/api/2/search?jql=assignee={username}+AND+resolution=Unresolved+AND+issuetype=Task&startAt=0&maxResults=50");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                var issues = JsonSerializer.Deserialize<GetJiraIssuesResponse>(content, Options);
                Console.WriteLine("Received valid issues from Jira.");

                return issues;
            }

            Console.WriteLine($"Error retrieving issues from Jira, received status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<GetServiceDeskProjectsResponse?> GetServiceDeskRequestTypeFields(string projectKey, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.GetAsync($"/rest/api/2/issue/createmeta?projectKeys={projectKey}&issueTypeNames=Task&expand=projects.issuetypes.fields", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                var metadata = JsonSerializer.Deserialize<GetServiceDeskProjectsResponse>(content, Options);

                return metadata;
            }

            Console.WriteLine($"Error retrieving issues from Jira, received status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<GetServiceDeskProjectsResponse?> GetServiceDeskProjects(int start = 0, int limit = 50, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.GetAsync($"/rest/servicedeskapi/servicedesk?start={start}&limit={limit}", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                var metadata = JsonSerializer.Deserialize<GetServiceDeskProjectsResponse>(content, Options);

                return metadata;
            }

            Console.WriteLine($"Error retrieving issues from Jira, received status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<GetServiceDeskRequestTypesResponse?> GetServiceDeskRequestTypes(int projectId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.GetAsync($"/rest/servicedeskapi/servicedesk/{projectId}/requesttype", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                var metadata = JsonSerializer.Deserialize<GetServiceDeskRequestTypesResponse>(content, Options);

                return metadata;
            }

            Console.WriteLine($"Error retrieving issues from Jira, received status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<JiraConnectionStatusEnum> HeadIssues(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, "/rest/api/2/search?startAt=0&maxResults=1"), cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Connection to Jira is valid, received status code: {response.StatusCode}");
                return JiraConnectionStatusEnum.Connected;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine($"Unauthorized error: {response.StatusCode}");
                return JiraConnectionStatusEnum.Unauthorized;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }

        return JiraConnectionStatusEnum.NotConnected;
    }

    /// <inheritdoc/>
    public async Task<PostServiceDeskResponse?> PostCreateServiceDeskRequest(PostServiceDeskRequest issueCreateRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var serializedCreateRequest = JsonSerializer.Serialize(issueCreateRequest, Options);
            Console.WriteLine(serializedCreateRequest);
            var response = await client.PostAsJsonAsync("/rest/servicedeskapi/request", issueCreateRequest, Options, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                Console.WriteLine(content);

                var issue = JsonSerializer.Deserialize<PostServiceDeskResponse>(content, Options);
                Console.WriteLine("Created issue in servicedesk.");

                return issue;
            }

            Console.WriteLine($"Error creating issue in ServiceDesk, received status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }

        return null;
    }

    /// <inheritdoc/>
    public async Task<GetServiceDeskRequestTypeFieldsResponse?> GetServiceDeskRequestTypeFields(int serviceDeskId, int requestTypeId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.GetAsync($"rest/servicedeskapi/servicedesk/{serviceDeskId}/requesttype/{requestTypeId}/field", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync(cancellationToken);
                Console.WriteLine(content);

                var metadata = JsonSerializer.Deserialize<GetServiceDeskRequestTypeFieldsResponse>(content.Result, Options);

                return metadata;
            }

            Console.WriteLine($"Error retrieving issues from Jira, received status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }

        return null;
    }

    public async Task<PostAddAttachmentResponse?> PostAddAttachmentToIssueAsync(string issueKey, PostAddAttachmentRequest addAttachmentRequest, CancellationToken cancellationToken = default)
    {
        try
        {
            var serializedCreateRequest = JsonSerializer.Serialize(addAttachmentRequest, Options);
            Console.WriteLine(serializedCreateRequest);
            var response = await client.PostAsJsonAsync($"/rest/servicedeskapi/request/{issueKey}/attachment", addAttachmentRequest, Options, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                Console.WriteLine(content);

                var issue = JsonSerializer.Deserialize<PostAddAttachmentResponse>(content, Options);
                Console.WriteLine("Created issue in servicedesk.");

                return issue;
            }

            Console.WriteLine($"Error creating issue in ServiceDesk, received status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error: {e.Message}");
        }

        return null;
    }


    public async Task<PostTemporaryFileResult?> UploadTemporaryFileAsync(string serviceDeskId, string filePath, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceDeskId, nameof(serviceDeskId));
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath, nameof(filePath));

        try
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileName = Path.GetFileName(filePath);
            content.Add(new StreamContent(fileStream), "file", fileName);

            // Add required headers, this API is experimental and requires specific headers
            // please see https://support.atlassian.com/jira/kb/how-to-add-an-attachment-to-a-jira-service-management-cloud-ticket-using-the-rest-apis/
            if (!client.DefaultRequestHeaders.Contains("X-Atlassian-Token"))
            {
                client.DefaultRequestHeaders.Add("X-Atlassian-Token", "no-check");
            }

            if (!client.DefaultRequestHeaders.Contains("X-ExperimentalApi"))
            {
                client.DefaultRequestHeaders.Add("X-ExperimentalApi", "true");
            }

            var response = await client.PostAsync($"/rest/servicedeskapi/servicedesk/{serviceDeskId}/attachTemporaryFile", content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var resContent = response.Content.ReadAsStringAsync(cancellationToken);
                Console.WriteLine(JsonSerializer.Serialize(content));

                var metadata = JsonSerializer.Deserialize<PostTemporaryFileResult>(resContent.Result, Options);

                return metadata;
            }

            Console.WriteLine($"Error uploading temporary attachment in ServiceDesk, received status code: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error while uploading temporary file: {e.Message}");
        }

        return null;
    }
}