namespace Jts.Models.Jira;

public class PostServiceDeskResponse
{
    public required string IssueId { get; set; }
    public required string IssueKey { get; set; }
    public required string RequestTypeId { get; set; }
    public required string ServiceDeskId { get; set; }
    public required Reporter Reporter { get; set; }
    public required List<RequestFieldValue> RequestFieldValues { get; set; }
    public required CurrentStatus CurrentStatus { get; set; }
}

public class Reporter
{
    public required string Name { get; set; }
    public required string Key { get; set; }
    public required string EmailAddress { get; set; }
    public required string DisplayName { get; set; }
}

public class RequestFieldValue
{
    public required string FieldId { get; set; }
    public object? Value { get; set; }
}
public class CurrentStatus
{
    public required string Status { get; set; }
}