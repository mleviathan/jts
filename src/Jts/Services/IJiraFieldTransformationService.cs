using Jts.Models;
using Jts.Models.Jira;

namespace Jts.Services;

public interface IJiraFieldTransformationService
{
    /// <summary>
    /// Transforms the request type fields into a dictionary of field values for the service desk request.
    /// </summary>
    /// <param name="requestTypeFields">The request type fields.</param>
    /// <param name="parentIssue">The parent issue.</param>
    /// <param name="username">The username of the user creating the request.</param>
    /// <returns>A dictionary of field values for the service desk request.</returns>
    Dictionary<string, object> ElaborateRequiredFieldsValues(
        List<RequestTypeField> requestTypeFields,
        Issue parentIssue,
        string username);
}