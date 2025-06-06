using System.Runtime.CompilerServices;
using Jts.Models;
using Jts.Models.Jira;

[assembly: InternalsVisibleTo("Jts.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Jts.Services.HttpClients;

/// <summary>
/// Implementation of an HttpClient designed for calls to Jira.
/// </summary>
public interface IJiraHttpClient : IBaseHttpClient
{
    /// <summary>
    /// Gets the issues from Jira.
    /// Currently it gets the first 50 issues.
    /// </summary>
    /// <param name="username">Username of the user to get issues from</param>
    /// <returns></returns>
    Task<GetJiraIssuesResponse?> GetIssues(string username);

    /// <summary>
    /// Gets a specific issue from Jira.
    /// </summary>
    /// <param name="key">Key of the issue to obtain</param>
    /// <returns>Null if the issue was not found or user doesn't have permission to see it</returns>
    Task<JiraIssue?> GetIssue(string key);

    /// <summary>
    /// Creates a new issue in Servicedesk.
    /// </summary>
    /// <returns></returns>
    Task<PostServiceDeskResponse?> PostCreateServiceDeskRequest(PostServiceDeskRequest issueCreateRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the metadata for a specific request type.
    /// This returns all the fields required to create a request in service desk.
    /// </summary>
    /// <param name="requestTypeId">ID of the request type to fetch data from</param>
    /// <param name="serviceDeskId">ID of the project to fetch data from</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetServiceDeskRequestTypeFieldsResponse?> GetServiceDeskRequestTypeFields(int serviceDeskId, int requestTypeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Execute an Head Issues request to check if the connection to Jira is working.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>A string </returns>
    Task<JiraConnectionStatusEnum> HeadIssues(CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetch ServiceDesk projects from Jira.
    /// This is a paginated request, so you need to call it multiple times to get all the projects.
    /// https://developer.atlassian.com/cloud/jira/service-desk/rest/api-group-servicedesk/#api-rest-servicedeskapi-servicedesk-get
    /// </summary>
    /// <param name="start">The starting index of the returned objects. Base index: 0.</param>
    /// <param name="limit">The maximum number of items to return per page. Default: 50</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetServiceDeskProjectsResponse?> GetServiceDeskProjects(int start = 0, int limit = 50, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the request types for a specific project.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetServiceDeskRequestTypesResponse?> GetServiceDeskRequestTypes(int projectId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads an attachment to a Jira service desk request.
    /// </summary>
    /// <param name="serviceDeskId"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    Task<PostTemporaryFileResult?> UploadTemporaryFileAsync(string serviceDeskId, string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds an attachment to a Jira issue using the temporary attachment ID.
    /// This is used after uploading a file to Jira using the UploadTemporaryFileAsync method.
    /// </summary>
    /// <param name="issueKey"></param>
    /// <param name="temporaryAttachmentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PostAddAttachmentResponse?> PostAddAttachmentToIssueAsync(string issueKey, PostAddAttachmentRequest addAttachmentRequest, CancellationToken cancellationToken = default);
}