using Jts.Models;

namespace Jts;

/// <summary>
/// Interface to manage Jira and ServiceDesk issues.
/// </summary>
public interface IJiraManager
{
    /// <summary>
    /// Get the list of issues from Jira assigned to the user.
    /// </summary>
    /// <returns></returns>
    Task<List<Issue>?> GetIssues();

    /// <summary>
    /// Get the specific issue from Jira by its key then clone it in service desk.
    /// </summary>
    /// <param name="issueKey">Key of the issue to clone, usually formatted as {project_short}-{number} ex: SUP-1</param>
    /// <param name="projectKey">Key of the target project where issue should be cloned into ex: SUP</param>
    /// <returns>The issue cloned in service desk</returns>
    Task<Issue?> CloneIssue(string issueKey,
                            string projectKey);

    /// <summary>
    /// Execute a test to check if the connection to Jira is working.
    /// If an exception of type <cref name="HttpRequestUnauthorizedException"/> is thrown, the authorization header will be switched from Basic to Bearer
    /// using the apiKey as token.
    /// </summary>
    /// <returns></returns>
    Task<bool> CheckConnection();
}