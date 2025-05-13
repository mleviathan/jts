using Jts.Services.HttpClients;
using Moq;
using Jts.Services;
using Jts.Models;
using Jts.Models.Jira;

public class JiraIssueCreatorTests
{
    private static Links DefaultLinks => new Links { Self = "http://test" };
    
    [Fact]
    public async Task GetServiceDeskId_ShouldReturnNull_WhenProjectNotFound()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        
        mockHttpClient
            .Setup(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskProjectsResponse { Values = new List<Value>(), Links = DefaultLinks });

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty);

        // Act
        var result = await jiraIssueCreator.GetServiceDeskId("TEST");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetServiceDeskId_ShouldReturnId_WhenProjectFound()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var serviceDesk = new Value { Id = "123", ProjectKey = "TEST" };
        
        mockHttpClient
            .Setup(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskProjectsResponse { Values = new List<Value> { serviceDesk }, Links = DefaultLinks, IsLastPage = true });

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty);

        // Act
        var result = await jiraIssueCreator.GetServiceDeskId("TEST");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123, result.Value);
    }

    [Fact]
    public async Task GetTaskRequestTypeId_ShouldReturnNull_WhenRequestTypesNotFound()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        
        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetServiceDeskRequestTypesResponse?)null);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty);

        // Act
        var result = await jiraIssueCreator.GetTaskRequestTypeId(123);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetTaskRequestTypeId_ShouldReturnNull_WhenTaskTypeNotFound()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var requestType = new Value { Id = "456", Name = "Bug" };
        
        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskRequestTypesResponse { Values = new List<Value> { requestType }, Links = DefaultLinks });

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty);

        // Act
        var result = await jiraIssueCreator.GetTaskRequestTypeId(123);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetTaskRequestTypeId_ShouldReturnId_WhenTaskTypeFound()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var requestType = new Value { Id = "456", Name = "Task" };
        
        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskRequestTypesResponse { Values = new List<Value> { requestType }, Links = DefaultLinks });

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty);

        // Act
        var result = await jiraIssueCreator.GetTaskRequestTypeId(123);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(456, result.Value);
    }

    [Fact]
    public async Task GetServiceDeskId_ShouldHandlePagination_WhenProjectNotInFirstPage()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        
        var firstPage = new GetServiceDeskProjectsResponse 
        { 
            Values = new List<Value> { new Value { Id = "123", ProjectKey = "OTHER" } },
            Links = DefaultLinks,
            IsLastPage = false
        };

        var secondPage = new GetServiceDeskProjectsResponse 
        { 
            Values = new List<Value> { new Value { Id = "456", ProjectKey = "TARGET" } },
            Links = DefaultLinks,
            IsLastPage = true
        };

        mockHttpClient.SetupSequence(m => m.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(firstPage)
            .ReturnsAsync(secondPage);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty);

        // Act
        var result = await jiraIssueCreator.GetServiceDeskId("TARGET");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(456, result.Value);
    }

    [Fact]
    public async Task GetServiceDeskId_ShouldReturnNull_WhenResponseIsNull()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        mockHttpClient.Setup(m => m.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetServiceDeskProjectsResponse?)null);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty);

        // Act
        var result = await jiraIssueCreator.GetServiceDeskId("TARGET");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetServiceDeskId_ShouldReturnNull_WhenValuesIsNull()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
          var response = new GetServiceDeskProjectsResponse 
        { 
            Values = new List<Value>(),
            Links = DefaultLinks,
            IsLastPage = true
        };

        mockHttpClient.Setup(m => m.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty);

        // Act
        var result = await jiraIssueCreator.GetServiceDeskId("TARGET");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetTaskRequestTypeId_ShouldReturnId_WhenTaskTypeExists()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        
        var response = new GetServiceDeskRequestTypesResponse 
        { 
            Values = new List<Value> { new Value { Id = "123", Name = "Task" } },
            Links = DefaultLinks
        };

        mockHttpClient.Setup(m => m.GetServiceDeskRequestTypes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty);

        // Act
        var result = await jiraIssueCreator.GetTaskRequestTypeId(456);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123, result.Value);
    }

    [Fact]
    public async Task GetTaskRequestTypeId_ShouldReturnNull_WhenResponseIsNull()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        mockHttpClient.Setup(m => m.GetServiceDeskRequestTypes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetServiceDeskRequestTypesResponse?)null);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty);

        // Act
        var result = await jiraIssueCreator.GetTaskRequestTypeId(456);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetTaskRequestTypeId_ShouldReturnNull_WhenValuesIsNull()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
          var response = new GetServiceDeskRequestTypesResponse 
        { 
            Values = new List<Value>(),
            Links = DefaultLinks
        };

        mockHttpClient.Setup(m => m.GetServiceDeskRequestTypes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty);

        // Act
        var result = await jiraIssueCreator.GetTaskRequestTypeId(456);

        // Assert
        Assert.Null(result);
    }
}