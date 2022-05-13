using Microsoft.AspNetCore;
using Serilog;

namespace Ordering.API {
    public class Program {
        public static IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        public static int Main(string[] args) {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Console()
                .CreateLogger();

            try {
                Log.Information("Configuring web host ({ApplicationContext})...");
                CreateWebHostBuilder(args).Build().Run();

                Log.Information("Applying migrations ({ApplicationContext})...");

                Log.Information("Starting web host ({ApplicationContext})...");
                
                return 0;
            } catch (Exception ex) {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!");
                return 1;
            } finally {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .ConfigureAppConfiguration(x => x.AddConfiguration(Configuration))
                .UseStartup<Startup>()
                .UseSerilog();

    }
}

