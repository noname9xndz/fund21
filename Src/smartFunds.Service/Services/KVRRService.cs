using AutoMapper;
using smartFunds.Business;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smartFunds.Model.Common;
using KVRRAnswer = smartFunds.Model.Common.KVRRAnswer;
using KVRRQuestion = smartFunds.Model.Common.KVRRQuestion;

namespace smartFunds.Service.Services
{
    public interface IKVRRService
    {
        IEnumerable<KVRRQuestion> GetKVRR();
        bool SaveKvrrQuestion(KVRRQuestion question);
        bool DeleteAnswer(int id);
        bool DeleteQuestion(int id);
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
        public IEnumerable<KVRRQuestion> GetKVRR()
        {
            var questions = _kvrrManager.GetKVRR();
            return questions.Select(x => PopulateKVRR(x)).ToList();
        }

        public bool SaveKvrrQuestion(KVRRQuestion question)
        {
            if (question.EntityState == (int) Common.FormState.Add)
            {
                return _kvrrManager.AddKvrrQuestion(question).Result;
            }
            else if(question.EntityState == (int)Common.FormState.Edit)
            {
                return _kvrrManager.UpdateKvrrQuestion(question).Result;
            }
            return false;
        }

        public bool DeleteAnswer(int id)
        {
            return _kvrrManager.DeleteAnswer(id).Result;
        }

        public bool DeleteQuestion(int id)
        {
            return _kvrrManager.DeleteQuestion(id).Result;
        }

        #region Private method
        private Model.Common.KVRRQuestion PopulateKVRR(Data.Models.KVRRQuestion question)
        {
            if (question == null) return null;
            return new Model.Common.KVRRQuestion
            {
                Id = question.Id,
                Content = question.Content,
                Answers = PopulateKVRRAnswer(question.KVRRAnswers),
                No = question.No
            };
        }

        private List<Model.Common.KVRRAnswer> PopulateKVRRAnswer(ICollection<Data.Models.KVRRAnswer> answers)
        {
            return answers?.Select(x => new Model.Common.KVRRAnswer
            {
                Id = x.Id,
                Content = x.Content,
                Mark = x.Mark
            }).ToList();
        } 
        #endregion
    }
}
