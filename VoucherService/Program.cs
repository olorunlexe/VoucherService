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
        private static string _environmentName;
        public static void Main(string[] args)
        {

            CreateWebHostBuilder(args).Build().Run();
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) => 
                {
                    logging.ClearProviders();
                    _environmentName = hostingContext.HostingEnvironment.EnvironmentName;

                })
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
