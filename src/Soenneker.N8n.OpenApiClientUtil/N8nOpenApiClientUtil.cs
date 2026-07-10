using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Dictionaries.Singletons;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.N8n.HttpClients.Abstract;
using Soenneker.N8n.OpenApiClientUtil.Abstract;
using Soenneker.N8n.OpenApiClient;
using Soenneker.Kiota.GenericAuthenticationProvider;

namespace Soenneker.N8n.OpenApiClientUtil;

///<inheritdoc cref="IN8nOpenApiClientUtil"/>
public sealed class N8nOpenApiClientUtil : IN8nOpenApiClientUtil
{
    private readonly SingletonDictionary<N8nOpenApiClient> _clients;
    private readonly IN8nOpenApiHttpClient _httpClientUtil;
    private readonly string _apiKey;
    private readonly string _baseUrl;
    private readonly string _authHeaderName;
    private readonly string _authHeaderValueTemplate;

    public N8nOpenApiClientUtil(IN8nOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _httpClientUtil = httpClientUtil;
        _apiKey = configuration.GetValueStrict<string>("N8N:ApiKey");
        _baseUrl = configuration["N8n:ClientBaseUrl"] ?? "https://{your-n8n}/api/v1";
        _authHeaderName = configuration["N8n:AuthHeaderName"] ?? "X-N8N-API-KEY";
        _authHeaderValueTemplate = configuration["N8n:AuthHeaderValueTemplate"] ?? "{token}";
        _clients = new SingletonDictionary<N8nOpenApiClient>(CreateClient);
    }

    private async ValueTask<N8nOpenApiClient> CreateClient(string connectionKey, CancellationToken token)
    {
        (string apiKey, string baseUrl) = ParseConnectionKey(connectionKey);
        HttpClient httpClient = await _httpClientUtil.Get(token).NoSync();
        string authHeaderValue = _authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);

        var requestAdapter = new HttpClientRequestAdapter(
            new GenericAuthenticationProvider(headerName: _authHeaderName, headerValue: authHeaderValue), httpClient: httpClient)
        {
            BaseUrl = baseUrl
        };

        return new N8nOpenApiClient(requestAdapter);
    }

    public ValueTask<N8nOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return Get(_apiKey, _baseUrl, cancellationToken);
    }

    public ValueTask<N8nOpenApiClient> Get(string apiKey, CancellationToken cancellationToken = default)
    {
        return Get(apiKey, _baseUrl, cancellationToken);
    }

    public ValueTask<N8nOpenApiClient> Get(string apiKey, string baseUrl, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);

        string normalizedBaseUrl = new Uri(baseUrl, UriKind.Absolute).AbsoluteUri.TrimEnd('/');

        return _clients.Get(CreateConnectionKey(apiKey, normalizedBaseUrl), cancellationToken);
    }

    private static string CreateConnectionKey(string apiKey, string baseUrl) => string.Concat(apiKey, "\0", baseUrl);

    private static (string ApiKey, string BaseUrl) ParseConnectionKey(string connectionKey)
    {
        int separatorIndex = connectionKey.IndexOf('\0');

        return (connectionKey[..separatorIndex], connectionKey[(separatorIndex + 1)..]);
    }

    /// <summary>
    /// Releases resources used by the current instance.
    /// </summary>
    public void Dispose()
    {
        _clients.Dispose();
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public ValueTask DisposeAsync()
    {
        return _clients.DisposeAsync();
    }
}
