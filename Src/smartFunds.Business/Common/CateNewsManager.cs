using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using smartFunds.Common;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using smartFunds.Model.Common;
using LinqKit;
using System.Text.RegularExpressions;
using smartFunds.Common.Helpers;
using static smartFunds.Common.Constants;
using OfficeOpenXml.Style;
using System.Web;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using smartFunds.Common.Exceptions;

namespace smartFunds.Business.Common
{

    public interface ICateNewsManager
    {
        Task<CateNews> GetCateNews(int? cateId);
        Task<List<CateNews>> GetListCateNews();
        Task<CateNews> SaveCateNews(CateNews cate);
        Task UpdateCateNews(CateNews cate);
        Task DeleteCateNews(int? cateId);

        Task DeleteListCateNews(int[] cateIds);
        Task<List<CateNews>> GetAllCateNews();
      
    }
    public  class CateNewsManager : ICateNewsManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CateNewsManager(IUnitOfWork unitOfWork, IUserManager userManager, IHostingEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<CateNews> GetCateNews(int? cateId)
        {
            if (cateId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var cate = await _unitOfWork.CateNewsRepository.GetAsync(m => m.Id == cateId);
                if (cate != null)
                {
                    return cate;
                }
                throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CateNews>> GetListCateNews()
        {

            try
            {
                IQueryable<CateNews> allCate = await _unitOfWork.CateNewsRepository.GetAllAsync();
                List<CateNews> cates = allCate.ToList();
                return cates;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CateNews> SaveCateNews(CateNews cate)
        {
            try
            {
                cate.DateLastUpdated = DateTime.Now;
                cate.LastUpdatedBy = _userManager.CurrentUser();
                cate.CateNewsName = cate.CateNewsName.Replace("\r\n", "<br/>");
                cate.Contents = HttpUtility.HtmlDecode(cate.Contents);
                var savedCate = _unitOfWork.CateNewsRepository.Add(cate);
                await _unitOfWork.SaveChangesAsync();
                return savedCate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateCateNews(CateNews cate)
        {
            try
            {
                cate.DateLastUpdated = DateTime.Now;
                cate.LastUpdatedBy = _userManager.CurrentUser();
                cate.CateNewsName = cate.CateNewsName.Replace("\r\n", "<br/>");
                cate.Contents = HttpUtility.HtmlDecode(cate.Contents);
                _unitOfWork.CateNewsRepository.Update(cate);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteCateNews(int? cateId)
        {
            if (cateId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var cate = _unitOfWork.CateNewsRepository.GetAsync(m => m.Id == cateId).Result;
                if (cate == null)
                {
                    throw new NotFoundException();
                }
                _unitOfWork.CateNewsRepository.Delete(cate);
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteListCateNews(int[] cateIds)
        {
            if (cateIds == null || !cateIds.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                var cates = await _unitOfWork.CateNewsRepository.FindByAsync(i => cateIds.Contains(i.Id));
                if (cates != null && cates.Any())
                {
                    _unitOfWork.CateNewsRepository.BulkDelete(cates.ToList());
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CateNews>> GetAllCateNews()
        {
            try
            {
                var cates = await _unitOfWork.CateNewsRepository.GetAllAsync();
                return cates?.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
