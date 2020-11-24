using Hangfire;
using Hangfire.Annotations;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gillingoz.Chronos.Service
{
    public class ChronosService : BaseBackgroundJobService
    {
        public ChronosService([NotNull] IBackgroundJobClient backgroundJobs, [NotNull] IRecurringJobManager recurringJobs, [NotNull] ILogger<RecurringJobScheduler> logger, [NotNull] IConfiguration configuration) : base(backgroundJobs, recurringJobs, logger, configuration)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                CreateDataPackage createDataPackage = new CreateDataPackage(configuration, recurringJobs);
                ExtractDataPackage extractDataPackage = new ExtractDataPackage(configuration, recurringJobs);
                UploadDataPackage uploadDataPackage = new UploadDataPackage(configuration, recurringJobs);
            }
            catch (Exception e)
            {
                logger.LogError("An exception occurred while creating recurring jobs.", e);
            }
            return Task.CompletedTask;
        }
    }
}
