namespace FncScrew.Helpers
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FncScrew.Entities;
    using FncScrew.Models;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    public static class GeneralHelpers
    {
        public static async Task<string> ParseEventGridTelemetryBody(JObject telemetryJObject)
        {
            return await Task.FromResult(telemetryJObject.SelectToken("data.body").ToString());
        }
        public static async Task<string> Base64Decode(dynamic base64EncodedData)
        {
            dynamic base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return await Task.FromResult(Encoding.UTF8.GetString(base64EncodedBytes));
        }
        public static async Task<PayloadEntity> ConvertToObject(string json)
        {
            JToken root = JObject.Parse(json);
            JToken rootToken = root[ConfigReader.MAC];

            return await Task.FromResult(JsonConvert.DeserializeObject<PayloadEntity>(rootToken.ToString()));
        }
        public static async Task<(CloudTableClient, CloudTable)> CreateConnectionVariables(ILogger log)
        {
            log.LogInformation($"Trying to create necessary connection variables...");

            try
            {
                CloudStorageAccount cloudStorageConnection = CloudStorageAccount.Parse(ConfigReader.StorageConnectionString);
                CloudTableClient tableClient = cloudStorageConnection.CreateCloudTableClient();
                CloudTable weatherTable = tableClient.GetTableReference(ConfigReader.StorageTable);

                (CloudTableClient, CloudTable) objTuple = new ValueTuple<CloudTableClient, CloudTable>(tableClient, weatherTable);
                    
                return await Task.FromResult(objTuple);
            }
            catch (Exception ex)
            {
                log.LogError($"Exception when trying to create connection variables, msg: {ex.Message}");
                log.LogError($"Inner Exception logged: {ex.InnerException}");
            }

            return new ValueTuple<CloudTableClient, CloudTable>(null, null);
        }
        public static async Task<List<DataTableEntity>> QueryTableDataAsync(CloudTable table)
        {
            List<DataTableEntity> data = new List<DataTableEntity>();
            TableQuery<DataTableEntity> tableQuery = new TableQuery<DataTableEntity>();
            TableContinuationToken continuationToken = null;

            do
            {
                TableQuerySegment<DataTableEntity> querySegment = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                data.AddRange(querySegment);

                continuationToken = querySegment.ContinuationToken;
            } while (continuationToken != null);

            return data;
        }
    }
}
