using System.Net.Http;
using System.Threading.Tasks;
using Northwind.WebUI.IntegrationTests.Common;
using Xunit;

namespace Northwind.WebUI.IntegrationTests.Controllers.Categories;

public class GetCategoryList : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public GetCategoryList(CustomWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ReturnsSuccessResult()
    {
        HttpClient client = await _factory.GetAuthenticatedClientAsync();

        HttpResponseMessage response = await client.GetAsync("/api/categories/getall");

        response.EnsureSuccessStatusCode();
    }
}