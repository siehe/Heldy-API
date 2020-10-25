using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.DataAccess
{
    public class TypeRepository : ITypeRepository
    {
        private DBConfig _dbConfig;

        public TypeRepository()
        {
            _dbConfig = DbHelper.GetConfig();
        }

        public async Task<IEnumerable<TaskType>> GetTypesAsync()
        {
            var types = new List<TaskType>();

            using (var connection = new SqlConnection(_dbConfig.ConnectionString))
            using (var command = new SqlCommand("GetTypes", connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        await Task.Run(() => types.Add(DbHelper.CreateType(reader)));
                    }
                }
            }

            return types;
        }
    }
}
