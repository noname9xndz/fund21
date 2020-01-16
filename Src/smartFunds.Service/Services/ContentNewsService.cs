using AutoMapper;
using smartFunds.Business.Common;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using smartFunds.Common;
using Microsoft.AspNetCore.Http;

namespace smartFunds.Service.Services
{
    public interface IContentNewsService
    {
        Task<ContentNewsModel> GetContentNews(int? newsId);
        Task<ContentNewsModel> SaveContentNews(ContentNewsModel newsModel);
        Task UpdateContentNews(ContentNewsModel newsModel);
        Task DeleteContentNews(int? newsId);
        Task DeleteListContentNews(int[] newsIds);
        Task<List<ContentNewsModel>> GetListNewsByCategory(int category);
        Task<LstNewsModel> GetListContentNewsPaging(int pageSize, int pageIndex, int status);
        Task<LstNewsModel> GetListContentNewsOtherOld(int pageSize, int pageIndex, int status, DateTime dt);
        Task<LstNewsModel> GetListContentNewsOtherNew(int pageSize, int pageIndex, int status, DateTime dt);
        Task<LstNewsModel> GetListContentNewsisHome(int pageSize, int pageIndex);
        Task<List<ContentNewsModel>> GetListNewsByCategory(int category, string searchValue);
        Task<List<ContentNewsModel>> GetListContentNews();
        Task<List<ContentNewsModel>> GetAllContentNews();
        Task<List<CateNewsModel>> GetAllCateNews();
    }
    public  class ContentNewsService : IContentNewsService
    {
        private readonly IMapper _mapper;
        private readonly IContentNewsManager _newsManager;
        public ContentNewsService(IMapper mapper, IContentNewsManager newsManager)
        {
            _mapper = mapper;
            _newsManager = newsManager;
        }

        public async Task<LstNewsModel> GetListContentNewsPaging(int pageSize, int pageIndex, int status)
        {
            var lstNews = new LstNewsModel();
            var listTransactionHistory = await _newsManager.GetListContentNewsPaging(pageSize, pageIndex, status);
            var allTransactionHistory = await _newsManager.GetTotalContentNews(status);
            lstNews.LstNews = _mapper.Map<List<ContentNews>, List<ContentNewsModel>>(listTransactionHistory);
            lstNews.TotalCount = allTransactionHistory;
            return lstNews;
        }
        public async Task<LstNewsModel> GetListContentNewsOtherOld(int pageSize, int pageIndex, int status, DateTime dt)
        {
            var lstNews = new LstNewsModel();
            var listTransactionHistory = await _newsManager.GetListContentNewsOtherOld(pageSize, pageIndex, status, dt);
           // var allTransactionHistory = await _newsManager.GetTotalContentNews(status);
            lstNews.LstNews = _mapper.Map<List<ContentNews>, List<ContentNewsModel>>(listTransactionHistory);
           // lstNews.TotalCount = allTransactionHistory;
            return lstNews;
        }
        public async Task<LstNewsModel> GetListContentNewsOtherNew(int pageSize, int pageIndex, int status, DateTime dt)
        {
            var lstNews = new LstNewsModel();
            var listTransactionHistory = await _newsManager.GetListContentNewsOtherNew(pageSize, pageIndex, status, dt);
            // var allTransactionHistory = await _newsManager.GetTotalContentNews(status);
            lstNews.LstNews = _mapper.Map<List<ContentNews>, List<ContentNewsModel>>(listTransactionHistory);
            // lstNews.TotalCount = allTransactionHistory;
            return lstNews;
        }
        public async Task<LstNewsModel> GetListContentNewsisHome(int pageSize, int pageIndex)
        {
            var lstNews = new LstNewsModel();
            var listTransactionHistory = await _newsManager.GetListContentNewsisHome(pageSize, pageIndex);
            // var allTransactionHistory = await _newsManager.GetTotalContentNews(status);
            lstNews.LstNews = _mapper.Map<List<ContentNews>, List<ContentNewsModel>>(listTransactionHistory);
            // lstNews.TotalCount = allTransactionHistory;
            return lstNews;
        }
        
        public async Task<ContentNewsModel> GetContentNews(int? newsId)
        {
            try
            {
                ContentNews news = await _newsManager.GetContentNews(newsId);
                ContentNewsModel newsModel = _mapper.Map<ContentNewsModel>(news);
                return newsModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ContentNewsModel>> GetListNewsByCategory(int category)
        {
            try
            {
                List<ContentNews> lstnews = await _newsManager.GetListNewsByCategory(category);
                List<ContentNewsModel> lstNewsModels = _mapper.Map<List<ContentNews>, List<ContentNewsModel>>(lstnews);
                return lstNewsModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ContentNewsModel>> GetListNewsByCategory(int category, string searchValue)
        {
            try
            {
                var lstnews = await _newsManager.GetListNewsByCategory(category, searchValue);
                List<ContentNewsModel> lstModels = _mapper.Map<List<ContentNewsModel>>(lstnews);
                return lstModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ContentNewsModel> SaveContentNews(ContentNewsModel newsModel)
        {
            try
            {
                ContentNews news = _mapper.Map<ContentNews>(newsModel);
                ContentNews savedNews = await _newsManager.SaveContentNews(news);
                ContentNewsModel savedNewsModel = _mapper.Map<ContentNewsModel>(savedNews);
                return savedNewsModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateContentNews(ContentNewsModel newsModel)
        {
            try
            {
               ContentNews news = _mapper.Map<ContentNews>(newsModel);
                await _newsManager.UpdateContentNews(news);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteContentNews(int? newsId)
        {
            try
            {
                await _newsManager.DeleteContentNews(newsId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteListContentNews(int[] newsIds)
        {
            try
            {
                await _newsManager.DeleteListContentNews(newsIds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ContentNewsModel>> GetListContentNews()
        {
            try
            {
                List<ContentNews> lstNews = await _newsManager.GetListContentNews();
                List<ContentNewsModel> lstNewsModels = _mapper.Map<List<ContentNews>, List<ContentNewsModel>>(lstNews);
                return lstNewsModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ContentNewsModel>> GetAllContentNews()
        {
            try
            {
                var news = await _newsManager.GetAllContentNews();
                List<ContentNewsModel> lstNewsModels = _mapper.Map<List<ContentNewsModel>>(news);
                return lstNewsModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<CateNewsModel>> GetAllCateNews()
        {
            try
            {
                var news = await _newsManager.GetAllCateNews();
                List<CateNewsModel> lstNewsModels = _mapper.Map<List<CateNewsModel>>(news);
                return lstNewsModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
