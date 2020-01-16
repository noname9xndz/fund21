using AutoMapper;
using smartFunds.Business;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using smartFunds.Common.Exceptions;
using smartFunds.Model.Common;
using KVRRAnswer = smartFunds.Model.Common.KVRRAnswer;
using KVRRQuestion = smartFunds.Model.Common.KVRRQuestion;
using Microsoft.AspNetCore.Http;
using smartFunds.Common;

namespace smartFunds.Service.Services
{
    public interface IKVRRQuestionAnswerService
    {
        Task<List<KVRRQuestion>> GetKVRRQuestions();
        Task<KVRRQuestion> GetKVRRQuestionById(int id);
        Task<KVRRQuestion> GetKVRRQuestionNoAnswerById(int id);
        Task<KVRRQuestion> GetKVRRQuestionByNo(int no);
        Task Save(KVRRQuestion question);
        Task Update(KVRRQuestion question);
        Task UpdateOnlyQuestion(KVRRQuestion question);
        Task UpdateQuestionOrder(KVRRQuestion question);
        Task DeleteAnswer(List<int> ids);
        Task DeleteQuestion(int id);
        Task<bool> ImportListQuestions(IFormFile file);
        KVRRQuestionCategories CheckNeededFields(IFormFile file);
        byte[] ExportExampleKVRRs();
        bool IsQuestionExisted(string comparedContent);


    }
    public class KVRRQuestionAnswerService : IKVRRQuestionAnswerService
    {
        private readonly IMapper _mapper;
        private readonly IKVRRQuestionAnswerManager _kvrrQAManager;
        public KVRRQuestionAnswerService(IMapper mapper, IKVRRQuestionAnswerManager kvrrQAManager)
        {
            _mapper = mapper;
            _kvrrQAManager = kvrrQAManager;
        }
        public async Task<List<KVRRQuestion>> GetKVRRQuestions()
        {
            var questions = await _kvrrQAManager.GetKVRRQuestions();
            if(questions == null) throw new NotFoundException();
            return _mapper.Map<List<KVRRQuestion>>(questions);
        }
        public async Task<KVRRQuestion> GetKVRRQuestionById(int id)
        {
            var question = await _kvrrQAManager.GetKVRRQuestionById(id);
            if (question == null) throw new NotFoundException();
            return _mapper.Map<KVRRQuestion>(question);
        }

        public async Task<KVRRQuestion> GetKVRRQuestionNoAnswerById(int id)
        {
            var question = await _kvrrQAManager.GetKVRRQuestionNoAnswerById(id);
            if (question == null) throw new NotFoundException();
            return _mapper.Map<KVRRQuestion>(question);
        }

        public async Task<KVRRQuestion> GetKVRRQuestionByNo(int no)
        {
            var question = await _kvrrQAManager.GetKVRRQuestionByNo(no);
            if (question == null) throw new NotFoundException();
            return _mapper.Map<KVRRQuestion>(question);
        }

        public async Task Save(KVRRQuestion question)
        {
            try
            {
                var model = _mapper.Map<Data.Models.KVRRQuestion>(question);
                await _kvrrQAManager.Save(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(KVRRQuestion question)
        {
            try
            {
                var model = _mapper.Map<Data.Models.KVRRQuestion>(question);
                await _kvrrQAManager.Update(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateOnlyQuestion(KVRRQuestion question)
        {
            try
            {
                var model = _mapper.Map<Data.Models.KVRRQuestion>(question);
                await _kvrrQAManager.UpdateOnlyQuestion(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateQuestionOrder(KVRRQuestion question)
        {
            try
            {
                var model = _mapper.Map<Data.Models.KVRRQuestion>(question);
                await _kvrrQAManager.UpdateQuestionOrder(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAnswer(List<int> ids)
        {
            try
            {
                await _kvrrQAManager.DeleteAnswer(ids);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteQuestion(int id)
        {
            try
            {
                await _kvrrQAManager.DeleteQuestion(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ImportListQuestions(IFormFile file)
        {
            try
            {
                var result = await _kvrrQAManager.ImportListQuestions(file);
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public KVRRQuestionCategories CheckNeededFields(IFormFile file)
        {
            return  _kvrrQAManager.CheckNeededFields(file);
        }

        public byte[] ExportExampleKVRRs()
        {
            try
            {
                return _kvrrQAManager.ExportExampleKVRRs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsQuestionExisted(string comparedContent)
        {
            try
            {
                if (_kvrrQAManager.IsQuestionExisted(comparedContent) != null)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
