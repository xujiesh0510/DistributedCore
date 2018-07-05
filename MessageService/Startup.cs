using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiService1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            var ip = Configuration["ip"];
            var port = Configuration["port"];
            var serviceName = "MessageServce";
            var serviceId = serviceName + Guid.NewGuid();
            using (var consulClient = new ConsulClient(ConsulClientConfig))
            {
                consulClient.Agent.ServiceRegister(new AgentServiceRegistration
                {
                    Address = ip,
                    Port = int.Parse(port),
                    ID = serviceId,
                    Name = serviceName,
                    Check = new AgentServiceCheck
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10),
                        HTTP = $"http://{ip}:{port}/api/Health",
                        Interval = TimeSpan.FromSeconds(5),
                        Timeout = TimeSpan.FromSeconds(5)

                    }
                }).Wait();
            }

            applicationLifetime.ApplicationStopped.Register(() =>
            {
                using (var consulClient = new ConsulClient(ConsulClientConfig))
                {
                    Console.WriteLine("应用退出，从consul 退出");
                    consulClient.Agent.ServiceDeregister(serviceId).Wait();
                }
            });
        }

        private void ConsulClientConfig(ConsulClientConfiguration c)
        {
            c.Address = new Uri("http://127.0.0.1:8500");
            c.Datacenter = "dc1";
        }
    }
}
