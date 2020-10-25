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
using System.Threading;
using System.Threading.Tasks;

namespace Heldy.DataAccess
{
    public class TaskRepository : ITaskRepository
    {
        private DBConfig _dbConfig;

        private string _resourceName = "Heldy.DataAccess.Configs.DBConfig.json";
        private const string ASSIGNEE = "assignee";
        private const string AUTHOR = "author";

        public TaskRepository()
        {
            _dbConfig = GetConfig();
        }

        public async Task<IEnumerable<PersonTask>> GetPersonTasksAsync(int userId)
        {
            var tasks = new List<PersonTask>();
            var config = GetConfig();

            using (var connection = new SqlConnection(_dbConfig.ConnectionString))
            using (var command = new SqlCommand("GetTasksByAssigneeId", connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                command.Parameters.AddWithValue("assigneeId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        await Task.Run(() => tasks.Add(CreateTask(reader)));
                    }
                }

            }

            return tasks;
        }

        public async Task<IEnumerable<PersonTask>> GetTasksBySubject(int subjectId, int assigneeId)
        {
            var tasks = new List<PersonTask>();

            using (var connection = new SqlConnection(_dbConfig.ConnectionString))
            using (var command = new SqlCommand("GetTasksBySubjectId", connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                command.Parameters.AddWithValue("subjectId", subjectId);
                command.Parameters.AddWithValue("assigneeId", assigneeId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        await Task.Run(() => tasks.Add(CreateTask(reader)));
                    }
                }
            }

            return tasks;
        }

        private PersonTask CreateTask(IDataReader reader)
        {
            var task = new PersonTask();

            task.Id = reader.GetInt32(reader.GetOrdinal("taskId"));
            task.Statement = reader.GetString(reader.GetOrdinal("Statement"));
            task.Status = reader.GetString(reader.GetOrdinal("Status"));
            task.Type = reader.GetString(reader.GetOrdinal("Type"));
            task.Deadline = reader.GetDateTime(reader.GetOrdinal("Deadline"));

            task.EctsMark = reader["EctsMark"].ToString();
            task.Comment = reader["Comment"].ToString();

            task.Assignee = CreatePerson(reader, ASSIGNEE);
            task.Author = CreatePerson(reader, AUTHOR);
            task.Subejct = CreateSubject(reader);

            int grade;
            if (int.TryParse(reader["Grade"].ToString(), out grade))
            {
                task.Grade = grade;
            }

            return task;
        }

        private Person CreatePerson(IDataReader reader, string prefix)
        {
            var person = new Person();

            person.Id = reader.GetInt32(reader.GetOrdinal($"{prefix}Id"));
            person.Name = reader.GetString(reader.GetOrdinal($"{prefix}Name"));
            person.SecondName = reader.GetString(reader.GetOrdinal($"{prefix}SecondName"));
            person.Surname = reader.GetString(reader.GetOrdinal($"{prefix}Surname"));
            person.DOB = reader.GetDateTime(reader.GetOrdinal($"{prefix}DOB"));
            person.Email = reader.GetString(reader.GetOrdinal($"{prefix}Email"));

            return person;
        }

        private Subject CreateSubject(IDataReader reader)
        {
            var subject = new Subject();

            subject.Id = reader.GetInt32(reader.GetOrdinal("subjectId"));
            subject.Credits = reader.GetInt32(reader.GetOrdinal("Credits"));
            subject.Title = reader.GetString(reader.GetOrdinal("Title"));

            return subject;
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
    }
}