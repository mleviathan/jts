using System.Text.Json.Serialization;

namespace Jts.Models.Jira;

public class GetServiceDeskCommonResponse
{
    public int Size { get; set; }
    public int Start { get; set; }
    public int Limit { get; set; }
    public bool IsLastPage { get; set; }

    [JsonPropertyName("_links")]
    public required Links Links { get; set; }
    public required List<Value> Values { get; set; }
}

public class Links
{
    public string? Base { get; set; }
    public string? Context { get; set; }
    public string? Next { get; set; }
    public string? Prev { get; set; }
    public string? Self { get; set; }
    public string? Portal { get; set; }
}


public class Value
{
    public required string Id { get; set; }
    public string? ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public string? ProjectKey { get; set; }
    public Links? Links { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? HelpText { get; set; }
    public string? ServiceDeskId { get; set; }
    public List<string>? GroupIds { get; set; }
}