namespace MGH.Core.Infrastructure.Persistence.Persistence.Base;

public interface IQuery<T>
{
    IQueryable<T> Query();
}
