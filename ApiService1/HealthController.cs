using System;
using Microsoft.AspNetCore.Mvc;

namespace ApiService1
{
    [Route("api/[controller]")]
    public class HealthController:Controller
    {
        [HttpGet]
        public  IActionResult Get()
        {
            Console.WriteLine("健康检查"+ DateTime.Now);
            return Ok("ok");
        }
    }
}
