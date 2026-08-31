[![](https://img.shields.io/nuget/v/soenneker.n8n.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.n8n.openapiclientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.n8n.openapiclientutil/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.n8n.openapiclientutil/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.n8n.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.n8n.openapiclientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.n8n.openapiclientutil/codeql.yml?style=for-the-badge&label=codeql)](https://github.com/soenneker/soenneker.n8n.openapiclientutil/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.N8n.OpenApiClientUtil

Creates and caches authenticated n8n API clients for one or more n8n servers.

## Installation

```bash
dotnet add package Soenneker.N8n.OpenApiClientUtil
```

## Configuration

```json
{
  "N8n": {
    "ApiKey": "your-api-key",
    "ClientBaseUrl": "https://n8n.example.com/api/v1"
  }
}
```

## Usage

```csharp
using Soenneker.N8n.OpenApiClientUtil.Abstract;
using Soenneker.N8n.OpenApiClientUtil.Registrars;

services.AddN8nOpenApiClientUtilAsSingleton();

IN8nOpenApiClientUtil n8n = serviceProvider
    .GetRequiredService<IN8nOpenApiClientUtil>();

var client = await n8n.Get(cancellationToken);
var workflows = await client.Workflows.GetAsync(cancellationToken: cancellationToken);
```

Use `Get(apiKey, baseUrl)` for another n8n server. Equivalent connection settings reuse the same generated client within the utility's lifetime.

Scoped registration creates a generated-client cache per application scope while retaining the shared HTTP provider. Disposing the scoped utility does not remove that shared provider or its clients.
