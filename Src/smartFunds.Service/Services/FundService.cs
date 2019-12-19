using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using smartFunds.Business;
using smartFunds.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface IFundService
    {
        IEnumerable<FundModel> GetFunds(int pageSize, int pageIndex);
        FundModel GetFundById(int id);
        IEnumerable<FundModel> GetAllFund();
        Task<Fund> Save(FundModel fund);
        Task<Fund> Update(FundModel fund);
        Task<List<Fund>> Updates(List<FundModel> funds);
        IEnumerable<FundModel> GetFundByIds(int[] fundIds);
        Task<List<FundModel>> GetFundByStatus(EditStatus status);
        Task<bool> UpdateApprovedFunds(bool isApproved = true);
        Task<byte[]> ExportFund(SearchFund searchFund = null);
        Task DeleteFunds(int[] fundIds);
        bool IsExistPortfolioUsing(int[] fundIds);
        Task<bool> IsDuplicateName(string Name, string initTitle);
        Task<bool> IsDuplicateCode(string Name, string initTitle);
        List<SelectListItem> GetSelectListFund();
    }
    public class FundService : IFundService
    {
        private readonly IMapper _mapper;
        private readonly IFundManager _fundManager;
        public FundService(IMapper mapper, IFundManager fundManager)
        {
            _mapper = mapper;
            _fundManager = fundManager;
        }

        public IEnumerable<FundModel> GetFunds(int pageSize, int pageIndex)
        {
            try
            {
                var fundDto = _fundManager.GetFunds(pageSize, pageIndex);
                if (fundDto == null) return null;
                return _mapper.Map<IEnumerable<Fund>, IEnumerable<FundModel>>(fundDto);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public FundModel GetFundById(int id)
        {
            try
            {
                var fundDto = _fundManager.GetFundById(id);
                return _mapper.Map<Fund, FundModel>(fundDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<FundModel> GetAllFund()
        {
            var datas = _fundManager.GetAllFund();
            if (datas == null) return null;
            return _mapper.Map<IEnumerable<Fund>, IEnumerable<FundModel>>(datas);
        }

        public async Task<Fund> Save(FundModel fund)
        {
            try
            {
                var fundDto = _mapper.Map<FundModel, Fund>(fund);
                return await _fundManager.Save(fundDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Fund> Update(FundModel fund)
        {
            try
            {
                var fundDto = _mapper.Map<FundModel, Fund>(fund);
                return await _fundManager.Update(fundDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<FundModel> GetFundByIds(int[] fundIds)
        {
            try
            {
                var lstFunds = _fundManager.GetFundByIds(fundIds);
                var listFundDto = _mapper.Map<List<FundModel>>(lstFunds);
                return listFundDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FundModel>> GetFundByStatus(EditStatus status)
        {
            try
            {
                var lstFunds = _fundManager.GetFundByStatus(status).Result;
                var listFundDto = _mapper.Map<List<FundModel>>(lstFunds);
                return listFundDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Fund>> Updates(List<FundModel> funds)
        {
            try
            {
                var listFundDto = _mapper.Map<List<FundModel>, List<Fund>>(funds);
                return await _fundManager.Updates(listFundDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<byte[]> ExportFund(SearchFund searchFund = null)
        {
            return await _fundManager.ExportFund(searchFund);
        }

        public async Task<bool> UpdateApprovedFunds(bool isApproved = true)
        {
            try
            {
              return  await _fundManager.UpdateApprovedFunds(isApproved);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteFunds(int[] fundIds)
        {
            try
            {
                await _fundManager.DeleteFunds(fundIds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsExistPortfolioUsing(int[] fundIds)
        {
            try
            {
                return _fundManager.IsExistPortfolioUsing(fundIds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsDuplicateName(string Name, string initTitle)
        {
            try
            {
                return await _fundManager.IsDuplicateName(Name, initTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsDuplicateCode(string Code, string initTitle)
        {
            try
            {
                return await _fundManager.IsDuplicateCode(Code, initTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> GetSelectListFund()
        {
            var selectListFund = new List<SelectListItem>();
            var allFund = GetAllFund();
            foreach (var fund in allFund)
            {
                var selectItem = new SelectListItem { Value = fund.Id.ToString(), Text = fund.Title };
                selectListFund.Add(selectItem);
            }
            return selectListFund;
        }
    }
}
