﻿using MediatR;
using Moq;
using Northwind.Application.Customers.Commands.CreateCustomer;
using System.Threading;
using System.Threading.Tasks;
using Northwind.Application.UnitTests.Common;
using Xunit;

namespace Northwind.Application.UnitTests.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandTests : CommandTestBase
{
    [Fact]
    public void Handle_GivenValidRequest_ShouldRaiseCustomerCreatedNotification()
    {
        // Arrange
        Mock<IMediator> mediatorMock = new();
        CreateCustomerCommand.Handler sut = new(_context, mediatorMock.Object);
        string newCustomerId = "QAZQ1";

        // Act
        Task<Unit> result = sut.Handle(new CreateCustomerCommand { Id = newCustomerId, CompanyName = "New Company" }, CancellationToken.None);


        // Assert
        mediatorMock.Verify(m => m.Publish(It.Is<CustomerCreated>(cc => cc.CustomerId == newCustomerId), It.IsAny<CancellationToken>()), Times.Once);
    }
}