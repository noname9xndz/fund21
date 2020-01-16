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
    public interface ICateNewsService
    {
        Task<CateNewsModel> GetCateNews(int? cateId);
        Task<CateNewsModel> SaveCateNews(CateNewsModel cateModel);
        Task UpdateCateNews(CateNewsModel cateModel);
        Task DeleteCateNews(int? cateId);
        Task DeleteListCateNews(int[] cateIds);
       // Task<List<CateNewsModel>> GetFAQsByCategory(FAQCategory category);
      //  Task<List<CateNewsModel>> GetFAQsByCategory(FAQCategory category, string searchValue);
        Task<List<CateNewsModel>> GetListCateNews();
        Task<List<CateNewsModel>> GetAllCateNews();
       
     //   FAQCategory IsMapData(string text);
    }
    public class CateNewsService : ICateNewsService
    {
        private readonly IMapper _mapper;
        private readonly ICateNewsManager _cateManager;
        public CateNewsService(IMapper mapper, ICateNewsManager cateManager)
        {
            _mapper = mapper;
            _cateManager = cateManager;
        }

        public async Task<CateNewsModel> GetCateNews(int? cateId)
        {
            try
            {
                CateNews cate = await _cateManager.GetCateNews(cateId);
                CateNewsModel cateModel = _mapper.Map<CateNewsModel>(cate);
                return cateModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        public async Task<CateNewsModel> SaveCateNews(CateNewsModel cateModel)
        {
            try
            {
                CateNews cate = _mapper.Map<CateNews>(cateModel);
                CateNews savedcate = await _cateManager.SaveCateNews(cate);
                CateNewsModel savedCateModel = _mapper.Map<CateNewsModel>(savedcate);
                return savedCateModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateCateNews(CateNewsModel cateModel)
        {
            try
            {
                CateNews cate = _mapper.Map<CateNews>(cateModel);
                await _cateManager.UpdateCateNews(cate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteCateNews(int? faqId)
        {
            try
            {
                await _cateManager.DeleteCateNews(faqId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteListCateNews(int[] faqIds)
        {
            try
            {
                await _cateManager.DeleteListCateNews(faqIds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CateNewsModel>> GetListCateNews()
        {
            try
            {
                List<CateNews> cates = await _cateManager.GetAllCateNews();
                List<CateNewsModel> faqsModels = _mapper.Map<List<CateNews>, List<CateNewsModel>>(cates);
                return faqsModels;
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
                var faqs = await _cateManager.GetAllCateNews();
                List<CateNewsModel> faqsModels = _mapper.Map<List<CateNewsModel>>(faqs);
                return faqsModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
    }
