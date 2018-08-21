using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok("pridut:1 ,2,3");
        }
    }
}
