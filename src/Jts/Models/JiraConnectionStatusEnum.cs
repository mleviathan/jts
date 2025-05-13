namespace Jts.Models;

public enum JiraConnectionStatusEnum
{
    /// <summary>
    /// The connection to Jira is working.
    /// </summary>
    Connected = 0,

    /// <summary>
    /// The connection to Jira is not working.
    /// </summary>
    NotConnected = 1,

    /// <summary>
    /// The connection to Jira is not authorized.
    /// </summary>
    Unauthorized = 2,
}