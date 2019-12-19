using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Caching.AutoComplete
{
    public interface IRedisAutoComplete
    {
        Task AddAsync(IAutoCompleteModel item);
        Task AddAsync(List<IAutoCompleteModel> inputs);
        Task<List<AutoCompleteItem>> GetAsync(string setName, string key);
        Task<List<AutoCompleteItem>> GetAsync(string setName, string key, int limit);
        Task RemoveAsync(IAutoCompleteModel item);
        Task ClearAsync(string setName);
    }

    public class RedisAutoComplete : IRedisAutoComplete
    {
        private IRedisConnectionFactory _redisConnectionFactory;
        public RedisAutoComplete(IRedisConnectionFactory redisConnectionFactory)
        {
            _redisConnectionFactory = redisConnectionFactory;
        }

        public async Task AddAsync(List<IAutoCompleteModel> inputs)
        {
            foreach (var item in inputs)
            {
                if (item.AutoCompleteItem != null && !string.IsNullOrEmpty(item.AutoCompleteItem.Key))
                {
                    await InsertValue(item.AutoCompleteItem.SetName, item.AutoCompleteItem.Key, item.AutoCompleteItem);
                }
            }
        }

        public async Task AddAsync(IAutoCompleteModel item)
        {
            if (item.AutoCompleteItem != null && !string.IsNullOrEmpty(item.AutoCompleteItem.Key))
            {
                await InsertValue(item.AutoCompleteItem.SetName,item.AutoCompleteItem.Key, item.AutoCompleteItem);
            }
        }

        public async Task RemoveAsync(IAutoCompleteModel item)
        {
            if (item == null || string.IsNullOrEmpty(item.AutoCompleteItem.Key))
                return;
            var key = item.AutoCompleteItem.Key.ToLower();

            var existingItems = await this.GetAsync(item.AutoCompleteItem.SetName,key.Substring(0, item.AutoCompleteItem.Key.Length - 1));
            if (existingItems != null && existingItems.Count > 0)
            {

                var deleteItemKey = string.Format("{0}*#*{1}*#*", item.AutoCompleteItem.Key.ToLower(), JsonConvert.SerializeObject(item.AutoCompleteItem));

                await _redisConnectionFactory.Client.Database.SortedSetRemoveAsync(item.AutoCompleteItem.SetName, deleteItemKey);
            }
        }

        public async Task ClearAsync(string setName)
        {
            await _redisConnectionFactory.Client.Database.KeyDeleteAsync(setName);
        }

        public async Task<List<AutoCompleteItem>> GetAsync(string setName, string key, int limit)
        {
            var result = await GetAsync(setName,key);

            return result.Take(limit).ToList();
        }

        public async Task<List<AutoCompleteItem>> GetAsync(string setName,string key)
        {
            key = key.ToLower();

            var AutoCompleteResult = new List<AutoCompleteItem>();

            var start = await GetRank(setName,key);
            var end = await GetRank(setName,GetNextString(key));
            if (start < 0)
                return new List<AutoCompleteItem>();
            end = end > 0 ? end : -1;
            var result = await GetRange(setName,start, end);
            var filteredResult = result.Where(x => x.EndsWith("*#*"));
            foreach (var item in filteredResult)
            {
                var autores = item.Split(new string[] { "*#*" }, StringSplitOptions.RemoveEmptyEntries);
                if (autores != null && autores.Count() == 2)
                {
                    var criteria = JsonConvert.DeserializeObject<AutoCompleteItem>(autores[1]);
                    AutoCompleteResult.Add(criteria);
                }
            }

            return AutoCompleteResult;
        }

        #region private Methods

        private async Task InsertValue(string setName,string key, object value)
        {
            key = key.ToLower();
            var stringvalue = value == null ? key : JsonConvert.SerializeObject(value);
            var keyvalue = "";
            for (int i = 0; i < key.Length; i++)
            {
                keyvalue += key[i];
                var nextkey = GetNextString(keyvalue);
                if (i == key.Length - 1)
                {
                    keyvalue += "*#*" + stringvalue + "*#*";
                }
                await InsertToSortedSet(setName,keyvalue);
                await InsertToSortedSet(setName,nextkey);
            }
        }

        private async Task InsertToSortedSet(string setName, string keyvalue)
        {
            var entry = new SortedSetEntry(keyvalue, 0.0);
            var entryArray = new SortedSetEntry[] { entry };
            await _redisConnectionFactory.Client.Database.SortedSetAddAsync(setName, entryArray);
        }

        private string GetNextString(string input)
        {
            var lastchar = input[input.Length - 1];
            var op = input.TrimEnd(lastchar);
            ++lastchar;
            return op + lastchar;
        }

        private async Task<long> GetRank(string setName,string prefix)
        {
            var start = await _redisConnectionFactory.Client.Database.SortedSetRankAsync(setName, prefix);
            return start ?? -1;
        }

        private async Task<string[]> GetRange(string setName, long start, long end)
        {
            var result = await _redisConnectionFactory.Client.Database.SortedSetRangeByRankAsync(setName, start, end);
            var arrayresult = from r in result select r.HasValue ? r.ToString() : "";
            return arrayresult.ToArray();
        }

        #endregion private Methods
    }
}
