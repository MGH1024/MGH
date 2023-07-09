using MGH.EntityFramework.Abstract;

namespace MGH.EntityFramework.Concrete;

public class DbContextAccessor : IDbContextAccessor
{
    public DbContextBase Context { get; }

    public DbContextAccessor(DbContextBase context)
    {
        Context = context;
    }
}