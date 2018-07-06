using System;
using Microsoft.AspNetCore.Mvc;
using MsgService.Controllers;

namespace ApiService1
{
    [Route("api/[controller]")]
    public class MessageController: ControllerBase
    {
        [HttpPost("Send")]
        public ActionResult Send([FromBody]SmsMessage message)
        {
            Console.WriteLine($"-----------------------------send {message.Content} to {message.To} " + DateTime.Now);
            return Ok("ok");
        }


        [HttpGet("Send3")]
        public ActionResult Send3()
        {
            Console.WriteLine($"----------------------send xxxxxxxxxxxxxxx " + DateTime.Now);
            return Ok("ok");
        }
        [HttpPost("Send2")]
        public ActionResult Send()
        {
            Console.WriteLine($"send xxxxxxxxxxxxxxx " + DateTime.Now);
            return Ok("ok");
        }
    }
}
