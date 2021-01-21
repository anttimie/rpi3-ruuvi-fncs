using System;
using System.Collections.Generic;
using System.Text;

namespace FncScrew
{
    internal static class ConfigReader
    {
        public static string StorageConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("STORAGEACCOUNTRGRPIB0DC", EnvironmentVariableTarget.Process);
            }
        }
        public static string PartitionKey
        {
            get
            {
                return Environment.GetEnvironmentVariable("PARTITION_KEY_NAME", EnvironmentVariableTarget.Process);
            }
        }
        public static string StorageTable
        {
            get
            {
                return Environment.GetEnvironmentVariable("STORAGE_TABLE_NAME", EnvironmentVariableTarget.Process);
            }
        }
        public static double BeforeDays
        {
            get
            {
                return double.Parse(Environment.GetEnvironmentVariable("BEFORE_DAYS", EnvironmentVariableTarget.Process));
            }
        }
        public static string MAC
        {
            get
            {
                return Environment.GetEnvironmentVariable("MAC", EnvironmentVariableTarget.Process);
            }
        }

    }
}
