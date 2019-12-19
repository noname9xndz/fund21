using smartFunds.Common.Options;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Caching.AutoComplete
{
    public interface IRedisAutoCompleteProvider
    {
        Task AddAsync(IAutoCompleteModel item);
        Task AddAsync(IList<IAutoCompleteModel> items);
        Task ClearAsync(string setName);
        Task<IList<AutoCompleteItem>> GetAsync(string setName, string key, int limit = 0);
        Task RemoveAsync(IAutoCompleteModel item);   
    }

    public class RedisAutoCompleteProvider : IRedisAutoCompleteProvider
    {
        const int MIN_KEYSTROKE = 1;
        private readonly IOptions<smartFundsRedisOptions> _redisConfigurationOptions;
        private readonly IRedisAutoComplete _redisAutoComplete;
        public RedisAutoCompleteProvider(IOptions<smartFundsRedisOptions> redisConfigurationOptions, IRedisAutoComplete redisAutoComplete)
        {
            _redisConfigurationOptions = redisConfigurationOptions;
            _redisAutoComplete = redisAutoComplete;
        }

        public async Task AddAsync(IAutoCompleteModel item)
        {
            if (!_redisConfigurationOptions.Value.EnableAutoComplete)
            {
                return;
            }

            await _redisAutoComplete.AddAsync(item);
        }

        public async Task AddAsync(IList<IAutoCompleteModel> items)
        {
            if (!_redisConfigurationOptions.Value.EnableAutoComplete)
            {
                return;
            }

            if (items == null)
            {
                return;
            }

            await _redisAutoComplete.AddAsync(items.ToList());
        }

        public async Task<IList<AutoCompleteItem>> GetAsync(string setName, string key, int limit = 0)
        {
            var result = new List<AutoCompleteItem>();
            if (!_redisConfigurationOptions.Value.EnableAutoComplete)
            {
                return result;
            }

            if (string.IsNullOrEmpty(key) || key.Length < MIN_KEYSTROKE)
            {
                return result;
            }

            if (limit > 0)
            {
                result = await _redisAutoComplete.GetAsync(setName, key, limit);
            }
            else
            {
                result = await _redisAutoComplete.GetAsync(setName, key);
            }

            return result;

        }

        public async Task RemoveAsync(IAutoCompleteModel item)
        {
            if (!_redisConfigurationOptions.Value.EnableAutoComplete)
            {
                return;
            }

            await _redisAutoComplete.RemoveAsync(item);
        }

        public async Task ClearAsync(string setName)
        {
            if (!_redisConfigurationOptions.Value.EnableAutoComplete)
            {
                return;
            }

            await _redisAutoComplete.ClearAsync(setName);
        }
    }
}
