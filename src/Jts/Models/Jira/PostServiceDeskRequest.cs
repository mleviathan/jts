namespace Jts.Models.Jira;

/// <summary>
/// Data transfer object for creating a service desk request
/// </summary>
public class PostServiceDeskRequest
{
    public Dictionary<string, object> RequestFieldValues { get; set; }
    public List<string> RequestParticipants { get; set; }
    public string RequestTypeId { get; set; }
    public string ServiceDeskId { get; set; }

    public PostServiceDeskRequest(string requestTypeId, string serviceDeskId, Dictionary<string, object> requestFieldValues)
    {
        RequestTypeId = requestTypeId;
        ServiceDeskId = serviceDeskId;
        RequestFieldValues = requestFieldValues;
        RequestParticipants = [];
    }
}