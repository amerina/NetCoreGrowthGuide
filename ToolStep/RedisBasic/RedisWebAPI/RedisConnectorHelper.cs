using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using StackExchange.Redis;
using System;
using System.Configuration;

namespace RedisWebAPI
{
    public class RedisConnectorHelper
    {
        public static string RedisConnect;
        static RedisConnectorHelper()
        {
            RedisConnectorHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                //IConfiguration config = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true }).Build();
                //string connectionString = config.GetValue<string>("Redis:ConnectionString");

                return ConnectionMultiplexer.Connect(RedisConnect);
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
