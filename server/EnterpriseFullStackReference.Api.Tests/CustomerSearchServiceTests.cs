using EnterpriseFullStackReference.Api.Contracts.Customers;
using EnterpriseFullStackReference.Api.Data;
using EnterpriseFullStackReference.Api.Models;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnterpriseFullStackReference.Api.Tests;

public sealed class CustomerSearchServiceTests
{
    [Fact]
    public async Task SearchAsync_FiltersAcrossCustomerFieldsAndActiveState()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new CustomerSearchService(db);

        var result = await service.SearchAsync(new CustomerSearchRequest
        {
            Search = "north",
            IsActive = true,
            PageNumber = 1,
            PageSize = 25
        });

        Assert.Single(result.Items);
        Assert.Equal("CUST-1001", result.Items[0].CustomerNumber);
    }

    [Fact]
    public async Task SearchAsync_FiltersByStateCaseInsensitively()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new CustomerSearchService(db);

        var result = await service.SearchAsync(new CustomerSearchRequest { State = "tx", PageSize = 25 });

        Assert.Equal(2, result.TotalCount);
        Assert.All(result.Items, item => Assert.Equal("TX", item.State));
    }

    [Fact]
    public async Task SearchAsync_AppliesPaginationAndCompanySort()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new CustomerSearchService(db);

        var result = await service.SearchAsync(new CustomerSearchRequest
        {
            PageNumber = 2,
            PageSize = 2,
            SortField = "companyName",
            SortDirection = "asc"
        });

        Assert.Equal(4, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("Northwind Industries", result.Items[0].CompanyName);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new ApplicationDbContext(options);
    }

    private static void Seed(ApplicationDbContext db)
    {
        db.Customers.AddRange(
            new Customer { Id = 1, CustomerNumber = "CUST-1001", CompanyName = "Northwind Industries", ContactName = "Avery Stone", Email = "avery@example.test", Phone = "713-555-0101", City = "Houston", State = "TX", CreatedDate = new DateTime(2026, 8, 1), IsActive = true },
            new Customer { Id = 2, CustomerNumber = "CUST-1002", CompanyName = "Contoso Services", ContactName = "Jordan Lee", Email = "jordan@example.test", Phone = "512-555-0102", City = "Austin", State = "TX", CreatedDate = new DateTime(2026, 8, 2), IsActive = true },
            new Customer { Id = 3, CustomerNumber = "CUST-1003", CompanyName = "Fabrikam Group", ContactName = "Morgan Diaz", Email = "morgan@example.test", Phone = "303-555-0103", City = "Denver", State = "CO", CreatedDate = new DateTime(2026, 8, 3), IsActive = true },
            new Customer { Id = 4, CustomerNumber = "CUST-1004", CompanyName = "Tailspin Services", ContactName = "Casey Reed", Email = "casey@example.test", Phone = "206-555-0104", City = "Seattle", State = "WA", CreatedDate = new DateTime(2026, 8, 4), IsActive = false });
        db.SaveChanges();
    }
}
