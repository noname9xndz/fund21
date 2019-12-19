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

namespace smartFunds.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {                
        IJobRepository JobRepository { get; }
        ITestRepository TestRepository { get; }
        IFundPurchaseFeeRepository FundPurchaseFeeRepository { get; }
        IFundSellFeeRepository FundSellFeeRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderRequestRepository OrderRequestRepository { get; }
        IKVRRQuestionRepository KVRRQuestionRepository { get; }
        IKVRRAnswerRepository KVRRAnswerRepository { get; }
        IKVRRRepository KVRRRepository { get; }
        IKVRRMarkRepository KVRRMarkRepository { get; }
        IUserRepository UserRepository { get; }
        IFAQRepository FAQRepository { get; }
        IPortfolioRepository PortfolioRepository { get; }
        IKVRRPortfolioRepository KVRRPortfolioRepository { get; }
        ITransactionHistoryRepository TransactionHistoryRepository { get; }
        IInvestmentTargetRepository InvestmentTargetRepository { get; }
        IFundRepository FundRepository { get; }
        IPortfolioFundRepository PortfolioFundRepository { get; }
        ITaskRepository TaskRepository { get; }
        IUserFundRepository UserFundRepository { get; }
        IFundTransactionHistoryRepository FundTransactionHistoryRepository { get; }
        IContactCMSRepository ContactCMSRepository { get; }
        IHomepageCMSRepository HomepageCMSRepository { get; }
        IIntroducingPageCMSRepository IntroducingPageCMSRepository { get; }
        IInvestmentTargetCMSRepository InvestmentTargetCMSRepository { get; }
        ICustomerLevelRepository CustomerLevelRepository { get; }
        IMaintainingFeeRepository MaintainingFeeRepository { get; }
        IInvestmentRepository InvestmentRepository { get; }
        ITaskCompeletedRepository TaskCompeletedRepository { get; }
        IWithdrawalFeeRepository WithdrawalFeeRepository { get; }
        Task<int> SaveChangesAsync();
        smartFundsDbContext GetCurrentContext();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly smartFundsDbContext _context;
        private readonly IOptions<smartFundsRedisOptions> _redisConfigurationOptions;
        private readonly IRedisAutoCompleteProvider _redisAutoCompleteProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool disposed;        

        //Repositories       
        private IJobRepository _jobRepository;                       
        private ITestRepository _testRepository;
        private IFundPurchaseFeeRepository _fundPurchaseFeeRepository;
        private IFundSellFeeRepository _fundSellFeeRepository;
        private IOrderRepository _orderRepository;
        private IOrderRequestRepository _orderRequestRepository;
        private IKVRRQuestionRepository _kvrrQuestionRepository;
        private IKVRRRepository _kvrrRepository;
        private IKVRRMarkRepository _kvrrMarkRepository;
        private IKVRRAnswerRepository _kvrrAnswerRepository;
        private IUserRepository _userRepository;
        private IFAQRepository _faqRepository;
        private IPortfolioRepository _portfolioRepository;
        private IKVRRPortfolioRepository _kvrrPortfolioRepository;
        private ITransactionHistoryRepository _transactionHistoryRepository;
        private IInvestmentTargetRepository _investmentTargetRepository;
        private IFundRepository _fundRepository;
        private IPortfolioFundRepository _portfolioFundRepository;
        private ITaskRepository _taskRepository;
        private IUserFundRepository _userFundRepository;
        private IFundTransactionHistoryRepository _fundTransactionHistoryRepository;
        private IContactCMSRepository _contactCMSRepository;
        private IHomepageCMSRepository _homepageCMSRepository;
        private IIntroducingPageCMSRepository _introducingPageCMSRepository;
        private IInvestmentTargetCMSRepository _investmentTargetCMSRepository;
        private ICustomerLevelRepository _customerLevelRepository;
        private IMaintainingFeeRepository _maintainingFeeRepository;
        private IInvestmentRepository _investmentRepository;
        private ITaskCompeletedRepository _taskCompeletedRepository;
        private IWithdrawalFeeRepository _withdrawalFeeRepository;

        public UnitOfWork(IDbContextFactory<smartFundsDbContext> dbContextFactory, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor)
        {
            _context = dbContextFactory.GetContext();
            _redisConfigurationOptions = redisConfigurationOptions;
            _redisAutoCompleteProvider = redisAutoCompleteProvider;           
            _httpContextAccessor = httpContextAccessor;
        }
        

