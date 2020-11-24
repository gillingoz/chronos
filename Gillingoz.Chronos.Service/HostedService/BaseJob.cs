using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Gillingoz.Chronos.Service
{
    public class BaseJob
    {
        IConfiguration Configuration;
        IRecurringJobManager RecurringJobManager;

        public BaseJob(IConfiguration configuration, IRecurringJobManager recurringJobs)
        {
            Configuration = configuration;
            RecurringJobManager = recurringJobs;
        }
        protected void LogInfo(
            PerformContext context,
            string message)
        {
            context.SetTextColor(ConsoleTextColor.White);
            Log(context, message);
        }
        protected void LogDebug(
            PerformContext context,
            string message)
        {
            context.SetTextColor(ConsoleTextColor.Magenta);
            Log(context, message);
        }
        protected void LogError(
            PerformContext context,
            string message)
        {
            context.SetTextColor(ConsoleTextColor.Red);
            Log(context, message);
        }
        protected void Log(
            PerformContext context,
            string message)
        {
            context.WriteLine(message);
            context.ResetTextColor();

        }
        protected bool IsNullOrEmpty(
            PerformContext context,
            string fieldName,
            string fieldValue)
        {
            bool retVal = string.IsNullOrEmpty(fieldValue);
            if (retVal)
            {
                LogError(context, $"Please check {fieldName} parameter is null or empty.");
            }
            return retVal;
        }

        protected bool IsNullOrEmpty(
            PerformContext context,
            string fieldName,
            string[] fieldValue)
        {
            bool retVal = fieldValue == null;
            if (retVal)
            {
                LogError(context, $"Please check {fieldName} parameter is null or empty.");
            }
            return retVal;
        }

        protected bool DirectoryExists(
            PerformContext context,
            string folderName,
            string folderValue
            )
        {
            bool retVal = Directory.Exists(folderValue);
            if (!retVal)
            {
                LogError(context, $"{folderName} folder for given path {folderValue} doesn't exist");
            }
            return retVal;
        }
        protected void MoveFileToFolder(
            PerformContext context,
            string source,
            string destination)
        {
            LogDebug(context, $"Moving file {source} to {destination} folder.");
            File.Move(source, destination, true);
        }

    }
}
