using System.Net;
using Jts.Models;
using Jts.Models.Jira;
using Jts.Services;
using Jts.Services.HttpClients;
using Moq;
using Moq.Protected;

namespace Jts.Tests.Services;

/// <summary>
/// Tests for the JiraManager class.
/// </summary>
public class JiraManagerTests
{
    private static Links DefaultLinks => new Links { Self = "http://test" };

    [Fact]
    public void Constructor_ShouldThrowException_WhenArgumentsAreInvalid()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new JiraManager(new JiraManagerOptions(string.Empty, "baseUrl", "email")));
        Assert.Throws<ArgumentException>(() => new JiraManager(new JiraManagerOptions("apiKey", string.Empty, "email")));
        Assert.Throws<ArgumentException>(() => new JiraManager(new JiraManagerOptions("apiKey", "baseUrl", string.Empty)));
        Assert.Throws<UriFormatException>(() => new JiraManager(new JiraManagerOptions("apiKey", "baseUrl", "email")));
    }

    [Fact]
    public void Constructor_ShouldInstantiate_WhenArgumentsAreValid()
    {
        // Arrange & Act & Assert
        var manager = new JiraManager(new JiraManagerOptions("apiKey", "https://something.com", "email"));
        Assert.NotNull(manager.JiraHttpClient);
        Assert.IsType<JiraHttpClient>(manager.JiraHttpClient);
    }

    [Fact]
    public async Task GetIssues_ShouldReturnIssues_WhenHttpClientReturnsValidResponse()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        mockHttpClient
            .Setup(client => client.GetIssues("user"))
            .ReturnsAsync(new GetJiraIssuesResponse
            {
                Issues = new List<JiraIssue> { new JiraIssue { Key = "TEST-1" } }
            });

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };

        // Act
        var issues = await jiraManager.GetIssues();

        // Assert
        Assert.NotNull(issues);
        Assert.Single(issues);
        Assert.Equal("TEST-1", issues.First().Key);
    }

    [Fact]
    public async Task CheckConnection_ShouldReturnTrue_WhenConnectionIsSuccessful()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        mockHttpClient
            .Setup(client => client.HeadIssues(It.IsAny<CancellationToken>()))
            .ReturnsAsync(JiraConnectionStatusEnum.Connected);

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };

        // Act
        var result = await jiraManager.CheckConnection();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CheckConnection_ShouldReturnFalse_WhenUnauthorizedAndBearerTokenFails()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        var sequence = mockHttpClient.SetupSequence(client => client.HeadIssues(It.IsAny<CancellationToken>()))
            .ReturnsAsync(JiraConnectionStatusEnum.Unauthorized)
            .ReturnsAsync(JiraConnectionStatusEnum.Unauthorized);

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };

        // Act
        var result = await jiraManager.CheckConnection();

        // Assert
        Assert.False(result);
        mockHttpClient.Verify(client => client.SetAuthenticationHeaderAsBearer(options.ApiKey), Times.Once);
    }

    [Fact]
    public async Task CheckConnection_ShouldReturnFalse_WhenNotConnected()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        mockHttpClient
            .Setup(client => client.HeadIssues(It.IsAny<CancellationToken>()))
            .ReturnsAsync(JiraConnectionStatusEnum.NotConnected);

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };

        // Act
        var result = await jiraManager.CheckConnection();

        // Assert
        Assert.False(result);
        mockHttpClient.Verify(client => client.SetAuthenticationHeaderAsBearer(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task CloneIssue_ShouldReturnNull_WhenSourceIssueNotFound()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        mockHttpClient
            .Setup(client => client.GetIssue("TEST-1"))
            .ReturnsAsync((JiraIssue?)null);

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };

        // Act
        var result = await jiraManager.CloneIssue("TEST-1", "DEST");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CloneIssue_ShouldReturnNull_WhenServiceDeskNotFound()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var sourceIssue = new JiraIssue { Key = "TEST-1" };

        mockHttpClient
            .Setup(client => client.GetIssue("TEST-1"))
            .ReturnsAsync(sourceIssue);

        mockHttpClient
            .Setup(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskProjectsResponse { Values = [], Links = DefaultLinks });

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };

        // Act
        var result = await jiraManager.CloneIssue("TEST-1", "DEST");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CloneIssue_ShouldReturnNull_WhenRequestTypeNotFound()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var sourceIssue = new JiraIssue { Key = "TEST-1" };
        var serviceDesk = new Value { Id = "123", ProjectKey = "DEST" };

        mockHttpClient
            .Setup(client => client.GetIssue("TEST-1"))
            .ReturnsAsync(sourceIssue);

        mockHttpClient
            .Setup(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskProjectsResponse { Values = new List<Value> { serviceDesk }, Links = DefaultLinks });

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskRequestTypesResponse { Values = new List<Value>(), Links = DefaultLinks });

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };

        // Act
        var result = await jiraManager.CloneIssue("TEST-1", "DEST");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CloneIssue_ShouldReturnNull_WhenFieldsNotFound()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var sourceIssue = new JiraIssue
        {
            Key = "TEST-1",
            Fields = new JiraIssueFields { Summary = "Test Issue" }
        };
        var serviceDesk = new Value { Id = "123", ProjectKey = "DEST" };
        var requestType = new Value { Id = "456", Name = "Task" };

        mockHttpClient
            .Setup(client => client.GetIssue("TEST-1"))
            .ReturnsAsync(sourceIssue);

        mockHttpClient
            .Setup(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskProjectsResponse { Values = new List<Value> { serviceDesk }, Links = DefaultLinks });

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskRequestTypesResponse { Values = new List<Value> { requestType }, Links = DefaultLinks });

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypeFields(123, 456, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetServiceDeskRequestTypeFieldsResponse?)null);

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };

        // Act
        var result = await jiraManager.CloneIssue("TEST-1", "DEST");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CloneIssue_ShouldReturnClonedIssue_WhenSuccessful()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var sourceIssue = new JiraIssue
        {
            Key = "TEST-1",
            Fields = new JiraIssueFields { Summary = "Test Issue" }
        };
        var serviceDesk = new Value { Id = "123", ProjectKey = "DEST" };
        var requestType = new Value { Id = "456", Name = "Task" };
        var field = new RequestTypeField
        {
            FieldId = "summary",
            Name = "Summary",
            Required = true
        };
        var fields = new GetServiceDeskRequestTypeFieldsResponse
        {
            RequestTypeFields = new List<RequestTypeField> { field }
        };
        var clonedIssue = new JiraIssue
        {
            Key = "DEST-1",
            Fields = new JiraIssueFields { Summary = "Test Issue" }
        };

        mockHttpClient
            .Setup(client => client.GetIssue("TEST-1"))
            .ReturnsAsync(sourceIssue);

        mockHttpClient
            .Setup(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskProjectsResponse { Values = new List<Value> { serviceDesk }, Links = DefaultLinks });

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskRequestTypesResponse { Values = new List<Value> { requestType }, Links = DefaultLinks });

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypeFields(123, 456, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fields);

        mockHttpClient
            .Setup(client => client.PostCreateServiceDeskRequest(It.IsAny<PostServiceDeskRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(clonedIssue);

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };

        // Act
        var result = await jiraManager.CloneIssue("TEST-1", "DEST");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("DEST-1", result.Key);
        Assert.Equal("Test Issue", result.Summary);
    }
    [Fact]
    public async Task CloneIssue_ShouldReturnNewIssue_WhenSuccessful()
    {
        // Arrange
        var mockJiraFieldTransformationService = new Mock<IJiraFieldTransformationService>();
        mockJiraFieldTransformationService.Setup(_ => _.ElaborateRequiredFieldsValues(It.IsAny<List<RequestTypeField>>(), It.IsAny<Issue>(), It.IsAny<string>()))
            .Returns(new Dictionary<string, object> { { "summary", "Test Issue" } });

        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        // Setup source issue
        var sourceIssue = new JiraIssue { Key = "TEST-1", Fields = new JiraIssueFields { Summary = "Test Issue" } };
        mockHttpClient.Setup(m => m.GetIssue("TEST-1")).ReturnsAsync(sourceIssue);

        // Setup service desk project
        var serviceDesk = new GetServiceDeskProjectsResponse
        {
            Values = new List<Value> { new Value { Id = "123", ProjectKey = "DEST" } },
            Links = DefaultLinks,
            IsLastPage = true
        };
        mockHttpClient.Setup(m => m.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceDesk);

        // Setup request type
        var requestTypes = new GetServiceDeskRequestTypesResponse
        {
            Values = new List<Value> { new Value { Id = "456", Name = "Task" } },
            Links = DefaultLinks
        };
        mockHttpClient.Setup(m => m.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync(requestTypes);

        // Setup request type fields
        var fields = new GetServiceDeskRequestTypeFieldsResponse
        {
            RequestTypeFields = new List<RequestTypeField> { new RequestTypeField { FieldId = "summary", Name = "Summary" } }
        };
        mockHttpClient.Setup(m => m.GetServiceDeskRequestTypeFields(123, 456, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fields);

        // Setup create request
        var createdIssue = new JiraIssue { Key = "DEST-1", Fields = new JiraIssueFields { Summary = "Test Issue" } };
        mockHttpClient.Setup(m => m.PostCreateServiceDeskRequest(It.IsAny<PostServiceDeskRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdIssue);

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };

        // Act
        var result = await jiraManager.CloneIssue("TEST-1", "DEST");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("DEST-1", result.Key);
        Assert.Equal("Test Issue", result.Summary);
    }

    [Fact]
    public async Task CloneIssue_ShouldReturnNull_WhenRequestTypeFieldsAreEmpty()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        // Setup source issue
        var sourceIssue = new JiraIssue { Key = "TEST-1" };
        mockHttpClient.Setup(m => m.GetIssue("TEST-1")).ReturnsAsync(sourceIssue);

        // Setup service desk project
        var serviceDesk = new GetServiceDeskProjectsResponse
        {
            Values = new List<Value> { new Value { Id = "123", ProjectKey = "DEST" } },
            Links = DefaultLinks,
            IsLastPage = true
        };
        mockHttpClient.Setup(m => m.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceDesk);

        // Setup request type
        var requestTypes = new GetServiceDeskRequestTypesResponse
        {
            Values = new List<Value> { new Value { Id = "456", Name = "Task" } },
            Links = DefaultLinks
        };
        mockHttpClient.Setup(m => m.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync(requestTypes);

        // Setup empty request type fields
        var fields = new GetServiceDeskRequestTypeFieldsResponse
        {
            RequestTypeFields = new List<RequestTypeField>()
        };
        mockHttpClient.Setup(m => m.GetServiceDeskRequestTypeFields(123, 456, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fields);

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };

        // Act
        var result = await jiraManager.CloneIssue("TEST-1", "DEST");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetIssues_ShouldReturnNull_WhenNoResponseReceived()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "test@example.com");	

        mockHttpClient.Setup(client => client.GetIssues("test@example.com"))
            .ReturnsAsync((GetJiraIssuesResponse?)null);
        
        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };
        
        Assert.Null(await jiraManager.GetIssues());
    }

    [Fact]
    public async Task GetIssues_ShouldReturnNull_WhenNoIssuesAreFound()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "test@example.com");	

        mockHttpClient.Setup(client => client.GetIssues("test"))
            .ReturnsAsync(new GetJiraIssuesResponse
            {
                Issues = []
            });

        var jiraManager = new JiraManager(options) { JiraHttpClient = mockHttpClient.Object };
        
        Assert.Null(await jiraManager.GetIssues());
    }
}