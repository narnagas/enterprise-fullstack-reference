using EnterpriseFullStackReference.Api.Contracts.Customers;
using EnterpriseFullStackReference.Api.Data;
using EnterpriseFullStackReference.Api.Models;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnterpriseFullStackReference.Api.Tests;

public sealed class CustomerEditorServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ReturnsCustomerDetail()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new CustomerEditorService(db);

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("CUST-1001", result.CustomerNumber);
        Assert.Equal("Northwind Industries", result.CompanyName);
        Assert.Equal("Avery Stone", result.ContactName);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenCustomerDoesNotExist()
    {
        await using var db = CreateDbContext();
        var service = new CustomerEditorService(db);

        var result = await service.GetByIdAsync(404);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEditableFieldsAndPersistsChanges()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new CustomerEditorService(db);

        var result = await service.UpdateAsync(1, new UpdateCustomerRequest
        {
            CompanyName = " Northwind Enterprise ",
            ContactName = " Taylor Brooks ",
            Email = " taylor@example.test ",
            Phone = " 713-555-0199 ",
            City = " Houston ",
            State = " TX ",
            IsActive = false
        });

        Assert.NotNull(result);
        Assert.Equal("Northwind Enterprise", result.CompanyName);
        Assert.Equal("Taylor Brooks", result.ContactName);
        Assert.Equal("taylor@example.test", result.Email);
        Assert.False(result.IsActive);

        var saved = await db.Customers.SingleAsync(x => x.Id == 1);
        Assert.Equal("Northwind Enterprise", saved.CompanyName);
        Assert.Equal("713-555-0199", saved.Phone);
        Assert.False(saved.IsActive);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNullWhenCustomerDoesNotExist()
    {
        await using var db = CreateDbContext();
        var service = new CustomerEditorService(db);

        var result = await service.UpdateAsync(404, new UpdateCustomerRequest
        {
            CompanyName = "Missing Customer",
            IsActive = true
        });

        Assert.Null(result);
    }

    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    private static void Seed(ApplicationDbContext db)
    {
        db.Customers.Add(new Customer
        {
            Id = 1,
            CustomerNumber = "CUST-1001",
            CompanyName = "Northwind Industries",
            ContactName = "Avery Stone",
            Email = "avery@example.test",
            Phone = "713-555-0101",
            City = "Houston",
            State = "TX",
            CreatedDate = new DateTime(2026, 8, 1),
            IsActive = true
        });
        db.SaveChanges();
    }
}
