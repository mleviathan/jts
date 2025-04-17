using System.Net;
using System.Net.Http.Headers;
using Jts.Services.HttpClients;
using Moq;
using Moq.Protected;

namespace Jts.Tests;

public class JiraHttpClientTests
{
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
        var jiraHttpClient = new JiraHttpClient("fakeApiKey", "fakeEmail", httpClient);

        // Act
        await jiraHttpClient.GetIssues();

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("rest/api/2/search")),
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
        var jiraHttpClient = new JiraHttpClient("fakeApiKey", "fakeEmail", httpClient);

        // Act
        await jiraHttpClient.GetIssues();

        // Assert
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("rest/api/2/search")),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenArgumentsAreInvalid()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new JiraHttpClient(null, "email"));
        Assert.Throws<ArgumentNullException>(() => new JiraHttpClient("apiKey", "email", null, null));
        Assert.Throws<ArgumentNullException>(() => new JiraHttpClient("apiKey", null));
    }

    [Fact]
    public void CreateBasicAuthHeader_ShouldGenerateCorrectHeader()
    {
        // Arrange
        var apiKey = "fakeApiKey";
        var email = "fakeEmail";
        var expectedHeader = "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(email + ":" + apiKey));

        // Act
        var jiraHttpClient = new JiraHttpClient(apiKey, email);
        var authHeader = jiraHttpClient.GetType()
            .GetMethod("CreateBasicAuthHeader", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(jiraHttpClient, new object[] { apiKey, email }) as AuthenticationHeaderValue;

        // Assert
        Assert.NotNull(authHeader);
        Assert.Equal(expectedHeader, authHeader.ToString());
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
        var jiraHttpClient = new JiraHttpClient("fakeApiKey", "fakeEmail", httpClient);

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => jiraHttpClient.GetIssues());
        Assert.Null(exception); // Method should handle the exception and return null
    }
}
