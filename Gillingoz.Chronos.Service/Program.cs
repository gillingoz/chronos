using Topshelf;
using Topshelf.Hosts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.Extensions.Hosting;

namespace Gillingoz.Chronos.Service
{
    class Program
    {
        public static void Main(string[] args)
        {
            Program.Run<Startup>(args);
        }

        public static void Run<TStartup>(string[] args)
            where TStartup : class
        {
            HostFactory.Run(x =>
            {
                x.Service<ApplicationHost<TStartup>>(s =>
                {
                    s.ConstructUsing(name => new ApplicationHost<TStartup>());
                    s.WhenStarted((svc, control) =>
                    {
                        svc.Start(control is ConsoleRunHost);
                        return true;
                    });
                    s.WhenStopped(svc => svc.Stop());
                    s.WhenShutdown(svc => svc.Stop());
                });

                x.StartAutomaticallyDelayed();

                // set default service name
                string defaultServiceName = typeof(TStartup).Namespace;
                if (!string.IsNullOrEmpty(defaultServiceName))
                    x.SetServiceName(defaultServiceName);
            });
        }
    }
}
