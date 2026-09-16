using EnterpriseFullStackReference.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseFullStackReference.Api.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var project = modelBuilder.Entity<Project>();

        project.HasKey(x => x.Id);
        project.Property(x => x.ProjectNumber).HasMaxLength(50).IsRequired();
        project.Property(x => x.Name).HasMaxLength(150).IsRequired();
        project.Property(x => x.CustomerName).HasMaxLength(150).IsRequired();
        project.Property(x => x.Status).HasMaxLength(50).IsRequired();

        project.HasIndex(x => x.ProjectNumber).IsUnique();
        project.HasIndex(x => x.CustomerName);
        project.HasIndex(x => x.Status);
    }
}
