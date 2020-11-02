using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Models.Requests;
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

        public TaskRepository()
        {
            _dbConfig = DbHelper.GetConfig();
        }

        public async Task CreateTaskAsync(CreateUpdateTaskRequest task)
        {
            using (var connection = new SqlConnection(_dbConfig.ConnectionString))
            using (var command = new SqlCommand("CreateTask", connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                command.Parameters.AddWithValue("statement", task.Statement);
                command.Parameters.AddWithValue("deadline", task.Deadline);
                command.Parameters.AddWithValue("description", task.Description);
                command.Parameters.AddWithValue("subjectId", task.SubjectId);
                command.Parameters.AddWithValue("assigneeId", task.AssigneeId);
                command.Parameters.AddWithValue("authorId", task.AuthorId);
                command.Parameters.AddWithValue("typeId", task.TypeId);
                command.Parameters.AddWithValue("statusId", task.StatusId);

                await Task.Run(() => command.ExecuteNonQuery());
            }
        }

        public async Task<IEnumerable<PersonTask>> GetPersonTasksAsync(int userId)
        {
            var tasks = new List<PersonTask>();

            using (var connection = new SqlConnection(_dbConfig.ConnectionString))
            using (var command = new SqlCommand("GetTasksByAssigneeId", connection) { CommandType = CommandType.StoredProcedure })
            {
                connection.Open();
                command.Parameters.AddWithValue("assigneeId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        await Task.Run(() => tasks.Add(DbHelper.CreateTask(reader)));
                    }
                }

            }

            return tasks;
        }

        public async Task<IEnumerable<PersonTask>> GetTasksBySubjectAsync(int subjectId, int assigneeId)
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
                        await Task.Run(() => tasks.Add(DbHelper.CreateTask(reader)));
                    }
                }
            }

            return tasks;
        }

        
    }
}