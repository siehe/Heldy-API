using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Models.Requests;
using Newtonsoft.Json;

namespace Heldy.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private DBConfig _dbConfig;

        private const int teacherRoleId = 1;
        private const int studentRoleId = 2;

        public UserRepository()
        {
            _dbConfig = DbHelper.GetConfig();
        }

        public async Task CreateNewTeacher(Person person)
        {
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = await CreateInsertTeacherCommand(person, connection);

            connection.Open();

            await command.ExecuteNonQueryAsync();
        }

        public async Task CreateNewStudent(Person person)
        {
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = await CreateInsertStudentCommand(person, connection);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<Person> GetPerson(int id)
        {
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand("GetPersonById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            connection.Open();

            command.Parameters.AddWithValue("id", id);

            await using var reader = await command.ExecuteReaderAsync();

            var person = DbHelper.CreatePerson(reader);
            return person;
        }

        public async Task<Person> GetPersonByEmail(string email)
        {
            var sqlExpression = "GetPersonByEmail";
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand(sqlExpression, connection) { CommandType = CommandType.StoredProcedure };

            connection.Open();
            command.Parameters.AddWithValue("email", email);

            await using var reader = await command.ExecuteReaderAsync();

            Person res = null;

            if (reader.Read())
            {
                res = CreatePerson(reader);
            }

            return res;
        }

        private async Task<SqlCommand> CreateInsertTeacherCommand(Person person, SqlConnection connection)
        {
            var command = await CreateInsertPersonCommand(person, connection);
            command.Parameters.AddWithValue("role", teacherRoleId);

            return command;
        }

        private async Task<SqlCommand> CreateInsertStudentCommand(Person person, SqlConnection connection)
        {
            var command = await CreateInsertPersonCommand(person, connection);
            command.Parameters.AddWithValue("role", studentRoleId);

            return command;
        }


        private async Task<SqlCommand> CreateInsertPersonCommand(Person person, SqlConnection connection)
        {
            var sqlExpression = "CreatePerson";
            await using var command = new SqlCommand(sqlExpression, connection) {CommandType = CommandType.StoredProcedure};

            command.Parameters.AddWithValue("name", (object)person.Name ?? DBNull.Value);
            command.Parameters.AddWithValue("surname", (object)person.Surname ?? DBNull.Value);
            command.Parameters.AddWithValue("secondName", (object)person.SecondName ?? DBNull.Value);
            command.Parameters.AddWithValue("email", person.Email);
            command.Parameters.AddWithValue("password", person.Password);
            command.Parameters.AddWithValue("dob", person.DOB);

            return command;
        }

        private void SetDOBParameter(SqlCommand command, DateTime DOB)
        {
            if (DOB.Year < 1970)
            {
                command.Parameters.AddWithValue("dob", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("dob", DOB);
            }

        }

        private Person CreatePerson(IDataReader reader)
        {
            var person = new Person();

            person.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            person.Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name"));
            person.Surname = reader.IsDBNull(reader.GetOrdinal("Surname")) ? null : reader.GetString(reader.GetOrdinal("Surname"));
            person.SecondName = reader.IsDBNull(reader.GetOrdinal("SecondName")) ? null : reader.GetString(reader.GetOrdinal("SecondName"));
            person.DOB = reader.IsDBNull(reader.GetOrdinal("DOB")) ? new DateTime() : reader.GetDateTime(reader.GetOrdinal("DOB"));
            person.Email = reader.GetString(reader.GetOrdinal("Email"));
            person.Password = reader.GetString(reader.GetOrdinal("Password"));
            

            return person;
        }

        public async Task UpdatePersonAsync(UpdatePersonRequest updatePersonRequest, int personId)
        {
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand("UpdatePerson", connection) { CommandType = CommandType.StoredProcedure };

            await connection.OpenAsync();

            command.Parameters.AddWithValue("personId", personId);

            if (updatePersonRequest.Name != null)
                command.Parameters.AddWithValue("name", updatePersonRequest.Name);

            if (updatePersonRequest.SecondName != null)
                command.Parameters.AddWithValue("secondName", updatePersonRequest.SecondName);

            if (updatePersonRequest.Surname != null)
                command.Parameters.AddWithValue("surname", updatePersonRequest.Surname);

            if (updatePersonRequest.DOB != null)
                command.Parameters.AddWithValue("dob", updatePersonRequest.DOB);

            await command.ExecuteNonQueryAsync();
        }
    }
}