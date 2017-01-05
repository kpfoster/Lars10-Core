using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Lars10.Core.Data
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseRepository
    {
        protected BaseRepository(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            ConnectionString = connectionString;
        }

        public int BulkInsert<T>(IEnumerable<T> data, string tableToInsert, bool generateSchema = true) where T : class
        {
            int recordsInserted;

            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableToInsert;

                    if (generateSchema)
                    {
                        foreach (var column in ColumnsToMap<T>(tableToInsert))
                        {
                            bulkCopy.ColumnMappings.Add(column, column);
                        }
                    }

                    connection.Open();

                    var bulkData = data.ToList();

                    using (var reader = new GenericListDataReader<T>(bulkData))
                    {
                        bulkCopy.WriteToServer(reader);
                    }

                    recordsInserted = bulkData.Count;
                }
            }

            return recordsInserted;
        }

        protected IEnumerable<string> ColumnsToMap<T>(string tableToInsert) where T : class
        {
            var properties = typeof(T).GetProperties().Select(s => s.Name);

            return GetSchema(tableToInsert).Intersect(properties, StringComparer.OrdinalIgnoreCase);
        }

        protected IEnumerable<string> GetSchema(string tableName)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_Columns";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@table_name", SqlDbType.NVarChar, 384).Value = tableName;

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return (string)reader["column_name"];
                        }
                    }
                }
            }
        }

        protected string ConnectionString { get; }
    }
}
