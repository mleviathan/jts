using Jts.Services.HttpClients;
using Moq;
using Jts.Services;
using Jts.Models;
using Jts.Models.Jira;

namespace Jts.Tests.Services;

public class JiraIssueCreatorTests
{
    private static Links DefaultLinks => new Links { Self = "http://test" };

    [Fact]
    public async Task GetServiceDeskId_ShouldReturnNull_WhenProjectNotFound()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        mockHttpClient
            .Setup(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskProjectsResponse { Values = new List<Value>(), Links = DefaultLinks });

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

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
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var serviceDesk = new Value { Id = "123", ProjectKey = "TEST" };

        mockHttpClient
            .Setup(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskProjectsResponse { Values = new List<Value> { serviceDesk }, Links = DefaultLinks, IsLastPage = true });

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

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
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetServiceDeskRequestTypesResponse?)null);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

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
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var requestType = new Value { Id = "456", Name = "Bug" };

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskRequestTypesResponse { Values = new List<Value> { requestType }, Links = DefaultLinks });

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

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
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var requestType = new Value { Id = "456", Name = "Task" };

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskRequestTypesResponse { Values = new List<Value> { requestType }, Links = DefaultLinks });

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

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
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
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

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

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
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        mockHttpClient.Setup(m => m.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetServiceDeskProjectsResponse?)null);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

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
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var response = new GetServiceDeskProjectsResponse
        {
            Values = new List<Value>(),
            Links = DefaultLinks,
            IsLastPage = true
        };

        mockHttpClient.Setup(m => m.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

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
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        var response = new GetServiceDeskRequestTypesResponse
        {
            Values = new List<Value> { new Value { Id = "123", Name = "Task" } },
            Links = DefaultLinks
        };

        mockHttpClient.Setup(m => m.GetServiceDeskRequestTypes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

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
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");

        mockHttpClient.Setup(m => m.GetServiceDeskRequestTypes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetServiceDeskRequestTypesResponse?)null);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

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
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var options = new JiraManagerOptions("apiKey", "https://something.com", "user@example.com");
        var response = new GetServiceDeskRequestTypesResponse
        {
            Values = new List<Value>(),
            Links = DefaultLinks
        };

        mockHttpClient.Setup(m => m.GetServiceDeskRequestTypes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

        // Act
        var result = await jiraIssueCreator.GetTaskRequestTypeId(456);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Initialize_WhenIssueNotFound_SetsNoProperties()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();

        mockHttpClient
            .Setup(client => client.GetIssue(It.IsAny<string>()))
            .ReturnsAsync((JiraIssue?)null);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

        // Act
        await jiraIssueCreator.Initialize("TEST-123", "TEST");

        // Assert
        mockHttpClient.Verify(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Initialize_WhenSuccessful_SetsAllProperties()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();

        var jiraIssue = new JiraIssue { Key = "TEST-123" };
        var serviceDesk = new Value { Id = "123", ProjectKey = "TEST" };
        var requestType = new Value { Id = "456", Name = "Task" };
        var requestTypeFields = new List<RequestTypeField> { new RequestTypeField { Name = "TestField", FieldId = "field1" } };

        mockHttpClient
            .Setup(client => client.GetIssue(It.IsAny<string>()))
            .ReturnsAsync(jiraIssue);

        mockHttpClient
            .Setup(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskProjectsResponse { Values = new List<Value> { serviceDesk }, Links = DefaultLinks, IsLastPage = true });

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskRequestTypesResponse { Values = new List<Value> { requestType }, Links = DefaultLinks });

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypeFields(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskRequestTypeFieldsResponse { RequestTypeFields = requestTypeFields });

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, string.Empty, mockAttachmentHttpClient.Object);

        // Act
        await jiraIssueCreator.Initialize("TEST-123", "TEST");

        // Assert - Verify all required methods were called
        mockHttpClient.Verify(client => client.GetIssue("TEST-123"), Times.Once);
        mockHttpClient.Verify(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        mockHttpClient.Verify(client => client.GetServiceDeskRequestTypes(123, It.IsAny<CancellationToken>()), Times.Once);
        mockHttpClient.Verify(client => client.GetServiceDeskRequestTypeFields(123, 456, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateIssue_WhenSuccessful_ReturnsNewIssue()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();

        var jiraIssue = new JiraIssue { Key = "TEST-123" };
        var serviceDesk = new Value { Id = "123", ProjectKey = "TEST" };
        var requestType = new Value { Id = "456", Name = "Task" };
        var requestTypeFields = new List<RequestTypeField> { new RequestTypeField { Name = "TestField", FieldId = "field1" } };
        var serviceDeskResponse = new PostServiceDeskResponse
        {
            IssueId = "10000",
            IssueKey = "SD-123",
            RequestTypeId = "456",
            ServiceDeskId = "123",
            Reporter = new Reporter
            {
                Name = "testuser",
                Key = "testuser",
                EmailAddress = "test@example.com",
                DisplayName = "Test User"
            },
            RequestFieldValues = new List<RequestFieldValue>(),
            CurrentStatus = new CurrentStatus { Status = "Todo" }
        };

        SetupInitializeSuccessful(mockHttpClient, jiraIssue, serviceDesk, requestType, requestTypeFields);

        mockHttpClient
            .Setup(client => client.PostCreateServiceDeskRequest(It.IsAny<PostServiceDeskRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceDeskResponse);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, "testuser", mockAttachmentHttpClient.Object);
        await jiraIssueCreator.Initialize("TEST-123", "TEST");

        // Act
        var result = await jiraIssueCreator.CreateIssue("TEST");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("SD-123", result.Key);
    }

    [Fact]
    public async Task CreateIssue_WhenServiceDeskRequestFails_ReturnsNull()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();

        var jiraIssue = new JiraIssue { Key = "TEST-123" };
        var serviceDesk = new Value { Id = "123", ProjectKey = "TEST" };
        var requestType = new Value { Id = "456", Name = "Task" };
        var requestTypeFields = new List<RequestTypeField> { new RequestTypeField { Name = "TestField", FieldId = "field1" } };

        SetupInitializeSuccessful(mockHttpClient, jiraIssue, serviceDesk, requestType, requestTypeFields);

        mockHttpClient
            .Setup(client => client.PostCreateServiceDeskRequest(It.IsAny<PostServiceDeskRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PostServiceDeskResponse?)null);

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, "testuser", mockAttachmentHttpClient.Object);
        await jiraIssueCreator.Initialize("TEST-123", "TEST");

        // Act
        var result = await jiraIssueCreator.CreateIssue("TEST");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AlignAttachments_WhenNoAttachments_ReturnsOriginalIssue()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var issue = new Issue(new JiraIssue { Key = "TEST-123" });

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, "testuser", mockAttachmentHttpClient.Object);

        // Act
        var result = await jiraIssueCreator.AlignAttachments(issue);

        // Assert
        Assert.Same(issue, result);
        mockHttpClient.Verify(client =>
            client.PostAddAttachmentToIssueAsync(
                It.IsAny<string>(),
                It.IsAny<PostAddAttachmentRequest>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task AlignAttachments_WhenHasAttachments_ClonesAndAddsAttachments()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var issue = new Issue(new JiraIssue { Key = "TEST-123" })
        {
            AttachmentsContentUris = new List<string> { "http://test.com/attachment.txt" }
        };

        mockAttachmentHttpClient
            .Setup(client => client.DownloadAttachmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new byte[] { 1, 2, 3 });

        mockHttpClient
            .Setup(client => client.UploadTemporaryFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PostTemporaryFileResult
            {
                TemporaryAttachments = new List<TemporaryAttachment>
                {
                    new TemporaryAttachment { FileName = "test.txt", TemporaryAttachmentId = "temp-123" }
                }
            });

        mockHttpClient
            .Setup(client => client.PostAddAttachmentToIssueAsync(It.IsAny<string>(), It.IsAny<PostAddAttachmentRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PostAddAttachmentResponse());

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, "testuser", mockAttachmentHttpClient.Object);

        // Set the service desk ID through reflection since it's private
        var fieldInfo = typeof(JiraIssueCreator).GetField("_serviceDeskId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fieldInfo?.SetValue(jiraIssueCreator, 123);

        // Act
        var result = await jiraIssueCreator.AlignAttachments(issue);

        // Assert
        Assert.NotNull(result);
        mockHttpClient.Verify(client =>
            client.PostAddAttachmentToIssueAsync(
                "TEST-123",
                It.Is<PostAddAttachmentRequest>(r => r.TemporaryAttachmentIds.Contains("temp-123")),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CloneAttachments_WhenDownloadFails_ContinuesWithRemainingAttachments()
    {
        // Arrange
        var mockHttpClient = new Mock<IJiraHttpClient>();
        var mockAttachmentHttpClient = new Mock<IJiraAttachmentHttpClient>();
        var issue = new Issue(new JiraIssue { Key = "TEST-123" })
        {
            AttachmentsContentUris = new List<string>
            {
                "http://test.com/fail.txt",
                "http://test.com/success.txt"
            }
        };

        mockAttachmentHttpClient
            .Setup(client => client.DownloadAttachmentAsync("http://test.com/fail.txt", It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        mockAttachmentHttpClient
            .Setup(client => client.DownloadAttachmentAsync("http://test.com/success.txt", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new byte[] { 1, 2, 3 });

        mockHttpClient
            .Setup(client => client.UploadTemporaryFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PostTemporaryFileResult
            {
                TemporaryAttachments = new List<TemporaryAttachment>
                {
                    new TemporaryAttachment { FileName = "test.txt", TemporaryAttachmentId = "temp-123" }
                }
            });

        var jiraIssueCreator = new JiraIssueCreator(mockHttpClient.Object, "testuser", mockAttachmentHttpClient.Object);

        // Set the service desk ID through reflection
        var fieldInfo = typeof(JiraIssueCreator).GetField("_serviceDeskId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fieldInfo?.SetValue(jiraIssueCreator, 123);

        // Act
        var result = await jiraIssueCreator.CloneAttachmentsAsync(issue);

        // Assert
        Assert.Single(result);
        Assert.Equal("temp-123", result[0]);
    }

    private static void SetupInitializeSuccessful(
        Mock<IJiraHttpClient> mockHttpClient,
        JiraIssue jiraIssue,
        Value serviceDesk,
        Value requestType,
        List<RequestTypeField> requestTypeFields)
    {
        mockHttpClient
            .Setup(client => client.GetIssue(It.IsAny<string>()))
            .ReturnsAsync(jiraIssue);

        mockHttpClient
            .Setup(client => client.GetServiceDeskProjects(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskProjectsResponse { Values = new List<Value> { serviceDesk }, Links = DefaultLinks, IsLastPage = true });

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskRequestTypesResponse { Values = new List<Value> { requestType }, Links = DefaultLinks });

        mockHttpClient
            .Setup(client => client.GetServiceDeskRequestTypeFields(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetServiceDeskRequestTypeFieldsResponse { RequestTypeFields = requestTypeFields });
    }
}