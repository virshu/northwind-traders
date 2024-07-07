using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Northwind.WebUI.IntegrationTests.Common;
using Xunit;

namespace Northwind.WebUI.IntegrationTests.Controllers.Customers;

public class Delete : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public Delete(CustomWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GivenId_ReturnsSuccessStatusCode()
    {
        HttpClient client = await _factory.GetAuthenticatedClientAsync();

        string validId = "ALFKI";

        HttpResponseMessage response = await client.DeleteAsync($"/api/customers/delete/{validId}");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GivenInvalidId_ReturnsNotFoundStatusCode()
    {
        HttpClient client = await _factory.GetAuthenticatedClientAsync();

        string invalidId = "AAAAA";

        HttpResponseMessage response = await client.DeleteAsync($"/api/customers/delete/{invalidId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}