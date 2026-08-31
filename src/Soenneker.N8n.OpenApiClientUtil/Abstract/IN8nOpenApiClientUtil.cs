using Soenneker.N8n.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.N8n.OpenApiClientUtil.Abstract;

/// <summary>
/// Provides cached, authenticated n8n API clients for one or more servers.
/// </summary>
public interface IN8nOpenApiClientUtil : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets a client using the configured API key and base URL.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The configured n8n client.</returns>
    ValueTask<N8nOpenApiClient> Get(CancellationToken cancellationToken = default);

    /// <summary>Gets a client for a specific API key using the configured base URL.</summary>
    ValueTask<N8nOpenApiClient> Get(string apiKey, CancellationToken cancellationToken = default);

    /// <summary>Gets a client for a specific n8n connection.</summary>
    ValueTask<N8nOpenApiClient> Get(string apiKey, string baseUrl, CancellationToken cancellationToken = default);
}
