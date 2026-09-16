using EnterpriseFullStackReference.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseFullStackReference.Api.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Customer> Customers => Set<Customer>();

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

        var customer = modelBuilder.Entity<Customer>();
        customer.HasKey(x => x.Id);
        customer.Property(x => x.CustomerNumber).HasMaxLength(50).IsRequired();
        customer.Property(x => x.CompanyName).HasMaxLength(150).IsRequired();
        customer.Property(x => x.ContactName).HasMaxLength(150);
        customer.Property(x => x.Email).HasMaxLength(200);
        customer.Property(x => x.Phone).HasMaxLength(30);
        customer.Property(x => x.City).HasMaxLength(100);
        customer.Property(x => x.State).HasMaxLength(50);
        customer.HasIndex(x => x.CustomerNumber).IsUnique();
        customer.HasIndex(x => x.CompanyName);
        customer.HasIndex(x => x.State);
    }
}
