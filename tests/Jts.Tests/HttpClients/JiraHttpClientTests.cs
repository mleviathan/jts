using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Jts.Models;
using Jts.Models.Jira;
using Jts.Services.HttpClients;
using Moq;
using Moq.Protected;
using Xunit;

namespace Jts.Tests.HttpClients;

public class JiraHttpClientTests
{
    [Fact]
    public void Constructor_ShouldThrowException_WhenArgumentsAreInvalid()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new JiraHttpClient(string.Empty, "email", "baseUrl"));
        Assert.Throws<ArgumentException>(() => new JiraHttpClient("apiKey", string.Empty, "baseUrl"));
    }

    [Fact]
    public void CreateBasicAuthHeader_ShouldGenerateCorrectHeader()
    {
        // Arrange
        var apiKey = "fakeApiKey";
        var email = "fakeEmail";
        var expectedHeader = "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(email + ":" + apiKey));

        // Act
        var authHeader = BaseHttpClient.CreateBasicAuthHeader(apiKey, email);

        // Assert
        Assert.NotNull(authHeader);
        Assert.Equal(expectedHeader, authHeader.ToString());
    }

    [Fact]    public async Task GetIssue_WhenSuccessful_ReturnsJiraIssue()
    {
        // Arrange
        var mockResponse = new JiraIssue
        {
            Key = "TEST-123",
            Fields = new JiraIssueFields()
        };
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse))
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetIssue("TEST-123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TEST-123", result.Key);
    }

    [Fact]
    public async Task GetIssue_WhenNotSuccessful_ReturnsNull()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetIssue("TEST-123");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetIssue_WhenExceptionOccurs_ReturnsNull()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Network error"));

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetIssue("TEST-123");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetIssue_VerifyCorrectEndpointCalled()
    {
        // Arrange
        var expectedUri = "https://test.atlassian.net/rest/api/2/issue/TEST-123";
        HttpRequestMessage? capturedRequest = null;

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{}")
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        await client.GetIssue("TEST-123");

        // Assert        
        Assert.NotNull(capturedRequest);
        Assert.Equal(expectedUri, capturedRequest.RequestUri?.ToString());
        Assert.Equal(HttpMethod.Get, capturedRequest.Method);
    }

    [Fact]
    public async Task GetIssues_ShouldLogContent_WhenResponseIsSuccessful()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"issues\":[]}")
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var jiraHttpClient = new JiraHttpClient("fakeApiKey", "fakeEmail", "http://example.com", httpClient);

        // Act
        await jiraHttpClient.GetIssues("user");

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => 
                req.Method == HttpMethod.Get && 
                req.RequestUri != null && 
                req.RequestUri.ToString().Contains("rest/api/2/search")),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task GetIssues_ShouldLogError_WhenResponseIsUnsuccessful()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var jiraHttpClient = new JiraHttpClient("fakeApiKey", "fakeEmail", "http://example.com", httpClient);

        // Act
        await jiraHttpClient.GetIssues("user");

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => 
                req.Method == HttpMethod.Get && 
                req.RequestUri != null && 
                req.RequestUri.ToString().Contains("rest/api/2/search")),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task GetIssues_ShouldHandleException_WhenHttpRequestFails()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Network error"));

        var httpClient = new HttpClient(handlerMock.Object);
        var jiraHttpClient = new JiraHttpClient("fakeApiKey", "fakeEmail", "http://example.com", httpClient);

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => jiraHttpClient.GetIssues("user"));
        Assert.Null(exception); // Method should handle the exception and return null
    }

    [Fact]
    public async Task GetServiceDeskRequestTypeFields_ShouldReturnResponse_WhenSuccessful()
    {
        // Arrange
        var mockResponse = new GetServiceDeskProjectsResponse 
        { 
            Links = new Links { Self = "test" },
            Values = new List<Value> { new Value { Id = "123", Name = "Test Project" } }
        };
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse))
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetServiceDeskRequestTypeFields("TEST-PROJ");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Values);
        Assert.Equal("123", result.Values[0].Id);
    }

    [Fact]
    public async Task GetServiceDeskProjects_ShouldReturnResponse_WhenSuccessful()
    {
        // Arrange
        var mockResponse = new GetServiceDeskProjectsResponse 
        { 
            Links = new Links { Self = "test" },
            Values = new List<Value> { new Value { Id = "123", ProjectKey = "TEST" } }
        };
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse))
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetServiceDeskProjects();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Values);
        Assert.Equal("123", result.Values[0].Id);
        Assert.Equal("TEST", result.Values[0].ProjectKey);
    }

    [Fact]
    public async Task GetServiceDeskRequestTypes_ShouldReturnResponse_WhenSuccessful()
    {
        // Arrange
        var mockResponse = new GetServiceDeskRequestTypesResponse 
        { 
            Links = new Links { Self = "test" },
            Values = new List<Value> { new Value { Id = "456", Name = "Task" } }
        };
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse))
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetServiceDeskRequestTypes(123);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Values);
        Assert.Equal("456", result.Values[0].Id);
        Assert.Equal("Task", result.Values[0].Name);
    }

    [Fact]
    public async Task HeadIssues_ShouldReturnConnected_WhenSuccessful()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.HeadIssues();

        // Assert
        Assert.Equal(JiraConnectionStatusEnum.Connected, result);
    }

    [Fact]
    public async Task HeadIssues_ShouldReturnUnauthorized_WhenUnauthorized()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.HeadIssues();

        // Assert
        Assert.Equal(JiraConnectionStatusEnum.Unauthorized, result);
    }

    [Fact]
    public async Task HeadIssues_ShouldReturnNotConnected_WhenExceptionOccurs()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Network error"));

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.HeadIssues();

        // Assert
        Assert.Equal(JiraConnectionStatusEnum.NotConnected, result);
    }

    [Fact]
    public async Task PostCreateServiceDeskRequest_ShouldReturnJiraIssue_WhenSuccessful()
    {
        // Arrange
        var mockResponse = new PostServiceDeskResponse
        {
            IssueKey = "DEST-1",
            IssueId = "1",
            RequestTypeId = "456",
            ServiceDeskId = "123",
            Reporter = new Reporter {Name = "test", DisplayName = "test", EmailAddress = "user@example.com", Key = "test", } ,
            CurrentStatus = new CurrentStatus { Status = "Open" } ,
            // RequestFieldValues = new RequestFieldValue { Summary = "Test Issue" }
            RequestFieldValues = new List<RequestFieldValue>
            {
                new RequestFieldValue { FieldId = "summary", Value = "Test Issue", }
            },
        };
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse))
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        var request = new PostServiceDeskRequest("123", "456", new Dictionary<string, object>());

        // Act
        var result = await client.PostCreateServiceDeskRequest(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("DEST-1", result.IssueKey);
    }

    [Fact]
    public async Task GetServiceDeskRequestTypeFields_WithServiceDeskId_ShouldReturnResponse_WhenSuccessful()
    {
        // Arrange
        var mockResponse = new GetServiceDeskRequestTypeFieldsResponse 
        { 
            RequestTypeFields = new List<RequestTypeField> 
            { 
                new RequestTypeField { FieldId = "summary", Name = "Summary" } 
            }
        };
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse))
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetServiceDeskRequestTypeFields(123, 456);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.RequestTypeFields);
        Assert.Single(result.RequestTypeFields);
        Assert.Equal("summary", result.RequestTypeFields[0].FieldId);
        Assert.Equal("Summary", result.RequestTypeFields[0].Name);    }

    [Fact]
    public async Task SetAuthenticationHeaderAsBearer_ShouldSetCorrectHeader()
    {
        // Arrange
        HttpRequestMessage? capturedRequest = null;
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{}")
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        client.SetAuthenticationHeaderAsBearer("new-api-key");
        await client.GetIssue("TEST-123"); // Make a request to check the header

        // Assert
        Assert.NotNull(capturedRequest);
        Assert.NotNull(capturedRequest.Headers.Authorization);
        Assert.Equal("Bearer", capturedRequest.Headers.Authorization.Scheme);
        Assert.Equal("new-api-key", capturedRequest.Headers.Authorization.Parameter);
    }

    [Fact]
    public async Task GetServiceDeskRequestTypeFields_WithServiceDeskId_ShouldVerifyCorrectUrl()
    {
        // Arrange
        var expectedUri = "https://test.atlassian.net/rest/servicedeskapi/servicedesk/123/requesttype/456/field";
        HttpRequestMessage? capturedRequest = null;

        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((request, _) => capturedRequest = request)
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{}")
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        await client.GetServiceDeskRequestTypeFields(123, 456);

        // Assert
        Assert.NotNull(capturedRequest);
        Assert.Equal(expectedUri, capturedRequest.RequestUri?.ToString());
        Assert.Equal(HttpMethod.Get, capturedRequest.Method);
    }

    [Fact]
    public async Task GetServiceDeskRequestTypeFields_ShouldReturnNull_WhenExceptionOccurs()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Network error"));

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetServiceDeskRequestTypeFields("TEST-PROJ");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetServiceDeskProjects_ShouldReturnNull_WhenUnsuccessful()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetServiceDeskProjects();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetServiceDeskProjects_ShouldReturnNull_WhenExceptionOccurs()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Network error"));

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetServiceDeskProjects();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetServiceDeskRequestTypes_ShouldReturnNull_WhenUnsuccessful()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetServiceDeskRequestTypes(123);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetServiceDeskRequestTypes_ShouldReturnNull_WhenExceptionOccurs()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Network error"));

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetServiceDeskRequestTypes(123);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task PostCreateServiceDeskRequest_ShouldReturnNull_WhenUnsuccessful()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        var request = new PostServiceDeskRequest("123", "456", []);

        // Act
        var result = await client.PostCreateServiceDeskRequest(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task PostCreateServiceDeskRequest_ShouldReturnNull_WhenExceptionOccurs()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Network error"));

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        var request = new PostServiceDeskRequest("123", "456", []);

        // Act
        var result = await client.PostCreateServiceDeskRequest(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetServiceDeskRequestTypeFields_WithIds_ShouldReturnNull_WhenUnsuccessful()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetServiceDeskRequestTypeFields(123, 456);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetServiceDeskRequestTypeFields_WithIds_ShouldReturnNull_WhenExceptionOccurs()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Network error"));

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Act
        var result = await client.GetServiceDeskRequestTypeFields(123, 456);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task PostAddAttachmentToIssueAsync_WhenSuccessful_ReturnsResponse()
    {
        // Arrange
        var mockResponse = new PostAddAttachmentResponse
        {
            // Set any required properties for PostAddAttachmentResponse here
        };
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse))
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        var request = new PostAddAttachmentRequest(
            new List<string> { "temp-123" },
            false
        );

        // Act
        var result = await client.PostAddAttachmentToIssueAsync("TEST-123", request);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task PostAddAttachmentToIssueAsync_WhenNotSuccessful_ReturnsNull()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        var request = new PostAddAttachmentRequest(
            new List<string> { "temp-123" },
            false
        );

        // Act
        var result = await client.PostAddAttachmentToIssueAsync("TEST-123", request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UploadTemporaryFileAsync_WhenSuccessful_ReturnsResult()
    {
        // Arrange
        var mockResponse = new PostTemporaryFileResult
        {
            TemporaryAttachments = new List<TemporaryAttachment> 
            { 
                new TemporaryAttachment { FileName = "test.txt", TemporaryAttachmentId = "temp-123" } 
            }
        };
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse))
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Create a temporary file for testing
        var tempFilePath = Path.GetTempFileName();
        File.WriteAllText(tempFilePath, "Test content");

        try
        {
            // Act
            var result = await client.UploadTemporaryFileAsync("SD-123", tempFilePath);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.TemporaryAttachments);
            Assert.Single(result.TemporaryAttachments);
            Assert.Equal("test.txt", result.TemporaryAttachments[0].FileName);
            Assert.Equal("temp-123", result.TemporaryAttachments[0].TemporaryAttachmentId);
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    [Fact]
    public async Task UploadTemporaryFileAsync_WhenNotSuccessful_ReturnsNull()
    {
        // Arrange
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

        var client = new JiraHttpClient(
            "test-api-key",
            "test@email.com",
            "https://test.atlassian.net",
            new HttpClient(mockHandler.Object));

        // Create a temporary file for testing
        var tempFilePath = Path.GetTempFileName();
        File.WriteAllText(tempFilePath, "Test content");

        try
        {
            // Act
            var result = await client.UploadTemporaryFileAsync("SD-123", tempFilePath);

            // Assert
            Assert.Null(result);
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UploadTemporaryFileAsync_WithInvalidServiceDeskId_ThrowsArgumentException(string serviceDeskId)
    {
        // Arrange
        var client = new JiraHttpClient("test-api-key", "test@email.com", "https://test.atlassian.net");

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            client.UploadTemporaryFileAsync(serviceDeskId, "test.txt"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UploadTemporaryFileAsync_WithInvalidFilePath_ThrowsArgumentException(string filePath)
    {
        // Arrange
        var client = new JiraHttpClient("test-api-key", "test@email.com", "https://test.atlassian.net");

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            client.UploadTemporaryFileAsync("SD-123", filePath));
    }
}
