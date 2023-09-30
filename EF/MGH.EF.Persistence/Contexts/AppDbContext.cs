using MGH.Domain.Entities;
using MGH.EF.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MGH.EF.Persistence.Contexts;

public class AppDbContext : DbContext
{ 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerConfig).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}