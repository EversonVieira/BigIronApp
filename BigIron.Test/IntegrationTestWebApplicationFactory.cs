using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace BigIron.Test;

public class IntegrationTestWebApplicationFactory<Program> : WebApplicationFactory<Program> where Program : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Load configuration values into environment variables
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var configuration = config.Build();
        });
    }
}
