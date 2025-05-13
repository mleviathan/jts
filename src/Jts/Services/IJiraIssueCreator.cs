using Jts.Models;

namespace Jts.Services;

public interface IJiraIssueCreator
{
    /// <summary>
    /// Initializes the Jira issue creator with the specified issue key and project key.
    /// This method must be called before creating an issue.
    /// </summary>
    /// <param name="issueKey"></param>
    /// <param name="projectKey"></param>
    /// <returns></returns>
    Task Initialize(string issueKey, string projectKey);

    /// <summary>
    /// Creates a new issue in the specified project.
    /// This method requires that Initialize has been called first.
    /// </summary>
    /// <param name="projectKey"></param>
    /// <returns></returns>
    Task<Issue?> CreateIssue(string projectKey);
}