using Soenneker.N8n.OpenApiClientUtil.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.N8n.OpenApiClientUtil.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class N8nOpenApiClientUtilTests : HostedUnitTest
{
    private readonly IN8nOpenApiClientUtil _openapiclientutil;

    public N8nOpenApiClientUtilTests(Host host) : base(host)
    {
        _openapiclientutil = Resolve<IN8nOpenApiClientUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
