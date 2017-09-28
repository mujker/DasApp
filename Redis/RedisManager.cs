using DasApp.Models;
using StackExchange.Redis;

namespace DasApp.Redis
{
    public class RedisManager
    {
        public static ConnectionMultiplexer Redis;

        private RedisManager() { }
        static RedisManager()
        {
            if (Redis == null)
            {
                Redis = ConnectionMultiplexer.Connect(Settings.ConOption);
            }
        }
    }
}
