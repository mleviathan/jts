using System.Net;
using Jts.Models.Jira;
using Jts.Services;
using Jts.Services.HttpClients;
using Moq;
using Moq.Protected;

namespace Jts.Tests.Services;

public class JiraManagerTests
{
    [Fact]
    public void Constructor_ShouldThrowException_WhenArgumentsAreInvalid()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new JiraManager(string.Empty, "baseUrl", "email"));
        Assert.Throws<ArgumentException>(() => new JiraManager("apiKey", string.Empty, "email"));
        Assert.Throws<ArgumentException>(() => new JiraManager("apiKey", "baseUrl", string.Empty));
        Assert.Throws<UriFormatException>(() => new JiraManager("apiKey", "baseUrl", "email"));
    }


    [Fact]
    public void Constructor_ShouldInstantiate_WhenArgumentsAreValid()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new JiraManager(string.Empty, "baseUrl", "email"));
        Assert.Throws<ArgumentException>(() => new JiraManager("apiKey", string.Empty, "email"));
        Assert.Throws<ArgumentException>(() => new JiraManager("apiKey", "baseUrl", string.Empty));
        var manager = new JiraManager("apiKey", "https://something.com", "email");
        Assert.NotNull(manager.JiraHttpClient);
        Assert.IsType<JiraHttpClient>(manager.JiraHttpClient);
    }

    [Fact]
    public async Task GetIssues_ShouldReturnIssues_WhenHttpClientReturnsValidResponse()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        mockHttpClient
            .Setup(client => client.GetIssues())
            .ReturnsAsync(new GetJiraIssuesResponse
            {
                Issues = new List<JiraIssue> { new JiraIssue { Key = "TEST-1" } }
            });

        var jiraManager = new JiraManager(mockHttpClient.Object);

        // Act
        var issues = await jiraManager.GetIssues();

        // Assert
        Assert.NotNull(issues);
        Assert.Single(issues);
        Assert.Equal("TEST-1", issues.First().Key);
    }

    [Fact]
    public async Task GetIssues_ShouldReturnNull_WhenHttpClientReturnsNoIssues()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        mockHttpClient
            .Setup(client => client.GetIssues())
            .ReturnsAsync(new GetJiraIssuesResponse { Issues = new List<JiraIssue>() });

        var jiraManager = new JiraManager(mockHttpClient.Object);

        // Act
        var issues = await jiraManager.GetIssues();

        // Assert
        Assert.Null(issues);
    }

    [Fact]
    public async Task GetIssues_ShouldReturnNull_WhenHttpClientReturnsNull()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        mockHttpClient
            .Setup(client => client.GetIssues())
            .ReturnsAsync((GetJiraIssuesResponse?)null);

        var jiraManager = new JiraManager(mockHttpClient.Object);

        // Act
        var issues = await jiraManager.GetIssues();

        // Assert
        Assert.Null(issues);
    }
}