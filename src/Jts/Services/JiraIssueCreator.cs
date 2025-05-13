using Jts.Services;
using Jts.Models;
using Jts.Models.Jira;
using Jts.Services.HttpClients;

namespace Jts.Services;

public class JiraIssueCreator(IJiraHttpClient jiraHttpClient, string username)
{
    private readonly IJiraHttpClient JiraHttpClient = jiraHttpClient;
    private readonly IJiraFieldTransformationService _fieldTransformationService = new JiraFieldTransformationService();
    private readonly string _username = username;
    private JiraIssue? _jiraIssue;
    private int? _serviceDeskId;
    private int? _requestTypeId;
    private List<RequestTypeField>? _requestTypeFields;

    public async Task Initialize(string issueKey, string projectKey)
    {
        var jiraIssue = await JiraHttpClient.GetIssue(issueKey);
        if (jiraIssue == null)
        {
            Console.WriteLine($"Issue {issueKey} not found.");
            return;
        }
        _jiraIssue = jiraIssue;

        var serviceDeskId = await GetServiceDeskId(projectKey);
        if (serviceDeskId == null)
        {
            Console.WriteLine($"Project {projectKey} not found.");
            return;
        }
        _serviceDeskId = serviceDeskId.Value;

        var requestTypeId = await GetTaskRequestTypeId(serviceDeskId.Value);
        if (requestTypeId == null)
        {
            Console.WriteLine($"Request type 'Task' not found for project {projectKey}.");
            return;
        }
        _requestTypeId = requestTypeId.Value;

        var requestTypeFieldsResponse = await JiraHttpClient.GetServiceDeskRequestTypeFields(serviceDeskId.Value, requestTypeId.Value);
        if (requestTypeFieldsResponse == null)
        {
            Console.WriteLine($"Request type fields not found for project {projectKey}.");
            return;
        }
        _requestTypeFields = requestTypeFieldsResponse.RequestTypeFields;
    }

    public async Task<Issue?> CreateIssue(string projectKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(projectKey, nameof(projectKey));
        ArgumentException.ThrowIfNullOrWhiteSpace(_username, nameof(_username));
        ArgumentNullException.ThrowIfNull(_requestTypeFields, nameof(_requestTypeFields));
        ArgumentNullException.ThrowIfNull(_jiraIssue, nameof(_jiraIssue));
        ArgumentNullException.ThrowIfNull(_fieldTransformationService, nameof(_fieldTransformationService));
        ArgumentNullException.ThrowIfNull(_requestTypeId, nameof(_requestTypeId));
        ArgumentNullException.ThrowIfNull(_serviceDeskId, nameof(_serviceDeskId));
        
        if (_requestTypeFields == null || _requestTypeFields.Count == 0)
        {
            Console.WriteLine($"Request type fields not found for project {projectKey}.");
            return null;
        }

        var requestFieldValues = _fieldTransformationService.ElaborateRequiredFieldsValues(
            _requestTypeFields,
            new Issue(_jiraIssue),
            _username);

        var createIssueRequest = new PostServiceDeskRequest(
            _requestTypeId.Value.ToString(),
            _serviceDeskId.Value.ToString(),
            requestFieldValues);

        var createdIssue = await JiraHttpClient.PostCreateServiceDeskRequest(createIssueRequest);
        if (createdIssue == null)
        {
            Console.WriteLine($"Failed to create issue in ServiceDesk for {_jiraIssue.Key}.");
            return null;
        }

        var issue = new Issue(createdIssue);

        return issue;
    }
    
    /// <summary>
    /// Find the ServiceDesk ID for a given project key.
    /// This is a paginated request, so it will keep fetching until all projects are retrieved or project with given projectKey is found.
    /// </summary>
    /// <param name="projectKey">Key of the project to search</param>
    /// <returns>Null if every project has been checked and given projectKey has not been found</returns>
    internal async Task<int?> GetServiceDeskId(string projectKey)
    {
        GetServiceDeskProjectsResponse? response;
        var start = 0;
        var limit = 50;

        do
        {
            response = await JiraHttpClient.GetServiceDeskProjects(start, limit);
            if (response == null)
            {
                Console.WriteLine("No projects found.");
                return null;
            }

            if (response.Values == null || response.Values.Count == 0)
            {
                Console.WriteLine("No projects found.");
                return null;
            }

            var project = response.Values.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.ProjectKey) && p.ProjectKey.Equals(projectKey, StringComparison.OrdinalIgnoreCase));
            if (project != null && project.Id != null)
            {
                return int.Parse(project.Id);
            }

            start += limit;
        } while (!response.IsLastPage);

        return null;
    }

    /// <summary>
    /// Get the ID of the Task request type for a given project.
    /// </summary>
    /// <param name="projectId">ID obtained by searching the project via <cref="GetServiceDeskId"></param>
    /// <returns>Null if the specified project doesn't have Task as associated request type</returns>
    internal async Task<int?> GetTaskRequestTypeId(int projectId)
    {
        var response = await JiraHttpClient.GetServiceDeskRequestTypes(projectId);
        if (response == null)
        {
            Console.WriteLine("No request types found.");
            return null;
        }
        if (response.Values == null || response.Values.Count == 0)
        {
            Console.WriteLine("No request types found.");
            return null;
        }
        var requestType = response.Values.FirstOrDefault(r => !string.IsNullOrWhiteSpace(r.Name) && r.Name.Equals("Task", StringComparison.OrdinalIgnoreCase));

        return requestType?.Id != null ? int.Parse(requestType.Id) : null;
    }

}