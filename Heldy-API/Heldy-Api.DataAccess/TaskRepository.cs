using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Heldy.DataAccess
{
    public class TaskRepository : ITaskRepository
    {
        private string _connectionString;
        private string _resourceName = "Heldy.DataAccess.Configs.DBConfig.json";

        public async Task<IEnumerable<PersonTask>> GetPersonTasksAsync(int userId)
        {
            //Dummy logic to immitate response from DB
            //TODO: Implement connection to DB and retrieving inforamtion
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(_resourceName))
            using (var reader = new StreamReader(stream))
            {
                var config = reader.ReadToEnd();

                var tasks = new List<PersonTask>()
                {
                    new PersonTask()
                    {
                        Id = 1,
                        Statement = config
                    }
                };

                await Task.Run(() => Thread.Sleep(100));

                return tasks;
            }
        }
    }
}