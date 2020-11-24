using Hangfire;
using Hangfire.Annotations;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace Gillingoz.Chronos.Service
{
    public class CreateDataPackage : BaseJob
    {
        public CreateDataPackage(IConfiguration configuration, IRecurringJobManager recurringJobs) : base(configuration, recurringJobs)
        {
            var importJobs = configuration.GetSection("CreateDataPackageSettings").Get<List<CreateDataPackageSettings>>();
            foreach (var job in importJobs.Where(i => i.IsEnabled))
            {
                recurringJobs.AddOrUpdate(job.JobId, () => JobExecute(null, job), job.Schedule);
            }
        }
        public void JobExecute(
            PerformContext context,
            CreateDataPackageSettings importJob)
        {
            if (IsNullOrEmpty(context, nameof(importJob.DefinitionGroupId), importJob.DefinitionGroupId) ||
                IsNullOrEmpty(context, nameof(importJob.SourceFolder), importJob.SourceFolder) ||
                IsNullOrEmpty(context, nameof(importJob.TemplateFolder), importJob.TemplateFolder) ||
                IsNullOrEmpty(context, nameof(importJob.DestinationFolder), importJob.DestinationFolder) ||
                IsNullOrEmpty(context, nameof(importJob.DataFileName), importJob.DataFileName))
                return;

            if (DirectoryExists(context, nameof(importJob.SourceFolder), importJob.SourceFolder) &&
                DirectoryExists(context, nameof(importJob.DestinationFolder), importJob.DestinationFolder) &&
                DirectoryExists(context, nameof(importJob.TemplateFolder), importJob.TemplateFolder))
            {
                
                CheckFilesInSourceFolder(context, importJob.SourceFolder, importJob.TemplateFolder, importJob.DestinationFolder, importJob.DataFileName, importJob.XsltFileName, importJob.DefinitionGroupId);
            }
        }
        private void CheckFilesInSourceFolder(
            PerformContext context,
            string source,
            string template,
            string target,
            string data,
            string[] xsltFiles,
            string definitionGroupId)
        {
            LogInfo(context, $"Checking folders for {definitionGroupId}");
            var getFolderInFiles = Directory.GetFiles(source);
            LogInfo(context, $"There are {getFolderInFiles.Count()} files in the {nameof(source)} folder {source}");
            foreach (var item in getFolderInFiles)
            {
                LogInfo(context, $"Processing file: {item}");
                var sourceItem = $"{item}";
                // We need conversion if there is XSLT exist.
                foreach (var xsltFileName in xsltFiles)
                {
                    // Transform the data file using xslt
                    LogDebug(context, $"Converting file into XML using {xsltFileName}");
                    Transform(context, sourceItem, $"{template}\\{data}", xsltFileName);
                    LogInfo(context, $"File converted into xml");

                    // after initial XSLT runs, we need to run sequential XSLTS against to file generated as a result of first
                    File.Copy($"{template}\\{data}", sourceItem, true);
                    //sourceItem = $"{template}\\{data}";
                }
                // Move data file to template folder and overwrite if exists
                LogDebug(context, $"Moving {item} to {Path.Combine(template, data)}");
                MoveFileToFolder(context, item, Path.Combine(template, data));
                LogInfo(context, $"File converted into xml");

                FileToZip(template, target, definitionGroupId);
                File.Delete(item);
                LogInfo(context, $"Processing file: {item} completed");
            }
        }

        private void Transform(
            PerformContext context,
            string source,
            string target,
            string xsltFileName)
        {

            XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
            try
            {
                LogDebug(context, $"Loading {xsltFileName}");
                xslCompiledTransform.Load(xsltFileName);
                xslCompiledTransform.Transform(source, target);
            }
            catch (Exception ex)
            {
                LogDebug(context, ex.ToString());
            }
        }
        private void FileToZip(
            string zipSource,
            string zipTarget,
            string prefix)
        {
            ZipFile.CreateFromDirectory(zipSource, $"{zipTarget}\\{prefix}_{Guid.NewGuid()}_.zip");
        }
    }
}
