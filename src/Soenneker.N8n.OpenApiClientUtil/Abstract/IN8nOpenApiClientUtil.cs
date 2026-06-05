using Soenneker.N8n.OpenApiClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.N8n.OpenApiClientUtil.Abstract;

/// <summary>
/// Exposes a cached OpenAPI client instance.
/// </summary>
public interface IN8nOpenApiClientUtil: IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<N8nOpenApiClient> Get(CancellationToken cancellationToken = default);
}
