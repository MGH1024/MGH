using MGH.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MGH.EF.Persistence.Contexts;

public class AppDbContext : DbContext
{
    //way one
    // public ApplicationDbContext()
    // {
    // }
    //
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(
    //         "Data Source=localhost,14333;Initial Catalog=dbMGH.EF.Review;User ID=sa; Password=Abcde@12345;Integrated Security=false;MultipleActiveResultSets=true;Encrypt=false");
    // } 
    
    //way two
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Person> People { get; set; }
}