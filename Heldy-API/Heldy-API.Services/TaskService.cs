using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Models.Requests;
using Heldy.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services
{
    public class TaskService : ITaskService
    {
        private ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task CreateTaskAsync(CreateUpdateTaskRequest task)
        {
            await _taskRepository.CreateTaskAsync(task);
        }

        public async Task<IEnumerable<PersonTask>> GetPersonsTasksAsync(int userId)
        {
            var tasks = await _taskRepository.GetPersonTasksAsync(userId);
            return tasks;
        }

        public async Task<IEnumerable<PersonTask>> GetTasksBySubjectAsync(int subjectId, int assigneeId)
        {
            var tasks = await _taskRepository.GetTasksBySubjectAsync(subjectId, assigneeId);
            return tasks;
        }

        public async Task UpdateTaskAsync(CreateUpdateTaskRequest task)
        {
            await _taskRepository.UpdateTaskAsync(task);
        }
    }
}
