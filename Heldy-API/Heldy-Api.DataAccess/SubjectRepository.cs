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
    public class SubjectRepository : ISubjectRepository
    {
        private DBConfig _dbConfig;

        public SubjectRepository()
        {
            _dbConfig = DbHelper.GetConfig();
        }

        public async Task<int> CreateSubjectAsync(Subject subject)
        {
            int id = default;

            using (var connection = new SqlConnection(_dbConfig.ConnectionString))
            using (var command = new SqlCommand("CreateSubject", connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();

                command.Parameters.AddWithValue("title", subject.Title);
                command.Parameters.AddWithValue("credits", subject.Credits);

                using(var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        id = (int)reader.GetDecimal(reader.GetOrdinal("createdId"));
                    }
                }
            }

            return id;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync()
        {
            var subjects = new List<Subject>();

            using (var connection = new SqlConnection(_dbConfig.ConnectionString))
            using (var command = new SqlCommand("GetSubjects", connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();

                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        await Task.Run(() => subjects.Add(DbHelper.CreateSubject(reader)));
                    }
                }
            }

            return subjects;
        }
    }
}
