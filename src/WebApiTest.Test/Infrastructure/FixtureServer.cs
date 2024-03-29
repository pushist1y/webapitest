﻿using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using WebapiTest.Service;

namespace WebApiTest.Test.Infrastructure
{
    public class FixtureServer
    {
        public TestServer Server { get; }

        public FixtureServer()
        {
            var path = FindAppSettings(PlatformServices.Default.Application.ApplicationBasePath);
            var builder = new WebHostBuilder()
                    .UseEnvironment("Tests")
                    .UseContentRoot(path)
                .ConfigureAppConfiguration(cfg =>
                    cfg.SetBasePath(path)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile("appsettings.unversioned.json", optional: true)
                        .AddEnvironmentVariables()
                )
                .ConfigureLogging(b => b.AddConsole().AddDebug())


                .UseStartup<TestStartup>()
                .UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).GetTypeInfo().Assembly.FullName)

                ;


            //.UseStartup<Startup>(); //the test passes with this line

            Server = new TestServer(builder);
        }

        string FindAppSettings(string appPath)
        {
            var dir = new DirectoryInfo(appPath);
            while (true)
            {
                if ((dir?.EnumerateFiles("*appsettings*").Any()) ?? false)
                {
                    return dir?.FullName;
                }

                dir = dir?.Parent;
                if (dir == null)
                {
                    return null;
                }
            }
        }


    }
}
