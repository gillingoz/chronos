using Gillingoz.Chronos.Service.Model;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Gillingoz.Chronos.Service
{
    public class ExtractDataPackage : BaseJob
    {
        public ExtractDataPackage(IConfiguration configuration, IRecurringJobManager recurringJobs) : base(configuration, recurringJobs)
        {
            var importJobs = configuration.GetSection("ExtractDataPackageSettings").Get<List<ExtractDataPackageSettings>>();
            foreach (var job in importJobs.Where(i => i.IsEnabled))
            {
                recurringJobs.AddOrUpdate(job.JobId, () => JobExecute(null, job), job.Schedule);
            }
        }

        public void JobExecute(
            PerformContext context,
            ExtractDataPackageSettings importJob)
        {
            if (IsNullOrEmpty(context, nameof(importJob.DefinitionGroupId), importJob.DefinitionGroupId) ||
                IsNullOrEmpty(context, nameof(importJob.SourceFolder), importJob.SourceFolder) ||
                IsNullOrEmpty(context, nameof(importJob.DestinationFolder), importJob.DestinationFolder) ||
                IsNullOrEmpty(context, nameof(importJob.TempFolder), importJob.TempFolder) ||
                IsNullOrEmpty(context, nameof(importJob.ArchieveFolder), importJob.ArchieveFolder) ||
                IsNullOrEmpty(context, nameof(importJob.DataFileName), importJob.DataFileName))
                return;

            if (DirectoryExists(context, nameof(importJob.SourceFolder), importJob.SourceFolder) &&
                DirectoryExists(context, nameof(importJob.DestinationFolder), importJob.DestinationFolder))
            {
                ProcesSourceFilesInFolder(context, importJob.DefinitionGroupId, importJob.SourceFolder, importJob.DestinationFolder, importJob.TempFolder, importJob.ArchieveFolder, importJob.DataFileName);
            }
        }

        private void ProcesSourceFilesInFolder(
            PerformContext context,
            string definitionGroupId,
            string source,
            string target,
            string temp,
            string archieve,
            string[] dataFiles)
        {
            LogInfo(context, $"Checking folders for {definitionGroupId}");
            var getFolderInFiles = Directory.GetFiles(source);
            LogInfo(context, $"There are {getFolderInFiles.Count()} files in the {nameof(source)} folder {source}");
            foreach (var item in getFolderInFiles)
            {
                LogInfo(context, $"Processing file: {item}");
                var sourceItem = $"{item}";
                var tempfolder = $"{temp}\\{Guid.NewGuid()}";
                LogInfo(context, $"Extracting file: {item} to temp folder in {tempfolder}");
                ZipFile.ExtractToDirectory(sourceItem, tempfolder);
                foreach (var dataFile in dataFiles)
                {
                    LogInfo(context, $"Moving file from { tempfolder}\\{ dataFile} to {target}\\{DateTime.Now:yyyyMMddHHmm}_{dataFile}");
                    File.Move($"{tempfolder}\\{dataFile}", $"{target}\\{DateTime.Now:yyyyMMddHHmm}_{dataFile}");
                    LogInfo(context, $"File moved");
                }
                
                LogInfo(context, $"Deleting temp folder in {tempfolder}");
                Directory.Delete(tempfolder, true);
                LogInfo(context, $"Folder is deleted recursively");

                MoveFileToFolder(context, sourceItem, $"{archieve}\\{System.IO.Path.GetFileName(sourceItem)}");
                 ;
                LogInfo(context, $"Source {sourceItem} is moved to {archieve}\\{System.IO.Path.GetFileName(sourceItem)}");
            }
        }
    }
}