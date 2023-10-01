using MGH.Domain.Entities;
using MGH.EF.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MGH.EF.Persistence.Contexts;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostConfig).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
}