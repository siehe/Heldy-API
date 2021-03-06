﻿using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Models.Requests;
using Heldy.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.Services
{
    public class TaskService : ITaskService
    {
        private ITaskRepository _taskRepository;
        private IEmailService emailService;

        public TaskService(ITaskRepository taskRepository, IEmailService emailService)
        {
            _taskRepository = taskRepository;
            this.emailService = emailService;
        }

        public async Task CreateTaskAsync(CreateUpdateTaskRequest task)
        {
            await _taskRepository.CreateTaskAsync(task);
            var tasksCount = await _taskRepository.GetPersonToDoTasksCountAsync(task.AssigneeId);
            if (tasksCount >= 10)
            {
                await emailService.NotifyStudentAboutBigAmountOfTasksAsync(task.AssigneeId);
            }
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            await _taskRepository.DeleteTaskAsync(taskId);
        }

        public async Task UpdateGradeAsync(UpdateGradeRequest request)
        {
            await _taskRepository.UpdateGradeAsync(request.Id, request.Grade);
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
