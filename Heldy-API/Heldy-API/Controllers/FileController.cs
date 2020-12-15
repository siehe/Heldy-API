using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Heldy_API.Controllers
{
    [ApiController]
    [Route("file")]
    public class FileController : Controller
    {
        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            try
            {
                var res = System.IO.File.OpenRead("images/" + fileName);
                return File(res, "image/jpeg");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return BadRequest("image doesn`t exist");
        }
    }
}