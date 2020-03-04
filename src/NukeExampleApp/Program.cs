using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace NukeExampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:6000")
                .UseKestrel()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration(
                    (hostingContext, config) => config
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true))
                .Build()
                .Run();
        }
    }
}