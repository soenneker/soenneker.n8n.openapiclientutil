using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.N8n.HttpClients.Registrars;
using Soenneker.N8n.OpenApiClientUtil.Abstract;

namespace Soenneker.N8n.OpenApiClientUtil.Registrars;

/// <summary>
/// Registers the OpenAPI client utility for dependency injection.
/// </summary>
public static class N8nOpenApiClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="N8nOpenApiClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddN8nOpenApiClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddN8nOpenApiHttpClientAsSingleton()
                .TryAddSingleton<IN8nOpenApiClientUtil, N8nOpenApiClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="N8nOpenApiClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddN8nOpenApiClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddN8nOpenApiHttpClientAsSingleton()
                .TryAddScoped<IN8nOpenApiClientUtil, N8nOpenApiClientUtil>();

        return services;
    }
}
