using Heldy.Models;
using Heldy.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Heldy.DataAccess.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<PersonTask>> GetPersonTasksAsync(int userId);

        Task<int> GetPersonToDoTasksCountAsync(int userId);

        Task<IEnumerable<PersonTask>> GetTasksBySubjectAsync(int subjectId, int assigneeId);

        Task CreateTaskAsync(CreateUpdateTaskRequest task);

        Task UpdateTaskAsync(CreateUpdateTaskRequest task);

        Task DeleteTaskAsync(int taskId);

        Task UpdateGradeAsync(int taskId, int grade);
    }
}
