using System.Net.Http;
using System.Threading.Tasks;
using Northwind.Application.Products.Queries.GetProductsList;
using Northwind.WebUI.IntegrationTests.Common;
using Xunit;

namespace Northwind.WebUI.IntegrationTests.Controllers.Products;

public class GetAll : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public GetAll(CustomWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ReturnsProductsListViewModel()
    {
        HttpClient client = await _factory.GetAuthenticatedClientAsync();

        HttpResponseMessage response = await client.GetAsync("/api/products/getall");

        response.EnsureSuccessStatusCode();

        ProductsListVm vm = await Utilities.GetResponseContent<ProductsListVm>(response);

        Assert.IsType<ProductsListVm>(vm);
        Assert.NotEmpty(vm.Products);
    }
}