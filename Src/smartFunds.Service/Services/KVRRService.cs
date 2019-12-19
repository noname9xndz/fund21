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
using smartFunds.Model.Common;
using KVRR = smartFunds.Data.Models.KVRR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace smartFunds.Service.Services
{
    public interface IKVRRService
    {
        IEnumerable<smartFunds.Model.Common.KVRRModel> GetKVRRs(int pageSize, int pageIndex);
        KVRRModel GetKVRRById(int id);
        Task<IEnumerable<KVRRModel>> GetAllKVRR();
        Task<KVRRModel> Save(KVRRModel kvrr);
        Task Update(KVRRModel kvrr);
        Task<KVRRModel> GetKVRRByMark(List<int> ids);
        Task<ICollection<KVRRModel>> GetKVRRMarkUnUse();
        Task<bool> IsDuplicateName(string Name, string initTitle);
        List<SelectListItem> GetSelectListKVRR();
    }
    public class KVRRService : IKVRRService
    {
        private readonly IMapper _mapper;
        private readonly IKVRRManager _kvrrManager;
        public KVRRService(IMapper mapper, IKVRRManager kvrrManager)
        {
            _mapper = mapper;
            _kvrrManager = kvrrManager;
        }

        public IEnumerable<KVRRModel> GetKVRRs(int pageSize, int pageIndex)
        {
            var kvrrDto = _kvrrManager.GetKVRRs(pageSize, pageIndex);
            if (kvrrDto == null) return null;
            return _mapper.Map<IEnumerable<KVRR>, IEnumerable<KVRRModel>>(kvrrDto);
        }

        public async Task<IEnumerable<KVRRModel>> GetAllKVRR()
        {
            var kvrrDto = await _kvrrManager.GetAllKVRR();
            if (kvrrDto == null) return null;
            return _mapper.Map<IEnumerable<KVRR>, IEnumerable<KVRRModel>>(kvrrDto);
        }

        public KVRRModel GetKVRRById(int id)
        {
            var kvrrDto = _kvrrManager.GetKVRRById(id);
            return _mapper.Map<KVRR, KVRRModel>(kvrrDto);
        }

        public async Task<KVRRModel> Save(KVRRModel kvrr)
        {
            var kvrrDto = _mapper.Map<KVRRModel, KVRR>(kvrr);
            var dto = await _kvrrManager.Save(kvrrDto);
            var savedKVRRModel = _mapper.Map<KVRRModel>(dto);
            return savedKVRRModel;
        }

        public async Task Update(KVRRModel kvrr)
        {
            try
            {
                var kvrrDto = _mapper.Map<KVRRModel, KVRR>(kvrr);
                await _kvrrManager.Update(kvrrDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KVRRModel> GetKVRRByMark(List<int> ids)
        {
            try
            {
                var kvrr = await _kvrrManager.GetKVRRByMark(ids);
                return _mapper.Map<KVRRModel>(kvrr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ICollection<KVRRModel>> GetKVRRMarkUnUse()
        {
            try
            {
                var kvrr = await _kvrrManager.GetKVRRMarkUnUse();
                return _mapper.Map<List<KVRRModel>>(kvrr);
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
                return await _kvrrManager.IsDuplicateName(Name, initTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> GetSelectListKVRR()
        {
            var selectListKVRR = new List<SelectListItem>();
            var allKVRR = GetAllKVRR().Result;
            foreach (var kvrr in allKVRR)
            {
                var selectItem = new SelectListItem { Value = kvrr.Id.ToString(), Text = kvrr.Name };
                selectListKVRR.Add(selectItem);
            }
            return selectListKVRR;
        }
    }
}
