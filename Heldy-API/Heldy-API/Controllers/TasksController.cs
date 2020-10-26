using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Heldy.Models;
using Heldy.Services.Interfaces;
using Heldy_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Heldy_API.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : Controller
    {
        private ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Get all users tasks by userId. Test values: userId = 2;
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetAllUserTasksAsync(int userId)
        {
            var tasks = await _taskService.GetPersonsTasksAsync(userId);
            return Ok(tasks);
        }

        /// <summary>
        /// Getting all persons tasks by subjectId and personId. Test values: subjectId є [1;4]; assigneeId = 2
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="assigneeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{subjectId}/persons/{assigneeId}")]
        public async Task<IActionResult> GetTasksBySubject(int subjectId, int assigneeId)
        {
            var tasks = await _taskService.GetTasksBySubject(subjectId, assigneeId);
            return Ok(tasks);
        }

        /// <summary>
        /// Creating tasks
        /// </summary>
        /// <param name="createTask"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTask(CreateTaskRequest createTask)
        {
            var task = new PersonTask()
            {
                Statement = createTask.Statement,
                Deadline = createTask.Deadline,
                Description = createTask.Description,
                Subejct = new Subject()
                {
                    Id = createTask.SubjectId
                },
                Status = new Column()
                {
                    Id = createTask.StatusId
                },
                Assignee = new Person()
                {
                    Id = createTask.AssigneId
                },
                Author = new Person()
                {
                    Id = createTask.AuthorId
                },
                Type = new TaskType()
                {
                    Id = createTask.TypeId
                }
            };

            await _taskService.CreateTask(task);

            return Ok(HttpStatusCode.Created);
        }
    }
}
