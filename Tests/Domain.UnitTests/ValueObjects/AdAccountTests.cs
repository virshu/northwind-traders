using Northwind.Domain.Exceptions;
using Northwind.Domain.ValueObjects;
using Xunit;

namespace Northwind.Domain.UnitTests.ValueObjects;

public class AdAccountTests
{
    [Fact]
    public void ShouldHaveCorrectDomainAndName()
    {
        AdAccount account = AdAccount.For("SSW\\Jason");

        Assert.Equal("SSW", account.Domain);
        Assert.Equal("Jason", account.Name);
    }

    [Fact]
    public void ToStringReturnsCorrectFormat()
    {
        const string value = "SSW\\Jason";

        AdAccount account = AdAccount.For(value);

        Assert.Equal(value, account.ToString());
    }

    [Fact]
    public void ImplicitConversionToStringResultsInCorrectString()
    {
        const string value = "SSW\\Jason";

        AdAccount account = AdAccount.For(value);

        string result = account;

        Assert.Equal(value, result);
    }

    [Fact]
    public void ExplicitConversionFromStringSetsDomainAndName()
    {
        AdAccount account = (AdAccount) "SSW\\Jason";

        Assert.Equal("SSW", account.Domain);
        Assert.Equal("Jason", account.Name);
    }

    [Fact]
    public void ShouldThrowAdAccountInvalidExceptionForInvalidAdAccount()
    {
        Assert.Throws<AdAccountInvalidException>(() => (AdAccount) "SSWJason");
    }
}