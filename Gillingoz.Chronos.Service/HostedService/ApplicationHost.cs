using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Gillingoz.Chronos.Service
{
    public class ApplicationHost<TStartup>
    where TStartup : class
    {
        private readonly string _launchDirectory;
        private IWebHost _webHost;
        private bool _stopRequested;

        public ApplicationHost()
        {
            _launchDirectory = Directory.GetCurrentDirectory();
        }

        public void Start(bool launchedFromConsole)
        {
            var contentRootPath = launchedFromConsole ? _launchDirectory : Directory.GetCurrentDirectory();

            // set up configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(contentRootPath)
                .AddJsonFile("jobs.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // set up web host
            IWebHostBuilder webHostBuilder = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://localhost:8443", "http://localhost:5555", "http://localhost:5500")
                .UseConfiguration(config)
                .UseContentRoot(contentRootPath)
                .UseStartup<TStartup>();

            // create and run host
            _webHost = webHostBuilder.Build();
            _webHost.Services.GetRequiredService<IHostApplicationLifetime>()
                .ApplicationStopped.Register(() =>
                {
                    if (!_stopRequested)
                        Stop();
                });

            _webHost.Start();

            // print information to console
            if (launchedFromConsole)
            {
                var hostingEnvironment = _webHost.Services.GetService<IWebHostEnvironment>();
                Console.WriteLine($"Hosting environment: {hostingEnvironment.EnvironmentName}");
                Console.WriteLine($"Content root path: {hostingEnvironment.ContentRootPath}");

                var serverAddresses = _webHost.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;
                foreach (var address in serverAddresses ?? Array.Empty<string>())
                {
                    Console.WriteLine($"Listening on: {address}");
                }
            }
        }

        public void Stop()
        {
            _stopRequested = true;
            _webHost?.Dispose();
        }
    }
}
