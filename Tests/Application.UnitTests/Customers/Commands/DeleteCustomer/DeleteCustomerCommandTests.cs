﻿using Northwind.Application.Common.Exceptions;
using Northwind.Application.Customers.Commands.DeleteCustomer;
using Northwind.Application.UnitTests.Common;
using System.Threading;
using System.Threading.Tasks;
using Northwind.Domain.Entities;
using Xunit;

namespace Northwind.Application.UnitTests.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandTests : CommandTestBase
{
    private readonly DeleteCustomerCommandHandler _sut;

    public DeleteCustomerCommandTests()
    {
        _sut = new DeleteCustomerCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_GivenInvalidId_ThrowsNotFoundException()
    {
        const string invalidId = "INVLD";

        DeleteCustomerCommand command = new() { Id = invalidId };

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_GivenValidIdAndZeroOrders_DeletesCustomer()
    {
        const string validId = "JASON";

        DeleteCustomerCommand command = new() { Id = validId };

        await _sut.Handle(command, CancellationToken.None);

        Customer customer = await _context.Customers.FindAsync(validId);

        Assert.Null(customer);
    }

    [Fact]
    public async Task Handle_GivenValidIdAndSomeOrders_ThrowsDeleteFailureException()
    {
        const string validId = "BREND";

        DeleteCustomerCommand command = new() { Id = validId };

        await Assert.ThrowsAsync<DeleteFailureException>(() => _sut.Handle(command, CancellationToken.None));

    }
}