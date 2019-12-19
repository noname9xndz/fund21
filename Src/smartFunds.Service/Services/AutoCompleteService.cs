using smartFunds.Caching.AutoComplete;
using smartFunds.Common;
using smartFunds.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using smartFunds.Data.Models.Contactbase;

namespace smartFunds.Service.Services
{
    public interface IAutoCompleteService
    {
        Task<IList<AutoCompleteItem>> GetItemsAsync(AutoCompleteType type, string key);
    }

    public class AutoCompleteService : IAutoCompleteService
    {
        private readonly IRedisAutoCompleteProvider _redisAutoCompleteProvide;
        public AutoCompleteService(IRedisAutoCompleteProvider redisAutoCompleteProvider)
        {
            _redisAutoCompleteProvide = redisAutoCompleteProvider;
        }

        public async Task<IList<AutoCompleteItem>> GetItemsAsync(AutoCompleteType type, string key)
        {
            return await _redisAutoCompleteProvide.GetAsync(ResolveModel(type), key);
        }

        private static string ResolveModel(AutoCompleteType type)
        {
            switch (type)
            {
                case AutoCompleteType.Setting:
                    return new Setting().AutoCompleteItem.SetName;
                case AutoCompleteType.Member:
                    return new Member().AutoCompleteItem.SetName;
                case AutoCompleteType.ContactBaseCountry:
                    return new Country().AutoCompleteItem.SetName;
                case AutoCompleteType.ContactBaseLocality:
                    return new Locality().AutoCompleteItem.SetName;
                default:
                    throw new InvalidOperationException("Auto complete type not implemented");
            }
        }
    }
}
