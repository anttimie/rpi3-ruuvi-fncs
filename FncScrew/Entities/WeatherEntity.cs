using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace FncScrew.Entities
{
    public class WeatherEntity : TableEntity
    {
        public WeatherEntity(string indoorClimate, Guid guid)
        {
            this.PartitionKey = indoorClimate;
            this.RowKey = guid.ToString();
        }

        public WeatherEntity() { }

        public string DataFormat { get; set; }
        public string Humidity { get; set; }
        public string Temperature { get; set; }
        public string Pressure { get; set; }
        //public string Acceleration { get; set; }
        //public string AccelerationX { get; set; }
        //public string AccelerationY { get; set; }
        //public string AccelerationZ { get; set; }
        //public string TxPower { get; set; }
        public string Battery { get; set; }
        //public string MovementCounter { get; set; }
        //public string MeasurementSequenceNumber { get; set; }
        //public string MacAddress { get; set; }
    }
}
