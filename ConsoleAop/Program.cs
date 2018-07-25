using System;
using AspectCore.DynamicProxy;

namespace ConsoleAop
{
    class Program
    {
        static void Main(string[] args)
        {
            ProxyGeneratorBuilder builder = new ProxyGeneratorBuilder();
            using (var proxyGenerator = builder.Build())
            {
                var p = (Person) proxyGenerator.CreateClassProxy(typeof(Person));
                Console.WriteLine(p.HelloAsync("rupeng").Result);
            }

            Console.ReadLine();
        }
    }
}