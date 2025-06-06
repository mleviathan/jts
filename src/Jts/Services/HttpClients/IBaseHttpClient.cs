namespace Jts.Services.HttpClients;

public interface IBaseHttpClient
{
    /// <summary>
    /// Sets the authentication header as Bearer token.
    /// </summary>
    /// <param name="apiKey">The API key to set as Bearer token.</param>
    void SetAuthenticationHeaderAsBearer(string apiKey);
}