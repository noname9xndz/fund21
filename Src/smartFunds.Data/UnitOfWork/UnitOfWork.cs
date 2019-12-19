using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data;
using smartFunds.Common.Options;
using smartFunds.Data.Repositories;
using smartFunds.Data.Repositories.HangFire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Data.Repositories.ContactBase;

namespace smartFunds.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ISettingRepository SettingRepository { get; }
        
        IInterchangeRepository InterchangeRepository { get; }
        IInterchangeLocalityRepository InterchangeLocalityRepository { get; }
        IJobRepository JobRepository { get; }
        IEventRepository EventRepository { get; }
        IMemberRepository MemberRepository { get; }
        IRegionRepository RegionRepository { get; }
        ICountryRepository CountryRepository { get; }
        ILocalityRepository LocalityRepository { get; }
        ISublocalityRepository SublocalityRepository { get; }
        IHostRepository HostRepository { get; }
        IEventGuestRepository EventGuestRepository { get; }
        IEventHostRepository EventHostRepository { get; }
        IMealAllocationRepository MealAllocationRepository { get; }
        ITestRepository TestRepository { get; }
        IKVRRQuestionRepository KVRRQuestionRepository { get; }
        IKVRRAnswerRepository KVRRAnswerRepository { get; }
        IUserRepository UserRepository { get; }
        IFAQRepository FAQRepository { get; }
        Task<int> SaveChangesAsync();
        Task BuildCacheAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly smartFundsDbContext _context;
        private readonly IOptions<smartFundsRedisOptions> _redisConfigurationOptions;
        private readonly IRedisAutoCompleteProvider _redisAutoCompleteProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool disposed;        

        //Repositories
        private SettingRepository _settingRepository;
        
        private IJobRepository _jobRepository;
        private IInterchangeRepository _interchangeRepository;
        private IInterchangeLocalityRepository _interchangeLocalityRepository;
        private IEventRepository _eventLocalityRepository;
        private IMemberRepository _memberRepository;
        private IRegionRepository _regionRepository;
        private ICountryRepository _countryRepository;
        private ILocalityRepository _localityRepository;
        private ISublocalityRepository _sublocalityRepository;
        private IHostRepository _hostRepository;
        private IEventGuestRepository _eventGuestRepository;
        private IEventHostRepository _eventHostRepository;
        private IMealAllocationRepository _mealAllocationRepository;
        private ITestRepository _testRepository;
        private IKVRRQuestionRepository _kvrrQuestionRepository;
        private IKVRRAnswerRepository _kvrrAnswerRepository;
        private IUserRepository _userRepository;
        private IFAQRepository _faqRepository;

        public UnitOfWork(IDbContextFactory<smartFundsDbContext> dbContextFactory, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor)
        {
            _context = dbContextFactory.GetContext();
            _redisConfigurationOptions = redisConfigurationOptions;
            _redisAutoCompleteProvider = redisAutoCompleteProvider;           
            _httpContextAccessor = httpContextAccessor;
        }

        public ISettingRepository SettingRepository
        {
            get
            {
                return _settingRepository = _settingRepository ?? new SettingRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }
        

        public IJobRepository JobRepository
        {
            get
            {
                return _jobRepository = _jobRepository ?? new JobRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IInterchangeRepository InterchangeRepository
        {
            get
            {
                return _interchangeRepository = _interchangeRepository ?? new InterchangeRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider, _httpContextAccessor);
            }
        }

        public IInterchangeLocalityRepository InterchangeLocalityRepository
        {
            get
            {
                return _interchangeLocalityRepository = _interchangeLocalityRepository ?? new InterchangeLocalityRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IEventRepository EventRepository
        {
            get
            {
                return _eventLocalityRepository = _eventLocalityRepository ?? new EventRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider, _httpContextAccessor);
            }
        }

        public IMemberRepository MemberRepository
        {
            get
            {
                return _memberRepository = _memberRepository ?? new MemberRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IRegionRepository RegionRepository
        {
            get
            {
                return _regionRepository = _regionRepository ?? new RegionRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public ICountryRepository CountryRepository
        {
            get
            {
                return _countryRepository = _countryRepository ?? new CountryRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public ILocalityRepository LocalityRepository
        {
            get
            {
                return _localityRepository = _localityRepository ?? new LocalityRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }


        public ISublocalityRepository SublocalityRepository
        {
            get
            {
                return _sublocalityRepository = _sublocalityRepository ?? new SublocalityRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IHostRepository HostRepository
        {
            get
            {
                return _hostRepository = _hostRepository ?? new HostRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IEventGuestRepository EventGuestRepository
        {
            get
            {
                return _eventGuestRepository = _eventGuestRepository ?? new EventGuestRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider, _httpContextAccessor);
            }
        }

        public IEventHostRepository EventHostRepository
        {
            get
            {
                return _eventHostRepository = _eventHostRepository ?? new EventHostRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider, _httpContextAccessor);
            }
        }

        public IMealAllocationRepository MealAllocationRepository
        {
            get
            {
                return _mealAllocationRepository = _mealAllocationRepository ?? new MealAllocationRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider, _httpContextAccessor);
            }
        }

        public ITestRepository TestRepository
        {
            get
            {
                return _testRepository = _testRepository ?? new TestRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IKVRRQuestionRepository KVRRQuestionRepository
        {
            get
            {
                return _kvrrQuestionRepository = _kvrrQuestionRepository ?? new KVRRQuestionRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }
        public IKVRRAnswerRepository KVRRAnswerRepository
        {
            get
            {
                return _kvrrAnswerRepository = _kvrrAnswerRepository ?? new KVRRAnswerRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository = _userRepository ?? new UserRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider, _httpContextAccessor);
            }
        }

        public IFAQRepository FAQRepository
        {
            get
            {
                return _faqRepository = _faqRepository ?? new FAQRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            var changeTracker = new List<ChangeTrackingEntity>();
            var changedEntries = _context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var item in changedEntries)
            {
                if (!(item.Entity is IAutoCompleteModel))
                    continue;

                changeTracker.Add(new ChangeTrackingEntity()
                {
                    Entity = item.Entity,
                    OriginalValue = item.OriginalValues.ToObject(),
                    State = item.State
                });
            }

            var saveChangeResult = await _context.SaveChangesAsync();

            if (_redisConfigurationOptions == null || !_redisConfigurationOptions.Value.EnableAutoComplete)
                return saveChangeResult;

            foreach (var item in changeTracker)
            {
                if (item.State == EntityState.Added)
                {
                    await _redisAutoCompleteProvider.AddAsync((IAutoCompleteModel)item.Entity);
                }
                else if (item.State == EntityState.Modified)
                {
                    await _redisAutoCompleteProvider.RemoveAsync((IAutoCompleteModel)item.OriginalValue);
                    await _redisAutoCompleteProvider.AddAsync((IAutoCompleteModel)item.Entity);
                }
                else if (item.State == EntityState.Deleted)
                {
                    await _redisAutoCompleteProvider.RemoveAsync((IAutoCompleteModel)item.Entity);
                }
            }

            return saveChangeResult;
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task BuildCacheAsync()
        {
            if (_redisConfigurationOptions.Value.EnableAutoComplete)
            {
               await SettingRepository.BuildCacheAsync();
               //await UserQueuedJobRepository.BuildCacheAsync();
            }
        }

    }
}
