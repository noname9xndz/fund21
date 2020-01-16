using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;

namespace smartFunds.Business.Common
{
    public interface IGlobalConfigurationManager
    {
        Task<GlobalConfiguration> GetConfig(string name);
        Task<GlobalConfiguration> SaveConfig(GlobalConfiguration config);
        Task UpdateConfig(GlobalConfiguration config);
        Task<GlobalConfiguration> GetValueConfig(string name);
        Task SetValueConfig(string name, string value);
    }

    public class GlobalConfigurationManager : IGlobalConfigurationManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public GlobalConfigurationManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GlobalConfiguration> GetConfig(string name)
        {
            if (name == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var configuration = await _unitOfWork.GlobalConfigurationRepository.GetAsync(x => x.Name == name);
                if (configuration != null)
                {
                    return configuration;
                }
                throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GlobalConfiguration> SaveConfig(GlobalConfiguration config)
        {
            try
            {

                if (config == null) throw new InvalidParameterException();
                var savedConfig = _unitOfWork.GlobalConfigurationRepository.Add(config);
                await _unitOfWork.SaveChangesAsync();
                return savedConfig;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateConfig(GlobalConfiguration config)
        {
            try
            {
                if (config == null) throw new InvalidParameterException();
                _unitOfWork.GlobalConfigurationRepository.Update(config);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GlobalConfiguration> GetValueConfig(string name)
        {
            if (name == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var configuration = await _unitOfWork.GlobalConfigurationRepository.GetAsync(x => x.Name == name);
                if (configuration != null)
                {
                    return configuration;
                }
                throw new NotFoundException();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SetValueConfig(string name, string value)
        {
            if (name == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                using (var dbContextTransaction = _unitOfWork.GetCurrentContext().Database.BeginTransaction())
                {
                    var configuration = await _unitOfWork.GlobalConfigurationRepository.GetAsync(x => x.Name == name);
                    if (configuration != null)
                    {
                        await _unitOfWork.GlobalConfigurationRepository.ExecuteSql($"Update GlobalConfiguration SET [Value]=N'{value}' WHERE Name=N'{name}'");
                    }
                    else
                    {
                        await _unitOfWork.GlobalConfigurationRepository.ExecuteSql($"INSERT INTO [dbo].[GlobalConfiguration] ([Name],[Value])VALUES (N'{name}',N'{value}')");
                    }
                    dbContextTransaction.Commit();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
