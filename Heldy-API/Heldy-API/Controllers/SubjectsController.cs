using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("subjects")]
    public class SubjectsController : Controller
    {
        private ISubjectService _subjectService;
        private ITaskService _taskService;

        public SubjectsController(ISubjectService subjectService, ITaskService taskService)
        {
            _subjectService = subjectService;
            _taskService = taskService;
        }

        /// <summary>
        /// Gets all subjects available
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = await _subjectService.GetSubjectsAsync();
            return Ok(subjects);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseRequest createCourseRequest)
        {
            var tasks = createCourseRequest.Tasks;
            var subject = new Subject()
            {
                Credits = createCourseRequest.Credits,
                Title = createCourseRequest.Title
            };

            var subjectId = await _subjectService.CreateSubjectAsync(subject);

            foreach(var task in tasks)
            {
                task.SubjectId = subjectId;
                await _taskService.CreateTaskAsync(task);
            }

            return Created("/subjects", "Successfully created");
        }
    }
}
