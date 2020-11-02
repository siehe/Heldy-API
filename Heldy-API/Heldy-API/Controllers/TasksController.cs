using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Heldy.Models;
using Heldy.Models.Requests;
using Heldy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Heldy_API.Controllers
{
    [ApiController]
    [Authorize]
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
            var tasks = await _taskService.GetTasksBySubjectAsync(subjectId, assigneeId);
            return Ok(tasks);
        }

        /// <summary>
        /// Creating tasks
        /// </summary>
        /// <param name="createTask"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateTask(CreateUpdateTaskRequest task)
        {
            if (task.Deadline < DateTime.Now)
            {
                return BadRequest("Invalid deadline date.");
            }

            await _taskService.CreateTaskAsync(task);

            return Ok(HttpStatusCode.Created);
        }

        [HttpPut]
        [Route("{taskId}")]
        public async Task<IActionResult> UpdateTask(CreateUpdateTaskRequest task)
        {
            await _taskService.UpdateTaskAsync(task);

            return Ok(HttpStatusCode.NoContent);
        }
    }
}
