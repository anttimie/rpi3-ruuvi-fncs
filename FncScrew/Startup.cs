using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using FncScrew;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]

namespace FncScrew
{
    public class Startup : IWebJobsStartup
    {
        public Startup()
        {

        }
        public void Configure(IWebJobsBuilder builder)
        {
            Console.WriteLine("lmfao");
        }

        public void ConfigureServices(IServiceCollection services)
        {

        }
    }
}
