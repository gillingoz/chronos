using Hangfire;
using Hangfire.Annotations;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Gillingoz.Chronos.Service
{

    public abstract class BaseBackgroundJobService : BackgroundService
    {
        protected readonly IBackgroundJobClient backgroundJobs;
        protected readonly IRecurringJobManager recurringJobs;
        protected readonly ILogger<RecurringJobScheduler> logger;
        protected readonly IConfiguration configuration;

        public BaseBackgroundJobService(
            [NotNull] IBackgroundJobClient backgroundJobs,
            [NotNull] IRecurringJobManager recurringJobs,
            [NotNull] ILogger<RecurringJobScheduler> logger,
            [NotNull] IConfiguration configuration)
        {
            this.backgroundJobs = backgroundJobs ?? throw new ArgumentNullException(nameof(backgroundJobs));
            this.recurringJobs = recurringJobs ?? throw new ArgumentNullException(nameof(recurringJobs));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        protected string GetConnectionString(string environment) => configuration.GetConnectionString($"{environment}");
    }
}
