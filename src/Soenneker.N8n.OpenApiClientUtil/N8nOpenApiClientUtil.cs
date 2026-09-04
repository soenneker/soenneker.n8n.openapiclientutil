using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Dictionaries.Singletons;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.N8n.HttpClients.Abstract;
using Soenneker.N8n.OpenApiClientUtil.Abstract;
using Soenneker.N8n.OpenApiClient;

namespace Soenneker.N8n.OpenApiClientUtil;

/// <inheritdoc cref="IN8nOpenApiClientUtil" />
public sealed class N8nOpenApiClientUtil : IN8nOpenApiClientUtil
{
    private readonly SingletonDictionary<N8nOpenApiClient> _clients;
    private readonly IN8nOpenApiHttpClient _httpClientUtil;
    private readonly IConfiguration _configuration;
    private readonly string? _baseUrl;

    public N8nOpenApiClientUtil(IN8nOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _httpClientUtil = httpClientUtil;
        _configuration = configuration;
        _baseUrl = configuration["N8n:ClientBaseUrl"];
        _clients = new SingletonDictionary<N8nOpenApiClient>(CreateClient);
    }

    private async ValueTask<N8nOpenApiClient> CreateClient(string connectionKey, CancellationToken token)
    {
        (string apiKey, string baseUrl) = ParseConnectionKey(connectionKey);
        HttpClient httpClient = await _httpClientUtil.Get(apiKey, baseUrl, token).NoSync();
        var requestAdapter = new HttpClientRequestAdapter(
            new AnonymousAuthenticationProvider(), httpClient: httpClient)
        {
            BaseUrl = baseUrl
        };

        return new N8nOpenApiClient(requestAdapter);
    }

    public ValueTask<N8nOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return Get(_configuration.GetValueStrict<string>("N8n:ApiKey"), GetConfiguredBaseUrl(), cancellationToken);
    }

    public ValueTask<N8nOpenApiClient> Get(string apiKey, CancellationToken cancellationToken = default)
    {
        return Get(apiKey, GetConfiguredBaseUrl(), cancellationToken);
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

    private string GetConfiguredBaseUrl()
    {
        return _baseUrl ?? throw new InvalidOperationException("N8n:ClientBaseUrl must be configured when a base URL is not supplied explicitly.");
    }

    public void Dispose()
    {
        _clients.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _clients.DisposeAsync();
    }
}
