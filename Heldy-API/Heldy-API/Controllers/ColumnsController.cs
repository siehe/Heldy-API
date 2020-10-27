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
    [Route("columns")]
    public class ColumnsController : Controller
    {
        private IColumnService _columnsService;

        public ColumnsController(IColumnService columnsService)
        {
            _columnsService = columnsService;
        }

        /// <summary>
        /// Getting all existing types of columns columns
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetColumns()
        {
            var columns = await _columnsService.GetColumns();

            return Ok(columns);
        }
    }
}
