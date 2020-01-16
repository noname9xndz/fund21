using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using smartFunds.Business.Common;
using smartFunds.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;

namespace smartFunds.Business
{
    public interface IKVRRMarkManager
    {
        Task<IEnumerable<KVRRMark>> GetKVRRMarksAsync(int pageSize, int pageIndex);
        Task<KVRRMark> GetKVRRMarkById(int id);
        Task<KVRRMark> Save(KVRRMark mark);
        Task<KVRRMark> Update(KVRRMark mark);
        Task Delete(int[] ids);
        Task<bool> ValidMark(KVRRMark currentMark);
        Task<bool> KVRRIsNotEmpty(int kvrrId, int initKvrrId);
    }
    public class KVRRMarkManager : IKVRRMarkManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;

        public KVRRMarkManager(IUnitOfWork unitOfWork, IUserManager userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IEnumerable<KVRRMark>> GetKVRRMarksAsync(int pageSize, int pageIndex)
        {
            if (pageSize < 0 || pageIndex < 0) throw new InvalidParameterException();

            if (pageSize == 0 && pageIndex == 0) return await _unitOfWork.KVRRMarkRepository.GetAllAsync("KVRR");

            return (await _unitOfWork.KVRRMarkRepository.GetAllAsync("KVRR"))?.Take(pageSize).Skip((pageIndex - 1) * pageSize).ToList();
        }

        public async Task<KVRRMark> GetKVRRMarkById(int id)
        {
            return await _unitOfWork.KVRRMarkRepository.GetAsync(k => k.Id == id, "KVRR");
        }

        public async Task<KVRRMark> Save(KVRRMark mark)
        {
            try
            {
                if (mark == null) throw new InvalidParameterException();

                mark.DateLastUpdated = DateTime.Now;
                mark.LastUpdatedBy = _userManager.CurrentUser();
                mark.KVRRId = mark.KVRRId == 0 ? null : mark.KVRRId;
                mark.KVRR = mark.KVRRId != null ? _unitOfWork.KVRRRepository.GetKVRRById(mark.KVRRId.Value) : null;
                var result = _unitOfWork.KVRRMarkRepository.Add(mark);
                await _unitOfWork.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KVRRMark> Update(KVRRMark mark)
        {
            try
            {
                if (mark == null) throw new InvalidParameterException();

                mark.DateLastUpdated = DateTime.Now;
                mark.LastUpdatedBy = _userManager.CurrentUser();
                mark.KVRRId = mark.KVRRId == 0 ? null : mark.KVRRId;
                mark.KVRR = mark.KVRRId != null ? _unitOfWork.KVRRRepository.GetKVRRById(mark.KVRRId.Value) : null;
                _unitOfWork.KVRRMarkRepository.Update(mark);
                await _unitOfWork.SaveChangesAsync();
                return mark;
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
                if (ids == null || !ids.Any()) throw new InvalidParameterException();

                var marks = await _unitOfWork.KVRRMarkRepository.FindByAsync(a => ids.Contains(a.Id));
                if(marks == null || !marks.Any()) throw new NotFoundException();
                _unitOfWork.KVRRMarkRepository.BulkDelete(marks.ToList());
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ValidMark(KVRRMark currentMark)
        {
            var marks = await _unitOfWork.KVRRMarkRepository.GetAllAsync();
            if (marks == null || !marks.Any()) return true;

            if (currentMark.EntityState == FormState.Edit)
                marks = marks.Where(x => x.Id != currentMark.Id);//.ToList();

            var existMark = marks.Where(x => (currentMark.MarkFrom >= x.MarkFrom && currentMark.MarkTo <= x.MarkTo)
                             || (currentMark.MarkFrom >= x.MarkFrom && currentMark.MarkFrom <= x.MarkTo && currentMark.MarkTo > x.MarkTo)
                             || (currentMark.MarkFrom < x.MarkFrom && currentMark.MarkTo >= x.MarkFrom && currentMark.MarkTo <= x.MarkTo)
                             || (currentMark.MarkFrom < x.MarkFrom && currentMark.MarkTo > x.MarkTo)).ToList();

            if (existMark != null && existMark.Any())
                return false;            

            return true;
        }

        public async Task<bool> KVRRIsNotEmpty(int kvrrId, int initKvrrId)
        {
            if (kvrrId <= 0)
                return false;
            return true;
        }
    }
}
