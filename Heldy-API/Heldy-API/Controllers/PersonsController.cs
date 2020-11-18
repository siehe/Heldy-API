using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Heldy.Models.Requests;
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
        private IUserService _userService;

        public PersonsController(IPersonService personService, IUserService userService)
        {
            _personService = personService;
            _userService = userService;
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

        [HttpPut]
        [Route("{personId}")]
        public async Task<IActionResult> UpdatePerson(UpdatePersonRequest updatePersonRequest, int personId)
        {
            await _userService.UpdatePersonAsync(updatePersonRequest, personId);

            return Ok(HttpStatusCode.OK);
        }
    }
}
