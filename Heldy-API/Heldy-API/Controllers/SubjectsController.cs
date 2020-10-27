using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
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
    }
}
