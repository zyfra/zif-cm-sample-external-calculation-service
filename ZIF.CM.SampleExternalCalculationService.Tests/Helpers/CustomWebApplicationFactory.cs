using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZIF.CM.SampleExternalCalculationService.Tests.Helpers
{
    public class CustomWebApplicationFactory: WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(OverrideConfiguration);
            builder.ConfigureTestServices(OverrideRegisteredServices);
        }
        
        private void OverrideConfiguration(IConfigurationBuilder builder)
        {
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.tests.json"));
        }

        private void OverrideRegisteredServices(IServiceCollection services)
        {
            // override registered services here
        }
        
    }
}