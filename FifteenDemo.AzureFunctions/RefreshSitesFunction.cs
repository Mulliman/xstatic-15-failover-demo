using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using XStatic.RemoteOperations;

namespace FifteenDemo.AzureFunctions
{
    public class RefreshSitesFunction(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<RefreshSitesFunction>();
        private readonly IConfiguration _configuration = configuration;

        [Function("RefreshSitesFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

            string? baseUri = _configuration["Cms.BaseUri"];
            string? clientId = _configuration["Cms.ClientId"];
            string? clientSecret = _configuration["Cms.ClientSecret"];

            if (string.IsNullOrEmpty(baseUri) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                _logger.LogError("ClientId or ClientSecret not found in configuration");
                return;
            }

            var remoteOperationsService = new XStaticRemoteOperationsClient(baseUri, clientId, clientSecret, _logger);
            var results = remoteOperationsService.GenerateAndDeployAllSites();

            var errors = new List<System.Exception>();

            await foreach (var result in results)
            {
                if (result.Exception != null)
                {
                    errors.Add(result.Exception);
                }
            }

            if (errors.Count > 0)
            {
                _logger.LogError("One or more sites failed to generate and deploy.");
                throw new AggregateException(errors);
            }

            _logger.LogInformation("All sites have been generated and deployed");
        }
    }
}
