using System.Net.Http;
using Northwind.Application.Products.Commands.CreateProduct;
using Northwind.WebUI.IntegrationTests.Common;
using System.Threading.Tasks;
using Xunit;

namespace Northwind.WebUI.IntegrationTests.Controllers.Products;

public class Create : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public Create(CustomWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GivenCreateProductCommand_ReturnsNewProductId()
    {
        HttpClient client = await _factory.GetAuthenticatedClientAsync();

        CreateProductCommand command = new CreateProductCommand
        {
            ProductName = "Coffee",
            SupplierId = 1,
            CategoryId = 1,
            UnitPrice = 19.00m,
            Discontinued = false
        };

        StringContent content = Utilities.GetRequestContent(command);

        HttpResponseMessage response = await client.PostAsync($"/api/products/create", content);

        response.EnsureSuccessStatusCode();

        int productId = await Utilities.GetResponseContent<int>(response);

        Assert.NotEqual(0, productId);
    }
}