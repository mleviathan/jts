using Jts.Models;
using Jts.Models.Jira;

namespace Jts.Services;

public class JiraFieldTransformationService : IJiraFieldTransformationService
{
    /// <summary>
    /// Elaborate the required fields values for a given request type.
    /// </summary>
    /// <param name="requestFields">The list of request type fields</param>
    /// <param name="parentIssue">The parent issue to clone from</param>
    /// <param name="username">The username for the request</param>
    /// <returns>A dictionary containing the elaborated field values</returns>
    public Dictionary<string, object> ElaborateRequiredFieldsValues(List<RequestTypeField> requestFields, Issue parentIssue, string username)
    {
        // Some fields might have a default value and not have any desired value by the user.
        // In this cases, it's possible to use default values.
        Tuple<string, object> ElaborateDefaultFields(RequestTypeField field) {
            ArgumentNullException.ThrowIfNull(field.ValidValues);
            ArgumentNullException.ThrowIfNull(field.JiraSchema);

            // Arrays must be formatted as a dictionary with the name of the field and the value as a list of objects with the name of the value
            // Example: "fieldName": [{"name": "value"}]
            if (field.JiraSchema.Type.Equals("array", StringComparison.OrdinalIgnoreCase))
            {
                var fieldName = field.FieldId;
                var fieldValue = new List<Dictionary<string, string>>();
                foreach (var value in field.ValidValues)
                {
                    fieldValue.Add(new Dictionary<string, string> { { "name", value.Label } });
                }
                return new Tuple<string, object>(fieldName, fieldValue);
            }

            if (field.ValidValues.Count > 0)
            {
                var fieldName = field.FieldId;
                var fieldValue = field.ValidValues.First().Value;
                return new Tuple<string, object>(fieldName, fieldValue);
            }

            throw new Exception(field.Name + " has no default value.");
        }

        var requiredFields = new Dictionary<string, object>();
        foreach (var field in requestFields)
        {
            // Skip non-required fields
            if (!field.Required)
            {
                continue;
            }

            if (field.Name.Equals("Parent Issue", StringComparison.OrdinalIgnoreCase))
            {
                requiredFields.Add(field.FieldId, parentIssue.Key);
            }
            else if (field.Name.Equals("Referente", StringComparison.OrdinalIgnoreCase))
            {
                requiredFields.Add(field.FieldId, new Dictionary<string, string> { { "name", username } });
            }
            else if (field.Name.Equals("Description", StringComparison.OrdinalIgnoreCase))
            {
                requiredFields.Add(field.FieldId, "Cloned from " + parentIssue.Key + " by JTS \r\n\r\n " + parentIssue.Description);
            }
            else if (field.Name.Equals("Summary", StringComparison.OrdinalIgnoreCase))
            {
                requiredFields.Add(field.FieldId, parentIssue.Summary);
            }
            else
            {
                var fieldValue = ElaborateDefaultFields(field);
                requiredFields.Add(fieldValue.Item1, fieldValue.Item2);
            }
        }

        return requiredFields;
    }
}
