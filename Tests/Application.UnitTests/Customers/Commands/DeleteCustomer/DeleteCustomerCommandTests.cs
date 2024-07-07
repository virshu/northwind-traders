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
        : base()
    {
        _sut = new DeleteCustomerCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_GivenInvalidId_ThrowsNotFoundException()
    {
        string invalidId = "INVLD";

        DeleteCustomerCommand command = new DeleteCustomerCommand { Id = invalidId };

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_GivenValidIdAndZeroOrders_DeletesCustomer()
    {
        string validId = "JASON";

        DeleteCustomerCommand command = new DeleteCustomerCommand { Id = validId };

        await _sut.Handle(command, CancellationToken.None);

        Customer customer = await _context.Customers.FindAsync(validId);

        Assert.Null(customer);
    }

    [Fact]
    public async Task Handle_GivenValidIdAndSomeOrders_ThrowsDeleteFailureException()
    {
        string validId = "BREND";

        DeleteCustomerCommand command = new DeleteCustomerCommand { Id = validId };

        await Assert.ThrowsAsync<DeleteFailureException>(() => _sut.Handle(command, CancellationToken.None));

    }
}