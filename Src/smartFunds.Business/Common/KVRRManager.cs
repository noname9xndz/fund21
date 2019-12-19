using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smartFunds.Data.Models;

namespace smartFunds.Business
{
    public interface IKVRRManager
    {
        IEnumerable<KVRRQuestion> GetKVRR();
        Task<bool> AddKvrrQuestion(Model.Common.KVRRQuestion question);
        Task<bool> UpdateKvrrQuestion(Model.Common.KVRRQuestion question);
        Task<bool> DeleteAnswer(int id);
        Task<bool> DeleteQuestion(int id);
    }
    public class KVRRManager : IKVRRManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public KVRRManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<KVRRQuestion> GetKVRR()
        {
            return _unitOfWork.KVRRQuestionRepository.GetDefineKVRRQuestion();
        }

        public async Task<bool> AddKvrrQuestion(Model.Common.KVRRQuestion question)
        {
            try
            {
                if(question == null)
                    return false;
                var questionDto = new KVRRQuestion
                {
                    Id = question.Id,
                    Content = question.Content,
                    No = question.No
                };
                var addQuestion = _unitOfWork.KVRRQuestionRepository.Add(questionDto);
                if (addQuestion != null)
                {
                    var answerDto = question.Answers?.Select(x => new Data.Models.KVRRAnswer
                    {
                        Id = x.Id,
                        Content = x.Content,
                        Mark = x.Mark
                    }).ToList();

                    if (answerDto != null)
                    {
                        foreach (var answer in answerDto)
                        {
                            _unitOfWork.KVRRAnswerRepository.Add(answer);
                        }
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        public async Task<bool> UpdateKvrrQuestion(Model.Common.KVRRQuestion question)
        {
            try
            {
                if (question == null)
                    return false;
                var questionDto = new KVRRQuestion
                {
                    Id = question.Id,
                    Content = question.Content,
                    No = question.No,
                    KVRRAnswers = question.Answers?.Select(x => new Data.Models.KVRRAnswer
                    {
                        Id = x.Id,
                        Content = x.Content,
                        Mark = x.Mark
                    }).ToList()
                };
                _unitOfWork.KVRRQuestionRepository.Update(questionDto);
                _unitOfWork.KVRRAnswerRepository.BulkUpdate(questionDto.KVRRAnswers);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public async Task<bool> DeleteAnswer(int id)
        {
            try
            {
                var answer = _unitOfWork.KVRRAnswerRepository.GetAsync(a => a.Id == id);
                _unitOfWork.KVRRAnswerRepository.Delete(answer.Result);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteQuestion(int id)
        {
            try
            {
                var answers = _unitOfWork.KVRRAnswerRepository.GetAnswersByQuestionId(id);
                if (answers != null)
                    _unitOfWork.KVRRAnswerRepository.BulkDelete(answers.ToList());

                var question = _unitOfWork.KVRRQuestionRepository.GetAsync(q => q.Id == id);
                if(question != null)
                    _unitOfWork.KVRRQuestionRepository.Delete(question.Result);
                
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
