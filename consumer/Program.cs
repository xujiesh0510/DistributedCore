using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Consul;
using RestTemplateCore;

namespace consumer
{
   
    class Program
    {
        static void Main(string[] args)
        {
            // UseTheNormalWay();
            UseRestTemplateWay();
           
        }

        private static void UseRestTemplateWay()
        {
            var restTemplate = new RestTemplate(new HttpClient()) {ConsulServerUrl = "http://127.0.0.1:8500"};
            var result = restTemplate.PostAsync("http://MessageServce/api/Message/Send",
                new SmsMessage { To = "13312345678", Content = "hi"}).Result;
            Console.WriteLine(result.StatusCode);
        }

        private static void ChooseOneRandom(AgentService[] services)
        {
            var serviceIndex = new Random().Next(services.Count());
            var service = services[serviceIndex];
            Console.WriteLine($"url:{service.Service} port: {service.Port}");
        }



        private static void UseTheNormalWay()
        {
            using (var consulClient = new ConsulClient(c =>
            {
                c.Address = new Uri("http://127.0.0.1:8500");
                c.Datacenter = "dc1";
            }))
            {
                
                var sevices = consulClient.Agent.Services().Result.Response
                    .Where(r => r.Value.Service.Equals("MessageServce", StringComparison.CurrentCultureIgnoreCase))
                    .Select(r => r.Value);
                ChooseOneRandom(sevices.ToArray());
                Console.ReadLine();
            }
        }
    }
}