using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VoucherService.MQ;
using Serilog;

namespace VoucherService
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = 
                new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        private static string _environmentName;
        public static void Main(string[] args)
        {
<<<<<<< HEAD
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration).CreateLogger();
            
            try {
                CreateWebHostBuilder(args).Build().Run();
                return;
            } catch (Exception ex) {
                Log.Fatal(ex, "Host terminated unexpectedly");
            } finally {
                Log.CloseAndFlush();
            }
=======

            CreateWebHostBuilder(args).Build().Run();
            
>>>>>>> 329272def250e790152112a1a1eb90a563960eb2
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
