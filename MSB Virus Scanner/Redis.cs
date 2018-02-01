using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace MSB_Virus_Scanner
{
    public class Redis
    {
        private string host = "10.100.8.188";
        //private string host = "127.0.0.1";
        private string port = "6379";

        private Lazy<ConnectionMultiplexer> _lazy_connection;
        private ConnectionMultiplexer _connection;
        private IDatabase _cache;
        private ISubscriber _subscriber;

        public Redis()
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { String.Format("{0}:{1}", host, port) }
            };

            _lazy_connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));

            _connection = _lazy_connection.Value;

            _cache = _connection.GetDatabase();

            _subscriber = _cache.Multiplexer.GetSubscriber();
        }

        public ISubscriber Subscriber()
        {
            return _subscriber;
        }
    }
}
