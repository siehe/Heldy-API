using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heldy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Heldy_API.Controllers
{
    [ApiController]
    [Route("types")]
    public class TypesController : Controller
    {
        private ITypeService _typeService;

        public TypesController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetTypes()
        {
            var types = await _typeService.GetTypesAsync();
            return Ok(types);
        }
    }
}
