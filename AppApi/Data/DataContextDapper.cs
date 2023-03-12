using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AppApi.Data;

public class DataContextDapper
{
    private readonly IConfiguration _configuration;

    public DataContextDapper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<T> LoadData<T>(string sqlQueary,object? param)
    {
        using(IDbConnection dapper= new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            if(param != null)
            {
                return dapper.Query<T>(sqlQueary, param);
            }
            else
            {
                return dapper.Query<T>(sqlQueary);
            }
        }
    }

    public T LoadDataSingle<T>(string sqlQueary, object? param)
    {
        using (IDbConnection dapper = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            if (param != null)
            {
                return dapper.QuerySingle<T>(sqlQueary, param);
            }
            else
            {
                return dapper.QuerySingle<T>(sqlQueary);
            }
        }
    }

    public bool ExecuteSql(string sql,object param)
    {
        using (IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            if (param != null)
            {
                return conn.Execute(sql, param) > 0;
            }
            else
            {
                return conn.Execute(sql) > 0;
            }
        }
    }

    public int ExecuteSqlWithRowCount(string sql, object param)
    {
        using (IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            if (param != null)
            {
                return conn.Execute(sql, param);
            }
            else
            {
                return conn.Execute(sql);
            }
        }
    }
}
