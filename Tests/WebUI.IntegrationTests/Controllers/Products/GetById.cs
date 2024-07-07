using Northwind.Application.Products.Queries.GetProductDetail;
using Northwind.WebUI.IntegrationTests.Common;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Northwind.WebUI.IntegrationTests.Controllers.Products;

public class GetById : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public GetById(CustomWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GivenId_ReturnsProductViewModel()
    {
        HttpClient client = await _factory.GetAuthenticatedClientAsync();

        int id = 1;

        HttpResponseMessage response = await client.GetAsync($"/api/products/get/{id}");

        response.EnsureSuccessStatusCode();

        ProductDetailVm product = await Utilities.GetResponseContent<ProductDetailVm>(response);

        Assert.Equal(id, product.ProductId);
    }

    [Fact]
    public async Task GivenInvalidId_ReturnsNotFoundStatusCode()
    {
        HttpClient client = await _factory.GetAuthenticatedClientAsync();

        int invalidId = 0;

        HttpResponseMessage response = await client.GetAsync($"/api/products/get/{invalidId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}