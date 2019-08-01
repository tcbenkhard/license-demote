using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SecurePipelineScan.VstsService;
using System;
using System.Net.Http;

[assembly: WebJobsStartup(typeof(Functions.Startup))]

namespace Functions
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            RegisterServices(builder.Services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            var vstsPat = Environment.GetEnvironmentVariable("vstsPat", EnvironmentVariableTarget.Process);
            var organization = Environment.GetEnvironmentVariable("organization", EnvironmentVariableTarget.Process) ?? "raboweb-test";

            services.AddSingleton<IVstsRestClient>(new VstsRestClient(organization, vstsPat));
        }
    }
}