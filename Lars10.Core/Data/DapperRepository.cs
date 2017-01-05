using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Dapper;

namespace Lars10.Core.Data
{
    [ExcludeFromCodeCoverage]
    public class DapperRepository : BaseRepository
    {

        protected DapperRepository(string connectionStringName)
            : base(connectionStringName)
        { }

        protected int Execute(string procName, DynamicParameters p)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Execute(procName, p, commandType: CommandType.StoredProcedure);
            }
        }

        protected int Execute(string sql, object param = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Execute(sql, param);
            }
        }

        protected int Insert<T>(IEnumerable<T> records, string tableName) where T : class
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var fields = new StringBuilder();
            var variables = new StringBuilder();

            foreach (var property in ColumnsToMap<T>(tableName))
            {
                fields.Append($", {property} ");
                variables.Append($", @{property} ");
            }

            var insertQuery = $"INSERT INTO {tableName} ({fields.Remove(0, 2)}) VALUES ({variables.Remove(0, 2)})";

            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Execute(insertQuery, records);
            }
        }

        protected IEnumerable<dynamic> GetList(string sql, object param = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query(sql, param).ToList();
            }
        }

        protected IEnumerable<T> GetList<T>(string sql, object param = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<T>(sql, param).ToList();
            }
        }

        protected IEnumerable<TReturn> GetList<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> f, string splitOnFieldName, object param = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query(sql, f, param, splitOn: splitOnFieldName).ToList();
            }
        }

        protected IEnumerable<T> GetList<T>(string storedProcName, CommandType commandType, object param = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<T>(storedProcName, param, commandType: commandType).ToList();
            }
        }

        protected dynamic GetSingleOrDefault(string sql, object param = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query(sql, param).SingleOrDefault();
            }
        }

        protected T GetSingleOrDefault<T>(string sql, object param = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<T>(sql, param).SingleOrDefault();
            }
        }

        protected TReturn GetSingleOrDefault<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> f, string splitOnFieldName, object param = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query(sql, f, param, splitOn: splitOnFieldName).SingleOrDefault();
            }
        }
    }
}
