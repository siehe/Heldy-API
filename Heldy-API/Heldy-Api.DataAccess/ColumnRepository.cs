using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.DataAccess
{
    public class ColumnRepository : IColumnsRepository
    {
        private DBConfig _dbConfig;
        private string _resourceName = "Heldy.DataAccess.Configs.DBConfig.json";

        public ColumnRepository()
        {
            _dbConfig = GetConfig();
        }

        public async Task<IEnumerable<Column>> GetColumns()
        {
            var columns = new List<Column>();

            using (var connection = new SqlConnection(_dbConfig.ConnectionString))
            using (var command = new SqlCommand("GetColumns", connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        await Task.Run(() => columns.Add(CreateColumn(reader)));
                    }
                }

            }

            return columns;
        }

        private DBConfig GetConfig()
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(_resourceName))
            using (var sr = new StreamReader(stream))
            {
                var config = JsonConvert.DeserializeObject<DBConfig>(sr.ReadToEnd());
                return config;
            }
        }

        private Column CreateColumn(IDataReader reader)
        {
            var column = new Column();

            column.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            column.Name = reader.GetString(reader.GetOrdinal("Name"));

            return column;
        }
    }
}
