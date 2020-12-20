using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace VoyadoSearch.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                // setup logging to a file, normally this would be sent to some sort of cloud sink, but we can change that later on easily.
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithExceptionDetails()
                        .WriteTo.Console(LogEventLevel.Debug)
                        .WriteTo.Debug(LogEventLevel.Debug)
                        .WriteTo.File(
                            @"logs/logs.txt", 
                            rollingInterval: RollingInterval.Day, 
                            restrictedToMinimumLevel: LogEventLevel.Information
                        );
                });
    }
}
