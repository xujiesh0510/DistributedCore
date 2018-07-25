using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ConsoleAop.Attributes;

namespace ConsoleAop
{
    public class Person
    {
        [HystrixCommand(nameof(HelloFallBackAsync))]
        public virtual async Task<string> HelloAsync(string name)
        {
            throw new ArgumentException("HelloAsync Dead");
            Console.WriteLine("hello " + name);
            return "ok";
        }

        [HystrixCommand(nameof(HelloFallBackAgainAsync))]
        public virtual async Task<string> HelloFallBackAsync(string name)
        {
            throw new ArgumentException("HelloFallBackAsync Dead");
            Console.WriteLine("执行失败 " + name);
            return "fail";
        }

        public virtual async Task<string> HelloFallBackAgainAsync(string name)
        {
            Console.WriteLine("再次执行失败 " + name);
            return "fail still";
        }
    }
}