using AwsFileStore.Infrastructure;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using System.Reflection;

namespace AwsFileStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var host = CreateGenericHost(basePath);
                Log.Debug("Starting application.");
                host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Error($"Unexpected error occurred: {ex.Message}");
                Environment.Exit(0);
            }
        }

        public static IHost CreateGenericHost(string basePath)
        {
            var logger = new LoggerConfiguration()
             .MinimumLevel.Verbose()
             .WriteTo.Console(theme: AnsiConsoleTheme.Code)
             .WriteTo.File("C:\\logs\\FileStoreApp\\.txt", rollingInterval: RollingInterval.Minute)
             .CreateLogger();

            Log.Logger = logger;

            IHost host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(basePath);
                    configHost.AddJsonFile("appsettings.json", false);
                })
                .ConfigureServices((builder, collection) =>
                {
                    collection.ConfigureServices(builder.Configuration);
                })
                .Build();

            return host;
        }
    }
}
