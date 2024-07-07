using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Northwind.Application.Customers.Queries.GetCustomerDetail;
using Northwind.WebUI.IntegrationTests.Common;
using Xunit;

namespace Northwind.WebUI.IntegrationTests.Controllers.Customers;

public class GetById : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public GetById(CustomWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GivenId_ReturnsCustomerViewModel()
    {
        HttpClient client = await _factory.GetAuthenticatedClientAsync();

        string id = "ALFKI";

        HttpResponseMessage response = await client.GetAsync($"/api/customers/get/{id}");

        response.EnsureSuccessStatusCode();

        CustomerDetailVm customer = await Utilities.GetResponseContent<CustomerDetailVm>(response);

        Assert.Equal(id, customer.Id);
    }

    [Fact]
    public async Task GivenInvalidId_ReturnsNotFoundStatusCode()
    {
        HttpClient client = await _factory.GetAuthenticatedClientAsync();
            
        string invalidId = "AAAAA";

        HttpResponseMessage response = await client.GetAsync($"/api/customers/get/{invalidId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}