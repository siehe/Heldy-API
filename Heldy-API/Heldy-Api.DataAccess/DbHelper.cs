using Heldy.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace Heldy.DataAccess
{
    public static class DbHelper
    {
        private static string _resourceName = "Heldy.DataAccess.Configs.DBConfig.json";
        private static string AUTHOR = "author";
        private static string ASSIGNEE = "assignee";

        public static DBConfig GetConfig()
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(_resourceName))
            using (var sr = new StreamReader(stream))
            {
                var config = JsonConvert.DeserializeObject<DBConfig>(sr.ReadToEnd());
                return config;
            }
        }

        public static PersonTask CreateTask(IDataReader reader)
        {
            var task = new PersonTask();

            task.Id = reader.GetInt32(reader.GetOrdinal("taskId"));
            task.Statement = reader.GetString(reader.GetOrdinal("Statement"));
            task.Deadline = reader.GetDateTime(reader.GetOrdinal("Deadline"));

            task.EctsMark = reader["EctsMark"].ToString();
            task.Comment = reader["Comment"].ToString();
            task.Description = reader["Description"].ToString();

            task.Assignee = CreatePerson(reader, ASSIGNEE);
            task.Author = CreatePerson(reader, AUTHOR);
            task.Subejct = CreateSubject(reader);
            task.Status = CreateStatus(reader);
            task.Type = CreateType(reader);

            int grade;
            if (int.TryParse(reader["Grade"].ToString(), out grade))
            {
                task.Grade = grade;
            }

            return task;
        }

        public static Person CreatePerson(IDataReader reader, string prefix)
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

        public static Subject CreateSubject(IDataReader reader)
        {
            var subject = new Subject();

            subject.Id = reader.GetInt32(reader.GetOrdinal("subjectId"));
            subject.Credits = reader.GetInt32(reader.GetOrdinal("Credits"));
            subject.Title = reader.GetString(reader.GetOrdinal("Title"));

            return subject;
        }

        public static Column CreateStatus(IDataReader reader)
        {
            var column = new Column();

            column.Id = reader.GetInt32(reader.GetOrdinal("statusId"));
            column.Name = reader.GetString(reader.GetOrdinal("Status"));

            return column;
        }

        public static TaskType CreateType(IDataReader reader)
        {
            var type = new TaskType();

            type.Id = reader.GetInt32(reader.GetOrdinal("typeId"));
            type.Name = reader.GetString(reader.GetOrdinal("Type"));

            return type;
        }


        public static Column CreateColumn(IDataReader reader)
        {
            var column = new Column();

            column.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            column.Name = reader.GetString(reader.GetOrdinal("Name"));

            return column;
        }
    }
}
