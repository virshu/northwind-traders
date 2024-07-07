using AutoMapper;
using Northwind.Application.Categories.Queries.GetCategoriesList;
using Northwind.Application.Customers.Queries.GetCustomerDetail;
using Northwind.Application.Customers.Queries.GetCustomersList;
using Northwind.Application.Employees.Queries.GetEmployeeDetail;
using Northwind.Application.Employees.Queries.GetEmployeesList;
using Northwind.Application.Products.Queries.GetProductDetail;
using Northwind.Application.Products.Queries.GetProductsFile;
using Northwind.Application.Products.Queries.GetProductsList;
using Northwind.Domain.Entities;
using Shouldly;
using Xunit;

namespace Northwind.Application.UnitTests.Mappings;

public class MappingTests : IClassFixture<MappingTestsFixture>
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests(MappingTestsFixture fixture)
    {
        _configuration = fixture.ConfigurationProvider;
        _mapper = fixture.Mapper;
    }

    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void ShouldMapCategoryToCategoryDto()
    {
        Category entity = new Category();

        CategoryDto result = _mapper.Map<CategoryDto>(entity);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<CategoryDto>();
    }

    [Fact]
    public void ShouldMapCustomerToCustomerLookupDto()
    {
        Customer entity = new Customer();

        CustomerLookupDto result = _mapper.Map<CustomerLookupDto>(entity);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<CustomerLookupDto>();
    }

    [Fact]
    public void ShouldMapProductToProductDetailVm()
    {
        Product entity = new Product();

        ProductDetailVm result = _mapper.Map<ProductDetailVm>(entity);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProductDetailVm>();
    }

    [Fact]
    public void ShouldMapProductToProductDto()
    {
        Product entity = new Product();

        ProductDto result = _mapper.Map<ProductDto>(entity);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProductDto>();
    }

    [Fact]
    public void ShouldMapProductToProductRecordDto()
    {
        Product entity = new Product();

        ProductRecordDto result = _mapper.Map<ProductRecordDto>(entity);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProductRecordDto>();
    }

    [Fact]
    public void ShouldMapCustomerToCustomerDetailVm()
    {
        Customer entity = new Customer();

        CustomerDetailVm result = _mapper.Map<CustomerDetailVm>(entity);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<CustomerDetailVm>();
    }

    [Fact]
    public void ShouldMapEmployeeToEmployeeLookupDto()
    {
        Employee entity = new Employee();

        EmployeeLookupDto result = _mapper.Map<EmployeeLookupDto>(entity);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<EmployeeLookupDto>();
    }

    [Fact]
    public void ShouldMapEmployeeTerritoryToEmployeeTerritoryDto()
    {
        EmployeeTerritory entity = new EmployeeTerritory();

        EmployeeTerritoryDto result = _mapper.Map<EmployeeTerritoryDto>(entity);

        result.ShouldNotBeNull();
        result.ShouldBeOfType<EmployeeTerritoryDto>();
    }
}