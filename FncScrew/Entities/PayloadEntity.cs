namespace FncScrew.Models
{
    using Newtonsoft.Json;

    public class PayloadEntity
    {
        [JsonProperty("data_format")]
        public string DataFormat { get; set; }
        
        [JsonProperty("humidity")]
        public string Humidity { get; set; }
        
        [JsonProperty("temperature")]
        public string Temperature { get; set; }
            
        [JsonProperty("Pressure")]
        public string Pressure { get; set; }
        [JsonProperty("acceleration")]
        public string Acceleration { get; set; }

        [JsonProperty("acceleration_x")]
        public string AccelerationX { get; set; }

        [JsonProperty("acceleration_y")]
        public string AccelerationY { get; set; }

        [JsonProperty("acceleration_z")]
        public string AccelerationZ { get; set; }

        [JsonProperty("tx_power")]
        public string TxPower { get; set; }

        [JsonProperty("battery")]
        public string Battery { get; set; }

        [JsonProperty("movement_counter")]
        public string MovementCounter { get; set; }

        [JsonProperty("measurement_sequence_number")]
        public string MeasurementSequenceNumber { get; set; }

        [JsonProperty("mac")]
        public string MacAddress { get; set; }

    }
}
