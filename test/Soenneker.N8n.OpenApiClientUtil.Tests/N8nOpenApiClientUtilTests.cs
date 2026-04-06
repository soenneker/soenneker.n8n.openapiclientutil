using Soenneker.N8n.OpenApiClientUtil.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.N8n.OpenApiClientUtil.Tests;

[Collection("Collection")]
public sealed class N8nOpenApiClientUtilTests : FixturedUnitTest
{
    private readonly IN8nOpenApiClientUtil _openapiclientutil;

    public N8nOpenApiClientUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _openapiclientutil = Resolve<IN8nOpenApiClientUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
