using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Newtonsoft.Json;

namespace Heldy.DataAccess
{
    public class UserRepository : IUserRepository
    {

        private DBConfig _dbConfig;

        private string _resourceName = "Heldy.DataAccess.Configs.DBConfig.json";
        private const int teacherRoleId = 1;

        public UserRepository()
        {
            _dbConfig = DbHelper.GetConfig();
        }

        public async Task CreateNewPerson(Person person)
        {
            var sqlExpression = "INSERT INTO Persons(Role, Name, Surname, SecondName, DOB, Email, Password) VALUES(@role, @name, @surname, @secondName, @dob, @email, @password)";
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand(sqlExpression, connection);

            connection.Open();

            var roleParam = new SqlParameter("@role",teacherRoleId);
            var nameParam = new SqlParameter("@name",person.Name);
            var surnameParam = new SqlParameter("@surname",person.Surname);
            var secondNameParam = new SqlParameter("@secondName",person.SecondName);
            var dobParam = new SqlParameter("@dob",person.DOB);
            var emailParam = new SqlParameter("@email",person.Email);
            var passwordParam = new SqlParameter("@password",person.Password);

            command.Parameters.Add(roleParam);
            command.Parameters.Add(nameParam);
            command.Parameters.Add(surnameParam);
            command.Parameters.Add(secondNameParam);
            command.Parameters.Add(dobParam);
            command.Parameters.Add(emailParam);
            command.Parameters.Add(passwordParam);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<Person> GetPersonByEmail(string email)
        {
            var sqlExpression = "SELECT [Id] ,[Role] ,[Name] ,[Surname] ,[SecondName] ,[DOB] ,[Email] ,[Password] FROM Persons WHERE [Email] = @email";
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand(sqlExpression, connection);

            connection.Open();
            command.Parameters.AddWithValue("@email", email);

            await using var reader = await command.ExecuteReaderAsync();

            Person res = null;

            if (reader.Read())
            {
                res = CreatePerson(reader);
            }

            return res;
        }

        private Person CreatePerson(IDataReader reader)
        {
            var person = new Person
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Surname = reader.GetString(reader.GetOrdinal("Surname")),
                SecondName = reader.GetString(reader.GetOrdinal("SecondName")),
                DOB = reader.GetDateTime(reader.GetOrdinal("DOB")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Password = reader.GetString(reader.GetOrdinal("Password"))
            };

            return person;
        }
    }
}