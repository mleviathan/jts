using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jts.Services.HttpClients;
using Moq;
using Moq.Protected;
using Xunit;

namespace Jts.Tests.HttpClients;


public class JiraAttachmentHttpClientTest
{
    private JiraAttachmentHttpClient CreateClient(HttpResponseMessage responseMessage)
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(handlerMock.Object);
        return new JiraAttachmentHttpClient("dummyApiKey", "dummyEmail", httpClient);
    }

    [Fact]
    public async Task DownloadAttachmentAsync_ReturnsContent_WhenStatusCodeIsSuccess()
    {
        // Arrange
        var expectedContent = new byte[] { 1, 2, 3 };
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(expectedContent)
        };
        var client = CreateClient(response);

        // Act
        var result = await client.DownloadAttachmentAsync("http://test/attachment");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedContent, result);
    }

    [Fact]
    public async Task DownloadAttachmentAsync_ReturnsNull_WhenStatusCodeIsNotSuccess()
    {
        // Arrange
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);
        var client = CreateClient(response);

        // Act
        var result = await client.DownloadAttachmentAsync("http://test/attachment");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DownloadAttachmentAsync_ThrowsArgumentException_WhenUriIsNullOrWhitespace()
    {
        var client = CreateClient(new HttpResponseMessage(HttpStatusCode.OK));

        await Assert.ThrowsAsync<ArgumentNullException>(() => client.DownloadAttachmentAsync(null!));
        await Assert.ThrowsAsync<ArgumentException>(() => client.DownloadAttachmentAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.DownloadAttachmentAsync("   "));
    }

    [Fact]
    public async Task DownloadAttachmentAsync_ReturnsNull_OnException()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Network error"));

        var httpClient = new HttpClient(handlerMock.Object);
        var client = new JiraAttachmentHttpClient("dummyApiKey", "dummyEmail", httpClient);

        // Act
        var result = await client.DownloadAttachmentAsync("http://test/attachment");

        // Assert
        Assert.Null(result);
    }
}