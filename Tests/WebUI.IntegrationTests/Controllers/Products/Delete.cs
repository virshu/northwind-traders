using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Northwind.WebUI.IntegrationTests.Common;
using Xunit;

namespace Northwind.WebUI.IntegrationTests.Controllers.Products;

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

        int validId = 1;

        HttpResponseMessage response = await client.DeleteAsync($"/api/products/delete/{validId}");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GivenInvalidId_ReturnsNotFoundStatusCode()
    {
        HttpClient client = await _factory.GetAuthenticatedClientAsync();

        int invalidId = 0;

        HttpResponseMessage response = await client.DeleteAsync($"/api/products/delete/{invalidId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}