using Heldy.Models;
using Heldy.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<PersonTask>> GetPersonsTasksAsync(int userId);

        Task<IEnumerable<PersonTask>> GetTasksBySubjectAsync(int subjectId, int assgineeId);

        Task CreateTaskAsync(CreateTaskRequest task);
    }
}
