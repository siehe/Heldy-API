﻿using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand("CreateTask", connection) { CommandType = CommandType.StoredProcedure };
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

        public async Task DeleteTaskAsync(int taskId)
        {
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand("DeleteTask", connection) { CommandType = CommandType.StoredProcedure };
            
            await connection.OpenAsync();

            command.Parameters.AddWithValue("taskId", taskId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateGradeAsync(int taskId, int grade)
        {
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand("UpdateGradeTask", connection) { CommandType = CommandType.StoredProcedure };
            connection.Open();
            command.Parameters.AddWithValue("taskId", taskId);
            command.Parameters.AddWithValue("grade", grade);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<PersonTask>> GetPersonTasksAsync(int userId)
        {
            var tasks = new List<PersonTask>();

            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand("GetTasksByAssigneeId", connection) { CommandType = CommandType.StoredProcedure };
            connection.Open();
            command.Parameters.AddWithValue("assigneeId", userId);

            await using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                await Task.Run(() => tasks.Add(DbHelper.CreateTask(reader)));
            }

            return tasks;
        }

        public async Task<int> GetPersonToDoTasksCountAsync(int userId)
        {
            var count = 0;

            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand("GetPersonToDoTasksCount", connection) { CommandType = CommandType.StoredProcedure };
            connection.Open();
            command.Parameters.AddWithValue("Id", userId);

            await using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                await Task.Run(() => count = reader.GetInt32(reader.GetOrdinal("count")));
            }

            return count;
        }

        public async Task<IEnumerable<PersonTask>> GetTasksBySubjectAsync(int subjectId, int assigneeId)
        {
            var tasks = new List<PersonTask>();

            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand("GetTasksBySubjectId", connection) { CommandType = CommandType.StoredProcedure };
            connection.Open();
            command.Parameters.AddWithValue("subjectId", subjectId);
            command.Parameters.AddWithValue("assigneeId", assigneeId);

            await using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                tasks.Add(DbHelper.CreateTask(reader));
            }

            return tasks;
        }

        public async Task UpdateTaskAsync(CreateUpdateTaskRequest task)
        {
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand("UpdateTask", connection) { CommandType = CommandType.StoredProcedure };
            connection.Open();
            command.Parameters.AddWithValue("taskId", task.Id);
            command.Parameters.AddWithValue("deadline", task.Deadline);
            command.Parameters.AddWithValue("description", task.Description);
            command.Parameters.AddWithValue("statement", task.Statement);
            command.Parameters.AddWithValue("statusId", task.StatusId);
            command.Parameters.AddWithValue("subjectId", task.SubjectId);
            command.Parameters.AddWithValue("typeId", task.TypeId);
            command.Parameters.AddWithValue("isInQa", task.IsInQa);

            await command.ExecuteNonQueryAsync();
        }
    }
}