using EnterpriseFullStackReference.Api.Contracts.Projects;
using EnterpriseFullStackReference.Api.Data;
using EnterpriseFullStackReference.Api.Models;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnterpriseFullStackReference.Api.Tests;

public sealed class ProjectSearchServiceTests
{
    [Fact]
    public async Task SearchAsync_FiltersBySearchAndActiveState()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new ProjectSearchService(db);

        var result = await service.SearchAsync(new ProjectSearchRequest
        {
            Search = "north",
            IsActive = true,
            PageNumber = 1,
            PageSize = 25
        });

        Assert.Single(result.Items);
        Assert.Equal("PRJ-1001", result.Items[0].ProjectNumber);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task SearchAsync_AppliesPagination()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new ProjectSearchService(db);

        var result = await service.SearchAsync(new ProjectSearchRequest
        {
            PageNumber = 2,
            PageSize = 2,
            SortField = "projectNumber",
            SortDirection = "asc"
        });

        Assert.Equal(2, result.PageNumber);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(4, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("PRJ-1003", result.Items[0].ProjectNumber);
    }

    [Fact]
    public async Task SearchAsync_SortsDescendingByCustomer()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new ProjectSearchService(db);

        var result = await service.SearchAsync(new ProjectSearchRequest
        {
            SortField = "customerName",
            SortDirection = "desc",
            PageSize = 25
        });

        Assert.Equal("Tailspin Services", result.Items[0].CustomerName);
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
        db.Projects.AddRange(
            new Project { Id = 1, ProjectNumber = "PRJ-1001", Name = "North Modernization", CustomerName = "Northwind Industries", Status = "Active", CreatedDate = new DateTime(2026, 9, 1), IsActive = true },
            new Project { Id = 2, ProjectNumber = "PRJ-1002", Name = "Portal Upgrade", CustomerName = "Contoso Services", Status = "Active", CreatedDate = new DateTime(2026, 9, 2), IsActive = true },
            new Project { Id = 3, ProjectNumber = "PRJ-1003", Name = "Reporting Refresh", CustomerName = "Fabrikam Group", Status = "Planning", CreatedDate = new DateTime(2026, 9, 3), IsActive = true },
            new Project { Id = 4, ProjectNumber = "PRJ-1004", Name = "Legacy Migration", CustomerName = "Tailspin Services", Status = "Completed", CreatedDate = new DateTime(2026, 9, 4), IsActive = false });
        db.SaveChanges();
    }
}
