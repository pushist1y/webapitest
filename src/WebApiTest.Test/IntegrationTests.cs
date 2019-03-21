using System.Threading.Tasks;
using WebApiTest.Test.Infrastructure;
using Xunit;

namespace WebapiTest.Test
{
    public class IntegrationTests : IClassFixture<FixtureServer>
    {
        private readonly FixtureServer _serverFixture;

        public IntegrationTests(FixtureServer serverFixture)
        {
            _serverFixture = serverFixture;
        }

        [Fact]
        public async Task TestGet()
        {
            using (var client = _serverFixture.Server.CreateClient())
            {
                var response = await client.GetAsync("/api/values");
                Assert.True(response.IsSuccessStatusCode);
            }
        }
        
    }
}
