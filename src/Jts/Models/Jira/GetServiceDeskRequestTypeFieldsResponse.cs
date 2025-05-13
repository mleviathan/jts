namespace Jts.Models.Jira;

/// <summary>
/// Response from the Jira API for the request types.
/// </summary>
    public class JiraSchema
    {
        public required string Type { get; set; }
        public string? System { get; set; }
        public string? Items { get; set; }
        public string? Custom { get; set; }
        public int? CustomId { get; set; }
    }

    public class RequestTypeField
    {
        public required string FieldId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool Required { get; set; }
        public List<ValidValue>? ValidValues { get; set; }
        public JiraSchema? JiraSchema { get; set; }
    }

    public class GetServiceDeskRequestTypeFieldsResponse
    {
        public List<RequestTypeField>? RequestTypeFields { get; set; }
        public bool CanRaiseOnBehalfOf { get; set; }
        public bool CanAddRequestParticipants { get; set; }
    }

    public class ValidValue
    {
        public required string Value { get; set; }
        public required string Label { get; set; }
        public List<object>? Children { get; set; }
    }

