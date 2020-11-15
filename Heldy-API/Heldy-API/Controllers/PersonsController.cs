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
    [Route("persons")]
    public class PersonsController : Controller
    {
        private IPersonService _personService;

        public PersonsController(IPersonService personService)
        {
            _personService = personService;
        }

        /// <summary>
        /// Get all persons of specified role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPersonsByRoleId(int roleId)
        {
            var persons = await _personService.GetPersonsAsync(roleId);
            return Ok(persons);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPerson(int id)
        {
            var person = await _personService.GetPersonAsync(id);
            return Ok(person);
        }
    }
}
