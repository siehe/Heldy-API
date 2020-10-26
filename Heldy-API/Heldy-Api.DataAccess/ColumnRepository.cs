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

        public ColumnRepository()
        {
            _dbConfig = DbHelper.GetConfig();
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
                        await Task.Run(() => columns.Add(DbHelper.CreateColumn(reader)));
                    }
                }

            }

            return columns;
        }
    }
}
