using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HystrixTest1.Entity;
using Microsoft.AspNetCore.Mvc;

namespace HystrixTest1.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private Person p;

        public ValuesController(Person p)
        {
            this.p = p;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            return await p.HelloAsync(id.ToString());
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
