using MGH.EntityFramework.Concrete;

namespace MGH.EntityFramework.Abstract;

public interface IDbContextAccessor
{
    DbContextBase Context { get; }
}