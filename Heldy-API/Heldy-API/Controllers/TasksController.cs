using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heldy.Models;
using Heldy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Heldy_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : Controller
    {
        private ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Route("tasks/{userId}")]
        public async Task<IActionResult> GetAllUserTasksAsync(int userId)
        {
            var tasks = await _taskService.GetPersonsTasksAsync(userId);
            return Ok(tasks);
        }
    }
}
