namespace FncScrew.Entities
{
    using System;
    using Microsoft.WindowsAzure.Storage.Table;
    public class DataTableEntity : TableEntity
    {
        public DataTableEntity(string indoorClimate, Guid guid)
        {
            this.PartitionKey = indoorClimate;
            this.RowKey = guid.ToString();
        }

        public DataTableEntity() { }

        public string DataFormat { get; set; }
        public string Humidity { get; set; }
        public string Temperature { get; set; }
        public string Pressure { get; set; }
        public string Battery { get; set; }
    }
}
