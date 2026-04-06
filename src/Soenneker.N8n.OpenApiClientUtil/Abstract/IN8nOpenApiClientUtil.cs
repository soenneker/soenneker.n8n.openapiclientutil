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
    ValueTask<N8nOpenApiClient> Get(CancellationToken cancellationToken = default);
}
