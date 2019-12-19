using AutoMapper;
using smartFunds.Business;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Auth;
using Microsoft.AspNetCore.Http;
using smartFunds.Model.Common;
using smartFunds.Common;

namespace smartFunds.Service.Services
{
    public interface IPortfolioService
    {
        IEnumerable<smartFunds.Model.Common.PortfolioModel> GetPortfolios(int pageSize, int pageIndex);
        PortfolioModel GetPortfolioById(int id);
        IEnumerable<PortfolioModel> GetAllPortfolio();
        Task<List<PortfolioModel>> GetPortfolioByStatus(EditStatus status);
        Task<List<PortfolioFundModel>> GetPortfolioFundByPortfolioId(int portfolioId, EditStatus status);
        Task<List<PortfolioFundModel>> GetPortfolioFundByPortfolioId(int portfolioId);
        Task RejectedPortfolio(int idPortfolio);
        Task<IEnumerable<PortfolioModel>> GetPortfoliosUnUse();
        Task<Portfolio> Save(PortfolioModel portfolio);
        Task<Portfolio> Update(PortfolioModel portfolio);
        Task<byte[]> ExportPortfolio(SearchPortfolio searchPortfolio = null);
        Task SaveFunds(PortfolioModel portfolioModel);
        Task<bool> IsPortfolioNameExists(string Title, string initTitle);
        Task UpdatePortfolioImage(HomepageCMSModel model, string typeUpload = "", int Id = 0);
    }
    public class PortfolioService : IPortfolioService
    {
        private readonly IMapper _mapper;
        private readonly IPortfolioManager _portfolioManager;
        public PortfolioService(IMapper mapper, IPortfolioManager portfolioManager)
        {
            _mapper = mapper;
            _portfolioManager = portfolioManager;
        }

        public async Task<List<PortfolioModel>> GetPortfolioByStatus(EditStatus status)
        {
            try
            {
                var lstPortfolios = _portfolioManager.GetPortfolioByStatus(status).Result;
                var listPortfolioDto = _mapper.Map<List<PortfolioModel>>(lstPortfolios);
                return listPortfolioDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PortfolioFundModel>> GetPortfolioFundByPortfolioId(int portfolioId, EditStatus status)
        {
            try
            {
                var lstPortfolioFunds = _portfolioManager.GetPortfolioFundByPortfolioId(portfolioId, status).Result;
                var listPortfolioFundDto = _mapper.Map<List<PortfolioFundModel>>(lstPortfolioFunds);
                return listPortfolioFundDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PortfolioFundModel>> GetPortfolioFundByPortfolioId(int portfolioId)
        {
            try
            {
                var lstPortfolioFunds = _portfolioManager.GetPortfolioFundByPortfolioId(portfolioId).Result;
                var listPortfolioFundDto = _mapper.Map<List<PortfolioFundModel>>(lstPortfolioFunds);
                return listPortfolioFundDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PortfolioModel> GetPortfolios(int pageSize, int pageIndex)
        {
            try
            {
                var portfolioDto = _portfolioManager.GetAllPortfolios(pageSize, pageIndex);
                if (portfolioDto == null) return null;
                return _mapper.Map<IEnumerable<Portfolio>, IEnumerable<PortfolioModel>>(portfolioDto);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PortfolioModel GetPortfolioById(int id)
        {
            try
            {
                var portfolioDto = _portfolioManager.GetPortfolioById(id);
                return _mapper.Map<Portfolio, PortfolioModel>(portfolioDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PortfolioModel> GetAllPortfolio()
        {
            var datas = _portfolioManager.GetAllPortfolio();
            if (datas == null) return null;
            return _mapper.Map<IEnumerable<Portfolio>, IEnumerable<PortfolioModel>>(datas.Result);
        }

        public async Task<IEnumerable<PortfolioModel>> GetPortfoliosUnUse()
        {
            try
            {
                var data = await _portfolioManager.GetPortfoliosUnUse();
                if (data == null) return null;
                return _mapper.Map<IEnumerable<PortfolioModel>>(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Portfolio> Save(PortfolioModel portfolio)
        {
            try
            {
                var portfolioDto = _mapper.Map<PortfolioModel, Portfolio>(portfolio);
                return await _portfolioManager.Save(portfolioDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Portfolio> Update(PortfolioModel portfolio)
        {
            try
            {
                var portfolioDto = _mapper.Map<PortfolioModel, Portfolio>(portfolio);
                return await _portfolioManager.Update(portfolioDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<byte[]> ExportPortfolio(SearchPortfolio searchPortfolio = null)
        {
            return await _portfolioManager.ExportPortfolio(searchPortfolio);
        }

        public async Task RejectedPortfolio(int idPortfolio)
        {
            try
            {
                await _portfolioManager.RejectedPortfolio(idPortfolio);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveFunds(PortfolioModel portfolioModel)
        {
            try
            {
                var portfolio = _mapper.Map<Portfolio>(portfolioModel);
                await _portfolioManager.SaveFunds(portfolio);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsPortfolioNameExists(string Title, string initTitle)
        {
            try
            {
                return await _portfolioManager.IsPortfolioNameExists(Title, initTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdatePortfolioImage(HomepageCMSModel model, string typeUpload = "", int Id = 0)
        {
            try
            {
                HomepageCMS data = _mapper.Map<HomepageCMS>(model);
                await _portfolioManager.UpdatePortfolioImage(data, typeUpload, Id );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
