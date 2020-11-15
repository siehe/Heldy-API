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
    public class PersonRepository : IPersonRepository
    {
        private DBConfig _dbConfig;

        public PersonRepository()
        {
            _dbConfig = DbHelper.GetConfig();
        }

        public async Task<IEnumerable<Person>> GetPersonsAsync(int roleId)
        {
            var persons = new List<Person>();

            using (var connection = new SqlConnection(_dbConfig.ConnectionString))
            using (var command = new SqlCommand("GetPersonsByRoleId", connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                command.Parameters.AddWithValue("roleId", roleId);

                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        await Task.Run(() => persons.Add(DbHelper.CreatePerson(reader)));
                    }
                }
            }

            return persons;
        }

        public async Task<Person> GetPersonAsync(int id)
        {
            var person = new Person();

            using (var connection = new SqlConnection(_dbConfig.ConnectionString))
            using (var command = new SqlCommand("GetPersonById", connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                command.Parameters.AddWithValue("Id", id);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        await Task.Run(() => person = DbHelper.CreatePerson(reader));
                    }
                }
            }

            return person;
        }
    }
}
