using smartFunds.Common;
using smartFunds.Common.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Caching
{
    public interface IRedisCacheProvider
    {
        Task<bool> AddAsync<T>(string key, T value, int expiryMinutes = 0);
        Task<bool> DeleteAsync(string key);
        void FlushCache();
        T Get<T>(string key);
        List<string> GetAllKeys();
        Task<T> GetAsync<T>(string key);
        string GetCurrentServer();
        Task<BuildCacheInfo> GetBuildCacheInfoAsync();
        Task SetBuildCacheInfoAsync(BuildCacheInfo buildCacheInfo);
    }

    public class RedisCacheProvider : IRedisCacheProvider
    {
        private readonly IOptions<smartFundsRedisOptions> _redisConfigurationOptions;
        private IRedisConnectionFactory _redisConnectionFactory;
        public RedisCacheProvider(IRedisConnectionFactory redisConnectionFactory, IOptions<smartFundsRedisOptions> redisConfigurationOptions)
        {
            _redisConnectionFactory = redisConnectionFactory;
            _redisConfigurationOptions = redisConfigurationOptions;
        }

        public async Task<bool> AddAsync<T>(string key, T value, int expiryMinutes = 0)
        {
            return await _redisConnectionFactory.Client.AddAsync<T>(key, value, GetCacheExpiry(expiryMinutes));
        }
        public async Task<bool> DeleteAsync(string key)
        {
            return await _redisConnectionFactory.Client.RemoveAsync(key);
        }
        public async Task<T> GetAsync<T>(string key)
        {
            return await _redisConnectionFactory.Client.GetAsync<T>(key);
        }

        public T Get<T>(string key)
        {
            return _redisConnectionFactory.Client.Get<T>(key);
        }


        public void FlushCache()
        {
            _redisConnectionFactory.Client.FlushDbAsync();
            _redisConnectionFactory.FlushDatabaseNode();
        }

        public string GetCurrentServer()
        {
            return "Cache server is: " + _redisConnectionFactory.Client.Database.Multiplexer.Configuration;
        }
        public List<string> GetAllKeys()
        {
            var keys = _redisConnectionFactory.Client.Database.Multiplexer.GetServer(_redisConfigurationOptions.Value.Endpoint, _redisConfigurationOptions.Value.Port).Keys();
            if (keys != null)
                return keys.Select(x => x.ToString()).ToList();
            return new List<string>();
        }

        public async Task<BuildCacheInfo> GetBuildCacheInfoAsync()
        {
            var status = await GetAsync<string>(Constants.Cache.BuildKeys.Status);
            BuildCacheStatusType? statusValue = null;
            if (status != null)
            {
                statusValue = EnumHelpers.ToEnum<BuildCacheStatusType>(status);
            }
            return new BuildCacheInfo
            {
                Status = statusValue,
                StartTime = await GetAsync<string>(Constants.Cache.BuildKeys.StartTime),
                EndTime = await GetAsync<string>(Constants.Cache.BuildKeys.EndTime),
                Heartbeat = await GetAsync<string>(Constants.Cache.BuildKeys.Heartbeat),
                Error = await GetAsync<string>(Constants.Cache.BuildKeys.Error)
            };
        }

        public async Task SetBuildCacheInfoAsync(BuildCacheInfo buildCacheInfo)
        {
            await AddAsync(Constants.Cache.BuildKeys.Status, buildCacheInfo.Status?.ToString()?? "");
            await AddAsync(Constants.Cache.BuildKeys.StartTime, buildCacheInfo.StartTime?.ToString());
            await AddAsync(Constants.Cache.BuildKeys.EndTime, buildCacheInfo.EndTime?.ToString()?? "");
            await AddAsync(Constants.Cache.BuildKeys.Heartbeat, buildCacheInfo.Heartbeat);
            await AddAsync(Constants.Cache.BuildKeys.Error, buildCacheInfo.Error?? "");
        }

        private DateTimeOffset GetCacheExpiry(int expiry)
        {
            if (expiry > 0)
            {
                return new DateTimeOffset(DateTime.Now.AddMinutes(expiry));
            }
            else
            {
                return new DateTimeOffset(DateTime.Now.AddMinutes(_redisConfigurationOptions.Value.Expiry));
            }
        }
    }
}