        public IJobRepository JobRepository
        {
            get
            {
                return _jobRepository = _jobRepository ?? new JobRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }         
     
        public ITestRepository TestRepository
        {
            get
            {
                return _testRepository = _testRepository ?? new TestRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IFundPurchaseFeeRepository FundPurchaseFeeRepository
        {
            get
            {
                return _fundPurchaseFeeRepository = _fundPurchaseFeeRepository ?? new FundPurchaseFeeRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IFundSellFeeRepository FundSellFeeRepository
        {
            get
            {
                return _fundSellFeeRepository = _fundSellFeeRepository ?? new FundSellFeeRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                return _orderRepository = _orderRepository ?? new OrderRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IOrderRequestRepository OrderRequestRepository
        {
            get
            {
                return _orderRequestRepository = _orderRequestRepository ?? new OrderRequestRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
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

        public IKVRRRepository KVRRRepository
        {
            get
            {
                return _kvrrRepository = _kvrrRepository ?? new KVRRRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider, _httpContextAccessor);
            }
        }

        public IKVRRMarkRepository KVRRMarkRepository
        {
            get
            {
                return _kvrrMarkRepository = _kvrrMarkRepository ?? new KVRRMarkRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
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

        public IPortfolioRepository PortfolioRepository
        {
            get
            {
                return _portfolioRepository = _portfolioRepository ?? new PortfolioRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider, _httpContextAccessor);
            }
        }

        public IKVRRPortfolioRepository KVRRPortfolioRepository
        {
            get
            {
                return _kvrrPortfolioRepository = _kvrrPortfolioRepository ?? new KVRRPortfolioRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public ITransactionHistoryRepository TransactionHistoryRepository
        {
            get
            {
                return _transactionHistoryRepository = _transactionHistoryRepository ?? new TransactionHistoryRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IInvestmentTargetRepository InvestmentTargetRepository
        {
            get
            {
                return _investmentTargetRepository = _investmentTargetRepository ?? new InvestmentTargetRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider, _httpContextAccessor);
            }
        }

        public IFundRepository FundRepository
        {
            get
            {
                return _fundRepository = _fundRepository ?? new FundRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider, _httpContextAccessor);
            }
        }

        public IPortfolioFundRepository PortfolioFundRepository
        {
            get
            {
                return _portfolioFundRepository = _portfolioFundRepository ?? new PortfolioFundRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public ITaskRepository TaskRepository
        {
            get
            {
                return _taskRepository = _taskRepository ?? new TaskRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public ITaskCompeletedRepository TaskCompeletedRepository
        {
            get
            {
                return _taskCompeletedRepository = _taskCompeletedRepository ?? new TaskCompeletedRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IUserFundRepository UserFundRepository
        {
            get
            {
                return _userFundRepository = _userFundRepository ?? new UserFundRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IContactCMSRepository ContactCMSRepository
        {
            get
            {
                return _contactCMSRepository = _contactCMSRepository ?? new ContactCMSRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IHomepageCMSRepository HomepageCMSRepository
        {
            get
            {
                return _homepageCMSRepository = _homepageCMSRepository ?? new HomepageCMSRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IFundTransactionHistoryRepository FundTransactionHistoryRepository
        {
            get
            {
                return _fundTransactionHistoryRepository = _fundTransactionHistoryRepository ?? new FundTransactionHistoryRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IIntroducingPageCMSRepository IntroducingPageCMSRepository
        {
            get
            {
                return _introducingPageCMSRepository = _introducingPageCMSRepository ?? new IntroducingPageCMSRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }
        public IInvestmentTargetCMSRepository InvestmentTargetCMSRepository
        {
            get
            {
                return _investmentTargetCMSRepository = _investmentTargetCMSRepository ?? new InvestmentTargetCMSRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public ICustomerLevelRepository CustomerLevelRepository
        {
            get
            {
                return _customerLevelRepository = _customerLevelRepository ?? new CustomerLevelRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider, _httpContextAccessor);
            }
        }

        public IMaintainingFeeRepository MaintainingFeeRepository
        {
            get
            {
                return _maintainingFeeRepository = _maintainingFeeRepository ?? new MaintainingFeeRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IInvestmentRepository InvestmentRepository
        {
            get
            {
                return _investmentRepository = _investmentRepository ?? new InvestmentRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public IWithdrawalFeeRepository WithdrawalFeeRepository
        {
            get
            {
                return _withdrawalFeeRepository = _withdrawalFeeRepository ?? new WithdrawalFeeRepository(_context, _redisConfigurationOptions, _redisAutoCompleteProvider);
            }
        }

        public smartFundsDbContext GetCurrentContext()
        {
            return _context;
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

    }
}
