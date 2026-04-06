using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.ValueTask;
using Soenneker.N8n.HttpClients.Abstract;
using Soenneker.N8n.OpenApiClientUtil.Abstract;
using Soenneker.N8n.OpenApiClient;
using Soenneker.Kiota.GenericAuthenticationProvider;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.N8n.OpenApiClientUtil;

///<inheritdoc cref="IN8nOpenApiClientUtil"/>
public sealed class N8nOpenApiClientUtil : IN8nOpenApiClientUtil
{
    private readonly AsyncSingleton<N8nOpenApiClient> _client;

    public N8nOpenApiClientUtil(IN8nOpenApiHttpClient httpClientUtil, IConfiguration configuration)
    {
        _client = new AsyncSingleton<N8nOpenApiClient>(async token =>
        {
            HttpClient httpClient = await httpClientUtil.Get(token).NoSync();

            var apiKey = configuration.GetValueStrict<string>("N8N:ApiKey");
            string authHeaderValueTemplate = configuration["N8n:AuthHeaderValueTemplate"] ?? "{token}";
            string authHeaderValue = authHeaderValueTemplate.Replace("{token}", apiKey, StringComparison.Ordinal);

            var requestAdapter = new HttpClientRequestAdapter(new GenericAuthenticationProvider(headerValue: authHeaderValue), httpClient: httpClient);

            return new N8nOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<N8nOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}
