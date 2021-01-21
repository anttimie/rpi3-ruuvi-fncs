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
                return Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.Process);
            }
        }
        public static string PartitionKey
        {
            get
            {
                return Environment.GetEnvironmentVariable("PartitionKey", EnvironmentVariableTarget.Process);
            }
        }
        public static string StorageTable
        {
            get
            {
                return Environment.GetEnvironmentVariable("StorageTable", EnvironmentVariableTarget.Process);
            }
        }
        public static double BeforeDays
        {
            get
            {
                return double.Parse(Environment.GetEnvironmentVariable("DaysBefore", EnvironmentVariableTarget.Process));
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
