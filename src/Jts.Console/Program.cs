using Jts;
using Jts.Constants;
using Jts.Models;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>();

var configurationRoot = builder.Build();

var apiKey = configurationRoot["apiKey"];
var baseUrl = configurationRoot["baseUrl"];
var email = configurationRoot["email"];

Console.WriteLine("Your apiKey is: " + apiKey);
Console.WriteLine("Your baseUrl is: " + baseUrl);
Console.WriteLine("Your email is: " + email);
ArgumentException.ThrowIfNullOrEmpty(apiKey, nameof(apiKey));
ArgumentException.ThrowIfNullOrEmpty(baseUrl, nameof(baseUrl));
ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

var jiraManagerOptions = new JiraManagerOptions(
    apiKey,
    baseUrl,
    email
);
JiraManager jiraManager = new(jiraManagerOptions);
await jiraManager.CheckConnection();

var availableCommands = string.Join(", ", Commands.AvailableCommands);
Console.WriteLine($"Available commands: {availableCommands}");

var cmd = Console.ReadLine();
if (cmd == null)
{
    Environment.Exit(0);
}

if (cmd.Equals(Commands.AvailableCommands[0], StringComparison.OrdinalIgnoreCase))
{
    Environment.Exit(0);
}
else if (cmd.Equals(Commands.AvailableCommands[1], StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine($"Available commands: {availableCommands}");
}
else if (cmd.Equals(Commands.AvailableCommands[2], StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine("Insert Key of the task to copy:");
    var issueKey = Console.ReadLine();
    ArgumentException.ThrowIfNullOrEmpty(issueKey, nameof(issueKey));

    Console.WriteLine("Insert key of the project to copy to:");
    var projectKey = Console.ReadLine();
    ArgumentException.ThrowIfNullOrEmpty(projectKey, nameof(projectKey));

    await jiraManager.CloneIssue(issueKey, projectKey);
}
else if (cmd.Equals(Commands.AvailableCommands[3], StringComparison.OrdinalIgnoreCase))
{
    var issues = await jiraManager.GetIssues();
    if (issues == null || issues.Count == 0)
    {
        Console.WriteLine("No issues found.");
        return;
    }
    Console.WriteLine("These are your jira issues: " + string.Join(", ", issues.Select(i => i.Key)));
}
else
{
    Console.WriteLine("Unknown command. Type 'help' for a list of available commands.");
}