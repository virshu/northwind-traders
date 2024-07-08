using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Northwind.Application.Common.Interfaces;
using Northwind.Common;
using Northwind.Domain.Entities;
using Northwind.Persistence;
using Shouldly;
using Xunit;

namespace Persistence.IntegrationTests;

public class NorthwindDbContextTests : IDisposable
{
    private readonly string _userId;
    private readonly DateTime _dateTime;
    private readonly NorthwindDbContext _sut;

    public NorthwindDbContextTests()
    {
        _dateTime = new DateTime(3001, 1, 1);
        Mock<IDateTime> dateTimeMock = new();
        dateTimeMock.Setup(m => m.Now).Returns(_dateTime);

        _userId = "00000000-0000-0000-0000-000000000000";
        Mock<ICurrentUserService> currentUserServiceMock = new();
        currentUserServiceMock.Setup(m => m.UserId).Returns(_userId);

        DbContextOptions<NorthwindDbContext> options = new DbContextOptionsBuilder<NorthwindDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _sut = new NorthwindDbContext(options, currentUserServiceMock.Object, dateTimeMock.Object);

        _sut.Products.Add(new Product
        {
            ProductId = 1, 
            ProductName = "Coffee"
        });

        _sut.SaveChanges();
    }

    [Fact]
    public async Task SaveChangesAsync_GivenNewProduct_ShouldSetCreatedProperties()
    {
        Product product = new()
        {
            ProductId = 2,
            ProductName = "Cake"
        };

        await _sut.Products.AddAsync(product);

        await _sut.SaveChangesAsync();

        product.Created.ShouldBe(_dateTime);
        product.CreatedBy.ShouldBe(_userId);
    }

    [Fact]
    public async Task SaveChangesAsync_GivenExistingProduct_ShouldSetLastModifiedProperties()
    {
        Product product = await _sut.Products.FindAsync(1);

        product!.UnitPrice = 4m;

        await _sut.SaveChangesAsync();

        product.LastModified.ShouldNotBeNull();
        product.LastModified.ShouldBe(_dateTime);
        product.LastModifiedBy.ShouldBe(_userId);
    }

    public void Dispose()
    {
        _sut?.Dispose();
    }
}