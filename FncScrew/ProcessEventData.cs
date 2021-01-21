namespace FncScrew
{
    using System;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.EventGrid;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using FncScrew.Models;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using FncScrew.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FncScrew.Helpers;
    public static class ProcessEventData
    {
        [FunctionName("ProcessEventData")]
        public static async Task Run([EventGridTrigger] JObject eventGridEventJobject, ILogger log)
        {
            log.LogInformation($"EventGridTrigger triggered at: {DateTime.UtcNow}");

            string encodedJSonString = await GeneralHelpers.ParseEventGridTelemetryBody(eventGridEventJobject);

            if (!string.IsNullOrEmpty(encodedJSonString))
            {
                string jsonString = await GeneralHelpers.Base64Decode(encodedJSonString);

                DataTableEntity mappedWeatherEntity = await MapPayloadToTableEntityAsync(jsonString);
                await SaveDataToStorageTableAsync(mappedWeatherEntity, log);
            }
        }
        
        [FunctionName("MapPayloadToTableEntity")]
        public static async Task<DataTableEntity> MapPayloadToTableEntityAsync(string payloadJsonString)
        {
            DataTableEntity dataTableEntity = new DataTableEntity(ConfigReader.PartitionKey, Guid.NewGuid());

            List<PayloadEntity> payloadList = new List<PayloadEntity>();
            PayloadEntity data = await GeneralHelpers.ConvertToObject(payloadJsonString);
            payloadList.Add(data);

            foreach (PayloadEntity payload in payloadList)
            {
                dataTableEntity.DataFormat = payload.DataFormat;
                dataTableEntity.Humidity = payload.Humidity;
                dataTableEntity.Temperature = payload.Temperature;
                dataTableEntity.Pressure = payload.Pressure;
                dataTableEntity.Battery = payload.Battery;
            }

            return await Task.FromResult(dataTableEntity);
        }

        [FunctionName("SaveDataToStorageTable")]
        public static async Task SaveDataToStorageTableAsync(DataTableEntity data, ILogger log)
        {
            log.LogInformation($"Trying to save data to Storage Table with data set: {data}");

            if (data != null)
            {
                CloudStorageAccount cloudStorageConnection = CloudStorageAccount.Parse(ConfigReader.StorageConnectionString);
                CloudTableClient client = cloudStorageConnection.CreateCloudTableClient();
                CloudTable table = client.GetTableReference(ConfigReader.StorageTable);
                
                TableOperation tableOperation = TableOperation.Insert(data);

                try
                {
                    await table.ExecuteAsync(tableOperation).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.LogError($"Table ExecuteAsync failed: {ex.Message}");
                    log.LogError($"Inner Exception: {ex.InnerException}");
                }
            }
        }
    }
}
