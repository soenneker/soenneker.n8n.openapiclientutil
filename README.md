[![](https://img.shields.io/nuget/v/soenneker.n8n.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.n8n.openapiclientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.n8n.openapiclientutil/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.n8n.openapiclientutil/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.n8n.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.n8n.openapiclientutil/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.N8n.OpenApiClientUtil
### A thread-safe utility for obtaining N8n's OpenApiClient singleton.

## Installation

```
dotnet add package Soenneker.N8n.OpenApiClientUtil
```

The parameterless `Get()` uses `N8N:ApiKey` and `N8n:ClientBaseUrl`. Pass connection values explicitly to work with multiple n8n instances:

```csharp
N8nOpenApiClient tenantClient = await n8nOpenApiClientUtil.Get(tenantApiKey, tenantBaseUrl);
```
