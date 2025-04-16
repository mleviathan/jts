using Jts.Constants;
using Jts.Services;
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

JiraManager jiraManager = new(apiKey, baseUrl, email);

var cmd = Console.ReadLine();
if (cmd == Commands.AvailableCommands[0])
{
    Environment.Exit(0);
}
else if (cmd == Commands.AvailableCommands[1])
{
    Console.Clear();
}
else if (cmd == Commands.AvailableCommands[2])
{
    var availableCommands = string.Join(", ", Commands.AvailableCommands);
    Console.WriteLine($"Available commands: {availableCommands}");
}
else if (cmd == Commands.AvailableCommands[3])
{
    Console.WriteLine("Insert Key of the task to copy:");
    var uri = Console.ReadLine();
}
else if (cmd == Commands.AvailableCommands[4])
{
    jiraManager.GetIssues().GetAwaiter().GetResult();
}
else
{
    Console.WriteLine("Unknown command. Type 'help' for a list of available commands.");
}