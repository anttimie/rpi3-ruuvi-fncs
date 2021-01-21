namespace FncScrew
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FncScrew.Entities;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Linq;
    using FncScrew.Helpers;
    public static class DeleteTableData
    {
        [FunctionName("DeleteTableData")]
        public static async Task Run([TimerTrigger("0 0 11 * * SUN")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation("DeleteTableData function executed");

            Task<(CloudTableClient, CloudTable)> connectionVariables = GeneralHelpers.CreateConnectionVariables(log);

            if (connectionVariables.Result.Item1 != null && connectionVariables.Result.Item2 != null)
            {
                List<DataTableEntity> queriedData = await QueryWeatherDataAsync(connectionVariables.Result.Item2, log);

                if (queriedData.Count > 0)
                {
                    double days = ConfigReader.BeforeDays;

                    List<DataTableEntity> deletableRows = queriedData.Where(r => r.Timestamp
                        < DateTime.UtcNow.AddDays(days)).ToList();

                    if (deletableRows.Count > 0)
                    {
                        await DeleteEntityAsync(deletableRows, connectionVariables.Result.Item2, log);
                    }
                }
            }

            log.LogInformation("DeleteTableData function execution is ready");
        }

        [FunctionName("QueryWeatherData")]
        public static async Task<List<DataTableEntity>> QueryWeatherDataAsync(CloudTable weatherTable, ILogger log)
        {
            log.LogInformation($"Collecting table data to be deleted");

            List<DataTableEntity> data = new List<DataTableEntity>();
            TableQuery<DataTableEntity> tableQuery = new TableQuery<DataTableEntity>();
            TableContinuationToken continuationToken = null;

            do
            {
                TableQuerySegment<DataTableEntity> querySegment = await weatherTable.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                data.AddRange(querySegment);

                continuationToken = querySegment.ContinuationToken;
            } while (continuationToken != null);

            return data;
        }
        [FunctionName("DeleteEntity")]
        public static async Task DeleteEntityAsync(List<DataTableEntity> deletableRows, CloudTable weatherTable, ILogger log)
        {
            log.LogInformation($"Trying to delete data from table with a count of: {deletableRows.Count}");

            try
            {
                foreach (DataTableEntity row in deletableRows)
                {
                    TableOperation deleteOperation = TableOperation.Delete(row);
                    TableResult result = await weatherTable.ExecuteAsync(deleteOperation);
                }
            }
            catch (StorageException ex)
            {
                log.LogError($"Exception when trying to delete rows in table, msg: {ex.Message}");
                log.LogError($"InnerException: {ex.InnerException}");
            }
        }
    }
}
