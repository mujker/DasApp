using System;
using System.Configuration;

namespace DasApp.Models
{
    public class Settings
    {
        /// <summary>
        /// rmi ip
        /// </summary>
        public static string RmiIp = ConfigurationManager.AppSettings["rmi-ip"];

        /// <summary>
        /// rmi port
        /// </summary>
        public static int RmiPort = Convert.ToInt32(ConfigurationManager.AppSettings["rmi-port"]);

        /// <summary>
        /// redis ip
        /// </summary>
        public static string RedisIp = ConfigurationManager.AppSettings["redis_ip"];

        /// <summary>
        /// redis port
        /// </summary>
        public static int RedisPort = Convert.ToInt32(ConfigurationManager.AppSettings["redis_port"]);

        /// <summary>
        /// redis password
        /// </summary>
        public static string RedisPw = ConfigurationManager.AppSettings["redis_pw"];
    }
}