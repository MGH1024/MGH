using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace MGH.Core.Persistence.Dapper;

public class DapperService(string connString) : IDapperService, IDisposable
{
    public object ExecuteScalar(string commandText,
        object param = null,
        SqlTransaction transaction = null,
        int? commandTimeout = null)
    {
        using IDbConnection db = new SqlConnection(connString);
        return db.ExecuteScalar(commandText,
            param: param,
            transaction: transaction,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }


    public int Execute(string commandText,
        object param = null,
        SqlTransaction transaction = null,
        int? commandTimeout = null,
        bool isSp = false)
    {
        using IDbConnection db = new SqlConnection(connString);
        if (isSp)
        {
            return db.Execute(commandText,
                param: param,
                transaction: transaction,
                commandTimeout: commandTimeout,
                commandType: CommandType.StoredProcedure);
        }
        else
        {
            return db.Execute(commandText, param);
        }
    }

    public IEnumerable<TEntity> Query<TEntity>(string commandText,
        object param = null) where TEntity : class
    {
        using IDbConnection db = new SqlConnection(connString);
        return db.Query<TEntity>(commandText, param: param);
    }

    public IEnumerable<TEntity> QuerySp<TEntity>(string storedProcedure,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        int? commandTimeout = null) where TEntity : class
    {
        using IDbConnection db = new SqlConnection(connString);
        return db.Query<TEntity>(storedProcedure,
            param: param,
            transaction: transaction,
            buffered: buffered,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<TEntity>> QuerySpAsync<TEntity>(string storedProcedure,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        int? commandTimeout = null) where TEntity : class
    {
        using IDbConnection db = new SqlConnection(connString);
        var res = await db.QueryAsync<TEntity>(storedProcedure,
            param: param,
            transaction: transaction,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
        return res;
    }

    public IEnumerable<T1> QueryOneToMany<T1, T2>(string storedProcedure,
        Func<T1, T2, T1> map,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        string splitOn = "",
        int? commandTimeout = null) where T1 : class
        where T2 : class
    {
        using IDbConnection db = new SqlConnection(connString);
        return db.Query(storedProcedure,
            map: map,
            param: param,
            transaction: transaction,
            buffered: buffered,
            splitOn: splitOn,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }

    public IEnumerable<T1> QuerySp<T1, T2>(string storedProcedure,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        string splitOn = "",
        int? commandTimeout = null)
        where T1 : class
        where T2 : class
    {
        using IDbConnection db = new SqlConnection(connString);
        return db.Query<T1, T2, T1>(storedProcedure,
            (arg, t2) =>
            {
                arg.GetType().GetProperty(typeof(T2).Name)?.SetValue(arg, t2);
                return arg;
            },
            param: param,
            transaction: transaction,
            buffered: buffered,
            splitOn: splitOn,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }

    public IEnumerable<T1> QuerySp<T1, T2, T3>(string storedProcedure,
        object param = null,
        dynamic outParam = null,
        SqlTransaction transaction = null,
        bool buffered = true,
        string splitOn = "",
        int? commandTimeout = null)
        where T1 : class
        where T2 : class
        where T3 : class
    {
        using IDbConnection db = new SqlConnection(connString);
        return db.Query<T1, T2, T3, T1>(storedProcedure,
            (arg, t2, t3) =>
            {
                arg.GetType().GetProperty(typeof(T2).Name)?.SetValue(arg, t2);
                arg.GetType().GetProperty(typeof(T3).Name)?.SetValue(arg, t3);
                return arg;
            },
            param: param,
            transaction: transaction,
            buffered: buffered,
            splitOn: splitOn,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }

    public IEnumerable<T1> QuerySp<T1, T2, T3, T4>(string storedProcedure,
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
    {
        using IDbConnection db = new SqlConnection(connString);
        return db.Query<T1, T2, T3, T4, T1>(storedProcedure,
            (arg, t2, t3, t4) =>
            {
                arg.GetType().GetProperty(typeof(T2).Name)?.SetValue(arg, t2);
                arg.GetType().GetProperty(typeof(T3).Name)?.SetValue(arg, t3);
                arg.GetType().GetProperty(typeof(T4).Name)?.SetValue(arg, t4);
                return arg;
            },
            param: param,
            transaction: transaction,
            buffered: buffered,
            splitOn: splitOn,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }

    public IEnumerable<T1> QuerySp<T1, T2, T3, T4, T5>(string storedProcedure,
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
    {
        using IDbConnection db = new SqlConnection(connString);
        return db.Query<T1, T2, T3, T4, T5, T1>(storedProcedure,
            (arg, t2, t3, t4, t5) =>
            {
                arg.GetType().GetProperty(typeof(T2).Name)?.SetValue(arg, t2);
                arg.GetType().GetProperty(typeof(T3).Name)?.SetValue(arg, t3);
                arg.GetType().GetProperty(typeof(T4).Name)?.SetValue(arg, t4);
                arg.GetType().GetProperty(typeof(T5).Name)?.SetValue(arg, t5);
                return arg;
            },
            param: param,
            transaction: transaction,
            buffered: buffered,
            splitOn: splitOn,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }

    public IEnumerable<T1> QuerySp<T1, T2, T3, T4, T5, T6>(string storedProcedure,
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
    {
        using IDbConnection db = new SqlConnection(connString);
        return db.Query<T1, T2, T3, T4, T5, T6, T1>(storedProcedure,
            (arg, t2, t3, t4, t5, t6) =>
            {
                arg.GetType().GetProperty(typeof(T2).Name)?.SetValue(arg, t2);
                arg.GetType().GetProperty(typeof(T3).Name)?.SetValue(arg, t3);
                arg.GetType().GetProperty(typeof(T4).Name)?.SetValue(arg, t4);
                arg.GetType().GetProperty(typeof(T5).Name)?.SetValue(arg, t5);
                arg.GetType().GetProperty(typeof(T6).Name)?.SetValue(arg, t6);
                return arg;
            },
            param: param,
            transaction: transaction,
            buffered: buffered,
            splitOn: splitOn,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }

    public IEnumerable<T1> QuerySp<T1, T2, T3, T4, T5, T6, T7>(string storedProcedure,
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
        where T7 : class
    {
        using IDbConnection db = new SqlConnection(connString);
        return db.Query<T1, T2, T3, T4, T5, T6, T7, T1>(storedProcedure,
            (arg, t2, t3, t4, t5, t6, t7) =>
            {
                arg.GetType().GetProperty(typeof(T2).Name)?.SetValue(arg, t2);
                arg.GetType().GetProperty(typeof(T3).Name)?.SetValue(arg, t3);
                arg.GetType().GetProperty(typeof(T4).Name)?.SetValue(arg, t4);
                arg.GetType().GetProperty(typeof(T5).Name)?.SetValue(arg, t5);
                arg.GetType().GetProperty(typeof(T6).Name)?.SetValue(arg, t6);
                arg.GetType().GetProperty(typeof(T7).Name)?.SetValue(arg, t7);
                return arg;
            },
            param: param,
            transaction: transaction,
            buffered: buffered,
            splitOn: splitOn,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}