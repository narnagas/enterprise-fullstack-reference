using EnterpriseFullStackReference.Api.Contracts.Projects;
using EnterpriseFullStackReference.Api.Data;
using EnterpriseFullStackReference.Api.Models;
using EnterpriseFullStackReference.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EnterpriseFullStackReference.Api.Tests;

public sealed class ProjectEditorServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ReturnsProjectDetail()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new ProjectEditorService(db);

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("PRJ-1001", result.ProjectNumber);
        Assert.Equal("North Modernization", result.Name);
        Assert.Equal("Northwind Industries", result.CustomerName);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenProjectDoesNotExist()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new ProjectEditorService(db);

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEditableFieldsAndPersistsChanges()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new ProjectEditorService(db);
        var dueDate = new DateTime(2026, 10, 15);

        var result = await service.UpdateAsync(1, new UpdateProjectRequest
        {
            Name = "  North Platform Modernization  ",
            CustomerName = "  Northwind Enterprise  ",
            Status = "  Planning  ",
            DueDate = dueDate,
            IsActive = false
        });

        Assert.NotNull(result);
        Assert.Equal("North Platform Modernization", result.Name);
        Assert.Equal("Northwind Enterprise", result.CustomerName);
        Assert.Equal("Planning", result.Status);
        Assert.Equal(dueDate, result.DueDate);
        Assert.False(result.IsActive);

        var persisted = await db.Projects.SingleAsync(x => x.Id == 1);
        Assert.Equal("North Platform Modernization", persisted.Name);
        Assert.Equal("Northwind Enterprise", persisted.CustomerName);
        Assert.Equal("Planning", persisted.Status);
        Assert.Equal(dueDate, persisted.DueDate);
        Assert.False(persisted.IsActive);
        Assert.Equal("PRJ-1001", persisted.ProjectNumber);
        Assert.Equal(new DateTime(2026, 9, 1), persisted.CreatedDate);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNullWhenProjectDoesNotExist()
    {
        await using var db = CreateDbContext();
        Seed(db);
        var service = new ProjectEditorService(db);

        var result = await service.UpdateAsync(999, new UpdateProjectRequest
        {
            Name = "Missing Project",
            CustomerName = "Example Customer",
            Status = "Active",
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
        db.Projects.Add(new Project
        {
            Id = 1,
            ProjectNumber = "PRJ-1001",
            Name = "North Modernization",
            CustomerName = "Northwind Industries",
            Status = "Active",
            CreatedDate = new DateTime(2026, 9, 1),
            DueDate = new DateTime(2026, 9, 30),
            IsActive = true
        });

        db.SaveChanges();
    }
}
