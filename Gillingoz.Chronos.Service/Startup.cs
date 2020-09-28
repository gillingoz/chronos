using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;

using Hangfire;
using Hangfire.SqlServer;
using Hangfire.LiteDB;
using Hangfire.Console;

namespace Gillingoz.Chronos.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(configuration => configuration
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseConsole()
                    .UseLiteDbStorage());

            services.AddHangfireServer();
            
            services.AddHostedService<ChronosService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHangfireDashboard("", new DashboardOptions
            {
                DashboardTitle = "Chronos Integration Service"
            });
        }
    }
}
