namespace FncScrew
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using FncScrew.Entities;
    using System.Net.Http;
    using Microsoft.WindowsAzure.Storage.Table;
    using FncScrew.Helpers;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    public static class GetDataToDisplay
    {
        [FunctionName("GetDataToDisplay")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Function GetDataToDisplay processed a request");

            List<DataTableEntity> entity = await GetLatestEntityAsync(log);

            string json = JsonConvert.SerializeObject(entity);

            return !String.IsNullOrEmpty(json) ? new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            } : new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        [FunctionName("GetLatestEntity")]
        public static async Task<List<DataTableEntity>> GetLatestEntityAsync(ILogger log)
        {
            log.LogInformation($"GetLatestEntity");

            List<DataTableEntity> entity = new List<DataTableEntity>();

            Task<(CloudTableClient, CloudTable)> connectionVariables = GeneralHelpers.CreateConnectionVariables(log);

            if (connectionVariables.Result.Item1 != null && connectionVariables.Result.Item2 != null)
            {
                CloudTable t = connectionVariables.Result.Item2;
                List<DataTableEntity> data = await GeneralHelpers.QueryTableDataAsync(t);
                entity = data.OrderByDescending(e => e.Timestamp).Take(5).ToList();
            }

            return entity;
        }
    }
}
