using System.Net.Http;
using System.Threading.Tasks;
using Northwind.Application.Customers.Queries.GetCustomersList;
using Northwind.WebUI.IntegrationTests.Common;
using Xunit;

namespace Northwind.WebUI.IntegrationTests.Controllers.Customers;

public class GetAll : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public GetAll(CustomWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ReturnsCustomersListViewModel()
    {
        HttpClient client = await _factory.GetAuthenticatedClientAsync();

        HttpResponseMessage response = await client.GetAsync("/api/customers/getall");

        response.EnsureSuccessStatusCode();

        CustomersListVm vm = await Utilities.GetResponseContent<CustomersListVm>(response);

        Assert.IsType<CustomersListVm>(vm);
        Assert.NotEmpty(vm.Customers);
    }
}