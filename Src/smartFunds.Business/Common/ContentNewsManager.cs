using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
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

namespace smartFunds.Business.Common
{
    public interface IContentNewsManager
    {
        Task<ContentNews> GetContentNews(int? NewsId);
        Task<List<ContentNews>> GetListContentNews();
        Task<List<ContentNews>> GetListContentNewsPaging(int pageSize, int pageIndex, int status);
        Task<List<ContentNews>> GetListContentNewsOtherOld(int pageSize, int pageIndex, int status,DateTime dt);
        Task<List<ContentNews>> GetListContentNewsOtherNew(int pageSize, int pageIndex, int status, DateTime dt);
        Task<List<ContentNews>> GetListContentNewsisHome(int pageSize, int pageIndex);
        Task<int> GetTotalContentNews(int status);
        Task<ContentNews> SaveContentNews(ContentNews news);
        Task UpdateContentNews(ContentNews news);
        Task DeleteContentNews(int? newsId);

        Task DeleteListContentNews(int[] newsIds);
        Task<List<ContentNews>> GetAllContentNews();
        Task<List<ContentNews>> GetListNewsByCategory(int cate);
        Task<List<ContentNews>> GetListNewsByCategory(int cate ,string searchValue);
        Task<List<CateNews>> GetAllCateNews();
        //  CateNews IsMapData(string text);
    }
    public  class ContentNewsManager : IContentNewsManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ContentNewsManager(IUnitOfWork unitOfWork, IUserManager userManager, IHostingEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<List<ContentNews>> GetListContentNewsPaging(int pageSize, int pageIndex,int status)
        {
            if (pageSize < 1 || pageIndex < 1)
            {
                throw new InvalidParameterException();
            }
            try
            {
              

                var lstnews = new List<ContentNews>();
                if(status==1)
                {
                    lstnews = (await _unitOfWork.ContentNewsRepository.GetAllAsync()).Where(i => i.Status ==true)
                                    .OrderByDescending(i => i.PostDate)
                                    .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                }
                else
                {
                    lstnews = (await _unitOfWork.ContentNewsRepository.GetAllAsync())
                                   .OrderByDescending(i => i.LastUpdatedBy)
                                   .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                return lstnews;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ContentNews>> GetListContentNewsisHome(int pageSize, int pageIndex)
        {
            if (pageSize < 1 || pageIndex < 1)
            {
                throw new InvalidParameterException();
            }
            try
            {


                var lstnews = new List<ContentNews>();
               
                    lstnews = (await _unitOfWork.ContentNewsRepository.GetAllAsync()).Where(i => i.Status == true&&i.Ishome==true)
                                    .OrderByDescending(i => i.PostDate)
                                    .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                
                return lstnews;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ContentNews>> GetListContentNewsOtherOld(int pageSize, int pageIndex, int newsId,DateTime dt)
        {
            if (pageSize < 1 || pageIndex < 1)
            {
                throw new InvalidParameterException();
            }
            try
            {


                var lstnews = new List<ContentNews>();
                if (newsId >0)
                {
                    if(dt!=null)
                    lstnews = (await _unitOfWork.ContentNewsRepository.GetAllAsync()).Where(i => i.Status == true && i.PostDate <= dt&&i.Id!= newsId)
                                    .OrderByDescending(i => i.PostDate)
                                    .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    else
                    {

                        lstnews = (await _unitOfWork.ContentNewsRepository.GetAllAsync()).Where(i => i.Status == true  && i.Id != newsId)
                                   .OrderByDescending(i => i.PostDate)
                                   .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    }

                }
                else
                {
                    if (dt != null)
                        lstnews = (await _unitOfWork.ContentNewsRepository.GetAllAsync()).Where(i => i.Status == true && i.PostDate <= dt )
                                   .OrderByDescending(i => i.PostDate)
                                   .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    else
                         if (dt != null)
                        lstnews = (await _unitOfWork.ContentNewsRepository.GetAllAsync()).Where(i => i.Status == true)
                                   .OrderByDescending(i => i.PostDate)
                                   .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                return lstnews;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ContentNews>> GetListContentNewsOtherNew(int pageSize, int pageIndex, int newsId, DateTime dt)
        {
            if (pageSize < 1 || pageIndex < 1)
            {
                throw new InvalidParameterException();
            }
            try
            {


                var lstnews = new List<ContentNews>();
                if (newsId > 0)
                {
                    if (dt != null)
                        lstnews = (await _unitOfWork.ContentNewsRepository.GetAllAsync()).Where(i => i.Status == true && i.PostDate >= dt && i.Id != newsId)
                                        .OrderByDescending(i => i.PostDate)
                                        .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    else
                    {

                        lstnews = (await _unitOfWork.ContentNewsRepository.GetAllAsync()).Where(i => i.Status == true && i.Id != newsId)
                                   .OrderByDescending(i => i.PostDate)
                                   .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                    }

                }
                else
                {
                    if (dt != null)
                        lstnews = (await _unitOfWork.ContentNewsRepository.GetAllAsync()).Where(i => i.Status == true && i.PostDate >= dt)
                                   .OrderByDescending(i => i.PostDate)
                                   .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    else
                         if (dt != null)
                        lstnews = (await _unitOfWork.ContentNewsRepository.GetAllAsync()).Where(i => i.Status == true)
                                   .OrderByDescending(i => i.PostDate)
                                   .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                return lstnews;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> GetTotalContentNews(int status)
        {
            try
            {
              

                var lstNews = new List<ContentNews>();

                if (status==1)
                {
                    return (await _unitOfWork.ContentNewsRepository.FindByAsync(i => i.Status==true))
                                    .OrderByDescending(i => i.PostDate)
                                    .Count();
                }
                else
                {
                    return (await _unitOfWork.ContentNewsRepository.GetAllAsync())
                                  .OrderByDescending(i => i.LastUpdatedBy)
                                    .Count();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<ContentNews> UploadImage(ContentNews news)
        {
            if (news.NewsImage.Length > 0)
            {
              //  var fileName = news.NewsImage.FileName.Split('\\').Last();
              //  var filePath = $"{_hostingEnvironment.WebRootPath}{smartFunds.Common.Constants.NewsImageFolder.Path}{fileName}";


                var fileName = Guid.NewGuid() + Path.GetExtension(news.NewsImage.FileName).ToLower();



                var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot/images/news",
                    fileName);

    
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await news.NewsImage.CopyToAsync(stream);
                }
                news.ImageThumb = fileName;
            }

            return news;
        }
        public async Task<ContentNews> GetContentNews(int? newsId)
        {
            if (newsId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var faq = await _unitOfWork.ContentNewsRepository.GetAsync(m => m.Id == newsId);
                if (faq != null)
                {
                   
                    faq.Contents = HttpUtility.HtmlDecode(faq.Contents);
                    return faq;
                }
                throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ContentNews>> GetListNewsByCategory(int category)
        {
            try
            {
                IQueryable< ContentNews> iListFaqs;
                List<ContentNews> listFaqs = new List<ContentNews>();
                if (category ==0)
                {
                    iListFaqs = await _unitOfWork.ContentNewsRepository.GetAllAsync();
                }
                else
                {
                    iListFaqs = await _unitOfWork.ContentNewsRepository.FindByAsync(x => x.CateNewsID == category);
                }
                listFaqs = iListFaqs.ToList();
                return listFaqs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ContentNews>> GetListNewsByCategory(int category, string searchValue)
        {
            try
            {

                IQueryable<ContentNews> iListFaqs;
                List<ContentNews> listFaqs = new List<ContentNews>();
                if (category == 0)
                {
                    if (string.IsNullOrEmpty(searchValue))
                        iListFaqs = await _unitOfWork.ContentNewsRepository.GetAllAsync();
                    else
                        iListFaqs = await _unitOfWork.ContentNewsRepository.FindByAsync(x => x.Title.ToLower().Contains(searchValue.ToLower()));
                }
                else
                {
                    if (string.IsNullOrEmpty(searchValue))
                        iListFaqs = await _unitOfWork.ContentNewsRepository.FindByAsync(x => x.CateNewsID == category);
                    else
                        iListFaqs = await _unitOfWork.ContentNewsRepository.FindByAsync(x => x.CateNewsID == category && (x.Title.ToLower().Contains(searchValue.ToLower())));
                }

                return iListFaqs?.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ContentNews>> GetListContentNews()
        {

            try
            {
                IQueryable<ContentNews> allFaqs = await _unitOfWork.ContentNewsRepository.GetAllAsync();
                List<ContentNews> faqs = allFaqs.ToList();
                return faqs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ContentNews> SaveContentNews(ContentNews news)
        {
            try
            {
                news.DateLastUpdated = DateTime.Now;
                news.LastUpdatedBy = _userManager.CurrentUser();
                news.Title = news.Title.Replace("\r\n", "<br/>");
                news.Contents = HttpUtility.HtmlEncode(news.Contents);
                 if (news.NewsImage?.Length > 0)
                {
                    news = UploadImage(news).Result;
                }
                var savedFAQ = _unitOfWork.ContentNewsRepository.Add(news);
                await _unitOfWork.SaveChangesAsync();
                return savedFAQ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UpdateContentNews(ContentNews news)
        {
            try
            {
                news.DateLastUpdated = DateTime.Now;
                news.LastUpdatedBy = _userManager.CurrentUser();
                news.Title = news.Title.Replace("\r\n", "<br/>");
                news.Contents = HttpUtility.HtmlDecode(news.Contents);
                if (news.NewsImage?.Length > 0)
                {
                    news = UploadImage(news).Result;
                }
                _unitOfWork.ContentNewsRepository.Update(news);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteContentNews(int? newsId)
        {
            if (newsId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var news = _unitOfWork.ContentNewsRepository.GetAsync(m => m.Id == newsId).Result;
                if (news == null)
                {
                    throw new NotFoundException();
                }
                _unitOfWork.ContentNewsRepository.Delete(news);
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task DeleteListContentNews(int[] newsIds)
        {
            if (newsIds == null || !newsIds.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                var lstnews = await _unitOfWork.ContentNewsRepository.FindByAsync(i => newsIds.Contains(i.Id));
                if (lstnews != null && lstnews.Any())
                {
                    _unitOfWork.ContentNewsRepository.BulkDelete(lstnews.ToList());
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ContentNews>> GetAllContentNews()
        {
            try
            {
                var lstNews = await _unitOfWork.ContentNewsRepository.GetAllAsync();
                return lstNews?.ToList();
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
                var lstNews = await _unitOfWork.CateNewsRepository.GetAllAsync();
                return lstNews?.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
