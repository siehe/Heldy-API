﻿using System;
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
            SetDOBParameter(command, person.DOB);

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