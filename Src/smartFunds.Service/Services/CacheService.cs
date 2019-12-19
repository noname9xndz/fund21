using smartFunds.Caching;
using smartFunds.Common;
using smartFunds.Data.Util;
using smartFunds.Data.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface ICacheService {
        Task<BuildCacheInfo> BuildCache();
        Task<bool> SetBuildCacheStatusToQueue();
        Task<BuildCacheInfo> GetBuildCacheInfo();
    }
    public class CacheService : ICacheService
    {
        private readonly IRedisCacheProvider _redisCacheProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheBuilder _cacheBuilder;
        public CacheService(IRedisCacheProvider redisCacheProvider, IServiceProvider serviceProvider, IUnitOfWork unitOfWork,
            ICacheBuilder cacheBuilder) 
        {
            _redisCacheProvider = redisCacheProvider;
            _serviceProvider = serviceProvider;
            _unitOfWork = unitOfWork;
            _cacheBuilder = cacheBuilder;
        }

        public async Task<BuildCacheInfo> BuildCache()
        {
            var buildInfo = await _redisCacheProvider.GetBuildCacheInfoAsync();
            if (buildInfo?.Status == BuildCacheStatusType.Queued)
            {
                try
                {
                    _redisCacheProvider.FlushCache();
                    buildInfo.Status = BuildCacheStatusType.InProgress;
                    buildInfo.StartTime = DateTime.Now.ToString();
                    buildInfo.Heartbeat = Constants.Cache.HeartbeatMessage.StartsmartFunds;
                    buildInfo.Error = "";
                    buildInfo.EndTime = null;
                    await _redisCacheProvider.SetBuildCacheInfoAsync(buildInfo);
                    //await _unitOfWork.BuildCacheAsync();
                    buildInfo.Heartbeat = Constants.Cache.HeartbeatMessage.FinishsmartFunds;
                    await _redisCacheProvider.SetBuildCacheInfoAsync(buildInfo);

                    buildInfo.Heartbeat = Constants.Cache.HeartbeatMessage.StartContactBase;
                    await _redisCacheProvider.SetBuildCacheInfoAsync(buildInfo);
                    await _cacheBuilder.BuildCacheAsync(_serviceProvider);
                    buildInfo.Heartbeat = Constants.Cache.HeartbeatMessage.FinishContactBase;
                    buildInfo.EndTime = DateTime.Now.ToString();
                    buildInfo.Status = BuildCacheStatusType.Finished;
                    await _redisCacheProvider.SetBuildCacheInfoAsync(buildInfo);
                }
                catch (Exception ex)
                {
                    buildInfo.Error = ex.ToString();
                    buildInfo.Status = BuildCacheStatusType.Error;
                    await _redisCacheProvider.SetBuildCacheInfoAsync(buildInfo);
                }
            }

            return buildInfo;
        }

        public async Task<bool> SetBuildCacheStatusToQueue()
        {
            var buildInfo = await _redisCacheProvider.GetBuildCacheInfoAsync();
            if (buildInfo?.Status != BuildCacheStatusType.Queued && buildInfo?.Status != BuildCacheStatusType.InProgress)
            {
                buildInfo.Status = BuildCacheStatusType.Queued;
                await _redisCacheProvider.SetBuildCacheInfoAsync(buildInfo);
                return true;
            }

            return false;
        }

        public async Task<BuildCacheInfo> GetBuildCacheInfo()
        {
            return await _redisCacheProvider.GetBuildCacheInfoAsync();
        }
    }
}
