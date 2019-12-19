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
    public interface IKVRRManager
    {
        IEnumerable<KVRR> GetKVRRs(int pageSize, int pageIndex);
        Task<IEnumerable<KVRR>> GetAllKVRR();
        Task<ICollection<KVRR>> GetKVRRMarkUnUse();
        KVRR GetKVRRById(int id);
        Task<KVRR> Save(KVRR kvrr);
        Task Update(KVRR kvrr);
        Task<KVRR> GetKVRRByMark(List<int> ids);
        Task<bool> IsDuplicateName(string newValue, string initValue);
    }
    public class KVRRManager : IKVRRManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public KVRRManager(IUnitOfWork unitOfWork, IUserManager userManager, IHostingEnvironment environment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _hostingEnvironment = environment;
        }

        public IEnumerable<KVRR> GetKVRRs(int pageSize, int pageIndex)
        {
            if (pageSize < 0 || pageIndex < 0) throw new InvalidParameterException();

            if (pageSize == 0 && pageIndex == 0) return _unitOfWork.KVRRRepository.GetAllKVRR().ToList();

            return _unitOfWork.KVRRRepository.GetAllKVRR().Take(pageSize).Skip((pageIndex - 1) * pageSize).ToList();
        }

        public async Task<IEnumerable<KVRR>> GetAllKVRR()
        {
            return await _unitOfWork.KVRRRepository.GetAllAsync();
        }

        public async Task<ICollection<KVRR>> GetKVRRMarkUnUse()
        {
            try
            {
                var currentKvrrMarks = await _unitOfWork.KVRRMarkRepository.FindByAsync(x => x.KVRRId != null);
                if (currentKvrrMarks == null) throw new NotFoundException();
                var currentKvrrMarkIds = currentKvrrMarks.Select(x => x.KVRRId).ToList();
                return await _unitOfWork.KVRRRepository.FindByAsync(x => !currentKvrrMarkIds.Contains(x.Id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public KVRR GetKVRRById(int id)
        {
            return _unitOfWork.KVRRRepository.GetKVRRById(id);
        }

        public async Task<KVRR> Save(KVRR kvrr)
        {
            try
            {
                if (kvrr == null) return null;

                kvrr.DateLastUpdated = DateTime.Now;
                kvrr.LastUpdatedBy = _userManager.CurrentUser();

                //upload image
                if (kvrr.KVRRImage?.Length > 0)
                {
                    kvrr = UploadKVRRImage(kvrr).Result;
                }

                var result = _unitOfWork.KVRRRepository.Add(kvrr);

                var kpsNew = new List<KVRRPortfolio>();
                // 1 kvrr - multi Portfolio
                //if (kvrr.PortfolioIds != null && kvrr.PortfolioIds.Any())
                //{
                //    foreach (var portfolio in kvrr.PortfolioIds)
                //    {
                //        kpsNew.Add(new KVRRPortfolio
                //        {
                //            KVRRId = kvrr.Id,
                //            PortfolioId = Int32.Parse(portfolio)
                //        });
                //    }
                //}

                // 1 kvrr - 1 Portfolio
                kpsNew.Add(new KVRRPortfolio
                {
                    KVRRId = kvrr.Id,
                    PortfolioId = Int32.Parse(kvrr.PortfolioId)
                });
                await CreateOrUpdateKVRRPortfolio(kpsNew);
                await _unitOfWork.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(KVRR kvrr)
        {
            try
            {
                if (kvrr == null) throw new InvalidParameterException(); 

                //delete old image if exist
                if (kvrr.KVRRImage != null)
                    DeleteOldKVRRImage(kvrr.KVRRImagePath);

                kvrr.DateLastUpdated = DateTime.Now;
                kvrr.LastUpdatedBy = _userManager.CurrentUser();

                //upload new image
                if (kvrr.KVRRImage?.Length > 0)
                {
                    kvrr = UploadKVRRImage(kvrr).Result;
                }

                _unitOfWork.KVRRRepository.Update(kvrr);

                var kpsNew = new List<KVRRPortfolio>();
                // 1 kvrr - multi Portfolio
                //if (kvrr.PortfolioIds != null && kvrr.PortfolioIds.Any())
                //{
                //    foreach (var portfolio in kvrr.PortfolioIds)
                //    {
                //        kpsNew.Add(new KVRRPortfolio
                //        {
                //            KVRRId = kvrr.Id,
                //            PortfolioId = Int32.Parse(portfolio)
                //        });
                //    }
                //}

                // 1 kvrr - 1 Portfolio
                kpsNew.Add(new KVRRPortfolio
                {
                    KVRRId = kvrr.Id,
                    PortfolioId = Int32.Parse(kvrr.PortfolioId)
                });
                await CreateOrUpdateKVRRPortfolio(kpsNew);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KVRR> GetKVRRByMark(List<int> ids)
        {
            try
            {
                var mark = 0;
                var answers = await _unitOfWork.KVRRAnswerRepository.FindByAsync(x => ids.Contains(x.Id));
                if(answers == null || !answers.Any()) throw new NotFoundException();
                foreach (var kvrrAnswer in answers)
                {
                    mark += kvrrAnswer.Mark.HasValue ? kvrrAnswer.Mark.Value : 0;
                }
                var kvrrMark = await _unitOfWork.KVRRMarkRepository.GetAsync(x => x.MarkFrom <= mark && x.MarkTo >= mark, "KVRR");
                return kvrrMark?.KVRR ?? throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<KVRR> UploadKVRRImage(KVRR kvrr)
        {         
            if (kvrr.KVRRImage.Length > 0)
            {
                var fileName = kvrr.KVRRImage.FileName.Split('\\').Last();
                var filePath = $"{_hostingEnvironment.WebRootPath}{smartFunds.Common.Constants.KVRRImageFolder.Path}{fileName}";

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await kvrr.KVRRImage.CopyToAsync(stream);
                }
                kvrr.KVRRImagePath = fileName;
            }
           
            return kvrr;
        }

        private bool DeleteOldKVRRImage(string oldName)
        {
            var oldPath = $"{_hostingEnvironment.WebRootPath}{smartFunds.Common.Constants.KVRRImageFolder.Path}{oldName}";
            try
            {
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
                return true;
            }
            catch (IOException ioExp)
            {
                return false;
            }
        }

        private async Task<bool> CreateOrUpdateKVRRPortfolio(IEnumerable<KVRRPortfolio> kps)
        {
            try
            {
                if (kps == null) return false;

                //delete all kvrrPortfolio by portfolioId then add new list kps into db again
                var kpsOld = await _unitOfWork.KVRRPortfolioRepository.FindByAsync(m => m.KVRRId == kps.First().KVRRId);
                //_unitOfWork.KVRRPortfolioRepository.BulkDelete(kpsOld);

                foreach (var itemOld in kpsOld)
                {
                    _unitOfWork.KVRRPortfolioRepository.Delete(itemOld);
                }
                foreach (var kp in kps)
                {
                    _unitOfWork.KVRRPortfolioRepository.Add(kp);
                }
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsDuplicateName(string newValue, string initValue)
        {
            if (!string.IsNullOrEmpty(newValue) && newValue.Trim().Equals(initValue))
                return true;
            var isValueExisted = await _unitOfWork.KVRRRepository.FindByAsync(x => x.Name.ToLower().Equals(newValue.ToLower()));
            if (isValueExisted != null && isValueExisted.Any())
                return false;

            return true;
        }
    }
}
