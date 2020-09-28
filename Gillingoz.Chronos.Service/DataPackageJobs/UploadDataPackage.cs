using Hangfire;
using Hangfire.Annotations;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gillingoz.Chronos.Service
{
    public class UploadDataPackage : BaseJob
    {
        public UploadDataPackage(IConfiguration configuration, IRecurringJobManager recurringJobs) : base(configuration, recurringJobs)
        {
            var uploadJobs = configuration.GetSection("UploadDataPackageSettings").Get<List<UploadDataPackageSettings>>();
            var environments = configuration.GetSection("EnvironmentSettings").Get<List<EnvironmentSettings>>();
            foreach (var job in uploadJobs.Where(i => i.IsEnabled))
            {
                var environment = environments.FirstOrDefault(i => i.Id == job.EnvironmentId && i.IsEnabled);
                if (environment != null)
                {
                    recurringJobs.AddOrUpdate(job.JobId, () => ExecuteJob(null, environment, job), job.Schedule);
                }
            }
        }

        public void ExecuteJob(PerformContext context, EnvironmentSettings environment, UploadDataPackageSettings settings)
        {
            var filesInFolder = Directory.GetFiles(settings.SourceFolder);
            foreach (var file in filesInFolder)
            {
                var fileInfo = new FileInfo(file);
                // if file is locked means it is not ready to process
                if (!fileInfo.IsFileLocked())
                {
                    var bytes = File.ReadAllBytes(file);
                    var fileContent = Convert.ToBase64String(bytes);
                    var executionId = Path.GetFileNameWithoutExtension(file);
                    var uniqueFileName = Path.GetFileName(file);

                    if (SendPackageToApi(context, environment, settings, fileContent, executionId, uniqueFileName))
                    { 
                        if (!fileInfo.IsFileLocked())
                        {
                            MoveFileToFolder(context, file, $"{settings.SentFolder}\\{uniqueFileName}" );
                        }
                    }
                }
            }
        }
        public bool SendPackageToApi(PerformContext context, EnvironmentSettings environment, UploadDataPackageSettings importJob, string fileContent, string executionId, string uniqueFileName)
        {
            LogDebug(context, "Package is beign uploaded to API");
            var client = new RestClient(environment.ApiUrl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader(environment.ApiSubscriptionKeyName, environment.ApiSubscriptionKeyValue);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(new
            {
                baseUrl = environment.BaseUrl,
                clientId = environment.ClientId,
                clientSecret = environment.ClientSecret,
                definitionGroupId = importJob.DefinitionGroupId,
                executionId,
                fileContent,
                legalEntityId = environment.LegalEntityId,
                tenant = environment.Tenant,
                uniqueFileName
            }), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            LogDebug(context, $"Package uploaded to API - response received ({response.StatusCode}): {response.Content}");
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

    }
}
