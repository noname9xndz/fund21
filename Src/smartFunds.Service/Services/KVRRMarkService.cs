using AutoMapper;
using smartFunds.Business;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Service.Services
{
    public interface IKVRRMarkService
    {
        Task<List<KVRRMarkModel>> GetKVRRMarks(int pageSize, int pageIndex);
        Task<KVRRMarkModel> GetKVRRMarkById(int id);
        Task<KVRRMark> Save(KVRRMarkModel mark);
        Task<KVRRMark> Update(KVRRMarkModel mark);
        Task Delete(int[] ids);
        Task<bool> ValidMark(KVRRMarkModel currentMark);
        Task<bool> KVRRIsNotEmpty(int kvrrId, int initKvrrId);
    }
    public class KVRRMarkService : IKVRRMarkService
    {
        private readonly IMapper _mapper;
        private readonly IKVRRMarkManager _kvrrMarkManager;
        public KVRRMarkService(IMapper mapper, IKVRRMarkManager kvrrMarkManager)
        {
            _mapper = mapper;
            _kvrrMarkManager = kvrrMarkManager;
        }

        public async Task<List<KVRRMarkModel>> GetKVRRMarks(int pageSize, int pageIndex)
        {
            try
            {
                var kvrrDto = await _kvrrMarkManager.GetKVRRMarksAsync(pageSize, pageIndex);
                if (kvrrDto == null) return null;
                return _mapper.Map<List<KVRRMark>, List<KVRRMarkModel>>(kvrrDto.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KVRRMarkModel> GetKVRRMarkById(int id)
        {
            try
            {
                var markDto = await _kvrrMarkManager.GetKVRRMarkById(id);
                return _mapper.Map<KVRRMark, KVRRMarkModel>(markDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KVRRMark> Save(KVRRMarkModel mark)
        {
            try
            {
                var markDto = _mapper.Map<KVRRMarkModel, KVRRMark>(mark);
                return await _kvrrMarkManager.Save(markDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KVRRMark> Update(KVRRMarkModel mark)
        {
            try
            {
                var markDto = _mapper.Map<KVRRMarkModel, KVRRMark>(mark);
                return await _kvrrMarkManager.Update(markDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Delete(int[] ids)
        {
            try
            {
                await _kvrrMarkManager.Delete(ids);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ValidMark(KVRRMarkModel mark)
        {
            try
            {
                var markDto = _mapper.Map<KVRRMark>(mark);
                return await _kvrrMarkManager.ValidMark(markDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> KVRRIsNotEmpty(int kvrrId, int initKvrrId)
        {
            try
            {
                return await _kvrrMarkManager.KVRRIsNotEmpty(kvrrId, initKvrrId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
