using smartFunds.Common.Options;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;

namespace smartFunds.Caching
{
    public interface IRedisConnectionFactory
    {
        StackExchangeRedisCacheClient Client { get; }
        void FlushDatabaseNode();
    }

    // Must be injected as singleton
    public class RedisConnectionFactory : IRedisConnectionFactory
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;
        private readonly IOptions<smartFundsRedisOptions> _redisConfigurationOptions;

        public RedisConnectionFactory(IOptions<smartFundsRedisOptions> redisConfigurationOptions)
        {
            _redisConfigurationOptions = redisConfigurationOptions;
            _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_redisConfigurationOptions.Value.Host));
        }

        public ConnectionMultiplexer Connection()
        {
            return _connection.Value;
        }

        private IDatabase Database
        {
            get
            {
                if (_connection.IsValueCreated)
                {
                    return _connection.Value.GetDatabase();
                }
                else
                {
                    var connection = _connection.Value;
                    if (connection != null)
                        return connection.GetDatabase();
                    else
                        return null;
                }
            }
        }

        private readonly object _clientLock = new object();
        private StackExchangeRedisCacheClient _client = null;
        public StackExchangeRedisCacheClient Client
        {
            get
            {
                if (_client == null)
                {
                    lock (_clientLock)
                    {
                        if (_client == null)
                        {
                            _client = new StackExchangeRedisCacheClient(Database.Multiplexer, new NewtonsoftSerializer(), _redisConfigurationOptions.Value.Database);
                        }
                    }
                }
                return _client;
            }
        }

        public void FlushDatabaseNode()
        {
            if (_connection.IsValueCreated)
            {
                var server = _connection.Value.GetServer(_redisConfigurationOptions.Value.Endpoint, _redisConfigurationOptions.Value.Port);
                server.FlushDatabase(_redisConfigurationOptions.Value.Database);
            }
        }
    }
}
