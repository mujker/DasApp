using System;
using System.Configuration;

namespace DasApp.Models
{
    public class Settings
    {
        public static string CryptKey = "hxsoft++";

        /// <summary>
        ///     rmi ip
        /// </summary>
        public static string RmiIp = ConfigurationManager.AppSettings["rmi-ip"];

        /// <summary>
        ///     rmi port
        /// </summary>
        public static int RmiPort = Convert.ToInt32(ConfigurationManager.AppSettings["rmi-port"]);
        public static int RetryCount = Convert.ToInt32(ConfigurationManager.AppSettings["retry_count"]);

        public static string RedisIp = ConfigurationManager.AppSettings["redis_ip"];
        public static int RedisPort = Convert.ToInt32(ConfigurationManager.AppSettings["redis_port"]);

        public static string RedisPw = string.IsNullOrEmpty(ConfigurationManager.AppSettings["redis_pw"])
            ? null
            : ConfigurationManager.AppSettings["redis_pw"];
    }
}