using AutoMapper;
using Northwind.Application.Customers.Queries.GetCustomersList;
using Northwind.Application.UnitTests.Common;
using Northwind.Persistence;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NorthwindTraders.Application.UnitTests.Infrastructure;

[Collection("QueryCollection")]
public class GetCustomersListQueryHandlerTests
{
    private readonly NorthwindDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomersListQueryHandlerTests(QueryTestFixture fixture)
    {
        _context = fixture.Context;
        _mapper = fixture.Mapper;
    }

    [Fact]
    public async Task GetCustomersTest()
    {
        GetCustomersListQueryHandler sut = new(_context, _mapper);

        CustomersListVm result = await sut.Handle(new GetCustomersListQuery(), CancellationToken.None);

        result.ShouldBeOfType<CustomersListVm>();

        result.Customers.Count.ShouldBe(3);
    }
}