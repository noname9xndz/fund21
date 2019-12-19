using System;
using System.Reflection;
using System.Threading.Tasks;

namespace smartFunds.Data.Util
{
    public interface ICacheBuilder
    {
        Task BuildCacheAsync(IServiceProvider serviceProvider);
    }

    public class CacheBuilder : ICacheBuilder
    {
        public async Task BuildCacheAsync(IServiceProvider serviceProvider)
        {
            foreach (Type type in RepositoryTypeHelper.GetRepositoryTypes())
            {
                if (type.Name.ToLower().IndexOf("generic") > -1 || type.Name.ToLower().IndexOf("repository") < 0)
                {
                    continue;
                }

                object instance = serviceProvider.GetService(type);
                MethodInfo method = type.GetMethod("BuildCacheAsync");
                if (method == null || instance == null)
                {
                    continue;
                }
                method.Invoke(instance, new object[0]);
                var task = (Task)method.Invoke(instance, new object[0]);
                await task;
            }
        }
    }
}
