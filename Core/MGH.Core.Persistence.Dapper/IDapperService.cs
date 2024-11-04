using Microsoft.Data.SqlClient;

namespace MGH.Core.Persistence.Dapper;

public interface IDapperService
{
    object ExecuteScalar(string commandText,
        object param = null!,
        SqlTransaction transaction = null,
        int? commandTimeout = null);

    int Execute(string commandText,
        object param = null,
        SqlTransaction transaction = null,
        int? commandTimeout = null,
        bool isSp = false);

    IEnumerable<TEntity> Query<TEntity>(string commandText,
        object param = null) where TEntity : class;

    IEnumerable<TEntity> QuerySp<TEntity>(string storedProcedure,
        dynamic param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        int? commandTimeout = null) where TEntity : class;

    Task<IEnumerable<TEntity>> QuerySpAsync<TEntity>(string storedProcedure,
        dynamic param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        int? commandTimeout = null) where TEntity : class;

    IEnumerable<T1> QueryOneToMany<T1, T2>(string storedProcedure,
        Func<T1, T2, T1> map,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        string splitOn = "",
        int? commandTimeout = null)
        where T1 : class
        where T2 : class;


    IEnumerable<T1> QuerySp<T1, T2>(string storedProcedure,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        string splitOn = "",
        int? commandTimeout = null)
        where T1 : class
        where T2 : class;

    IEnumerable<T1> QuerySp<T1, T2, T3>(string storedProcedure,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        string splitOn = "",
        int? commandTimeout = null)
        where T1 : class
        where T2 : class
        where T3 : class;

    IEnumerable<T1> QuerySp<T1, T2, T3, T4>(string storedProcedure,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        string splitOn = "",
        int? commandTimeout = null)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class;

    IEnumerable<T1> QuerySp<T1, T2, T3, T4, T5>(string storedProcedure,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        string splitOn = "",
        int? commandTimeout = null)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class;

    IEnumerable<T1> QuerySp<T1, T2, T3, T4, T5, T6>(string storedProcedure,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        string splitOn = "",
        int? commandTimeout = null)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class;

    IEnumerable<T1> QuerySp<T1, T2, T3, T4, T5, T6, T7>(string storedProcedure,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        string splitOn = "",
        int? commandTimeout = null)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class;
}