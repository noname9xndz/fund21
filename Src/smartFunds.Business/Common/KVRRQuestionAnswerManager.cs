using smartFunds.Data.UnitOfWork;
using smartFunds.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using static smartFunds.Common.Constants;
using System.Text.RegularExpressions;
using LinqKit;
using OfficeOpenXml.Style;

namespace smartFunds.Business
{
    public interface IKVRRQuestionAnswerManager
    {
        Task<IEnumerable<KVRRQuestion>> GetKVRRQuestions();
        Task<KVRRQuestion> GetKVRRQuestionById(int id);
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

        KVRRQuestion IsQuestionExisted(string comparedContent);
        Task<KVRRQuestion> GetKVRRQuestionNoAnswerById(int id);

    }
    public class KVRRQuestionAnswerManager : IKVRRQuestionAnswerManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public KVRRQuestionAnswerManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<KVRRQuestion>> GetKVRRQuestions()
        {
            try
            {
                return await _unitOfWork.KVRRQuestionRepository.GetAllAsync("KVRRAnswers");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KVRRQuestion> GetKVRRQuestionById(int id)
        {
            try
            {
                return await _unitOfWork.KVRRQuestionRepository.GetAsync(x => x.Id == id, "KVRRAnswers");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KVRRQuestion> GetKVRRQuestionNoAnswerById(int id)
        {
            try
            {
                return await _unitOfWork.KVRRQuestionRepository.GetAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<KVRRQuestion> GetKVRRQuestionByNo(int no)
        {
            try
            {
                return await _unitOfWork.KVRRQuestionRepository.GetAsync(x => x.No == no, "KVRRAnswers");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Save(KVRRQuestion question)
        {
            try
            {
                if (question == null) throw new InvalidParameterException();

                if (question.KVRRAnswers != null && question.KVRRAnswers.Any(i => !string.IsNullOrWhiteSpace(i.Content) && i.Mark != null))
                {
                    var listAnswers = question.KVRRAnswers.Where(i => !string.IsNullOrWhiteSpace(i.Content) && i.Mark != null);
                    if (listAnswers.Count() >= 2)
                    {
                        //update No before insert record
                        var questions = await _unitOfWork.KVRRQuestionRepository.GetAllAsync();
                        var no = (questions.Count() + 1);
                        question.No = no;
                        
                        var addQuestion = _unitOfWork.KVRRQuestionRepository.Add(question);
                        if (addQuestion != null)
                        {
                            _unitOfWork.KVRRAnswerRepository.BulkInsert(question.KVRRAnswers);
                        }
                        await _unitOfWork.SaveChangesAsync();
                    }
                }
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
                if (question == null) throw new InvalidParameterException();

                var oldQuestion = await GetKVRRQuestionNoAnswerById(question.Id);
                question.ImageDesktop = oldQuestion.ImageDesktop;
                question.ImageMobile = oldQuestion.ImageMobile;

                _unitOfWork.KVRRQuestionRepository.Update(question);

                var answerOld = await _unitOfWork.KVRRAnswerRepository.FindByAsync(x => x.KVRRQuestion.Id == question.Id);
                if (answerOld != null && answerOld.Any())
                {
                    _unitOfWork.KVRRAnswerRepository.BulkDelete(answerOld.ToList());
                }

                _unitOfWork.KVRRAnswerRepository.BulkUpdate(question.KVRRAnswers);
                await _unitOfWork.SaveChangesAsync();
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
                if (question == null) throw new InvalidParameterException();
                _unitOfWork.KVRRQuestionRepository.Update(question);

                await _unitOfWork.SaveChangesAsync();
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
                if (question == null) throw new InvalidParameterException();

                var oldQuestion = await GetKVRRQuestionNoAnswerById(question.Id);
                question.ImageDesktop = oldQuestion.ImageDesktop;
                question.ImageMobile = oldQuestion.ImageMobile;

                _unitOfWork.KVRRQuestionRepository.Update(question);
                await _unitOfWork.SaveChangesAsync();
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
                if (ids == null || !ids.Any()) throw new InvalidParameterException();
                var answers = await _unitOfWork.KVRRAnswerRepository.FindByAsync(x => ids.Contains(x.Id));
                if (answers == null || !answers.Any()) throw new NotFoundException();
                _unitOfWork.KVRRAnswerRepository.BulkDelete(answers.ToList());
                await _unitOfWork.SaveChangesAsync();
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
                var answers = await _unitOfWork.KVRRAnswerRepository.FindByAsync(x => x.KVRRQuestion.Id == id);
                if (answers != null && answers.Any())
                    _unitOfWork.KVRRAnswerRepository.BulkDelete(answers.ToList());

                var question = await _unitOfWork.KVRRQuestionRepository.GetAsync(q => q.Id == id);
                if (question == null) throw new NotFoundException();
                _unitOfWork.KVRRQuestionRepository.Delete(question);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ImportListQuestions(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);

            var listQuestions = new List<KVRRQuestion>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    var rowCount = worksheet.Dimension.Rows;
                    var questions = await _unitOfWork.KVRRQuestionRepository.GetAllAsync();
                    var no = questions.Count();
                    if (no == 0) no = 1;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var listAnswers = new List<KVRRAnswer>();
                        var categoryCell = "";
                        categoryCell = worksheet.Cells[row, 2].Value.ToString().Trim();

                        //update "No" Column in database
                        no += 1;
                        try
                        {
                            KVRRQuestion newQuestion = new KVRRQuestion();
                            newQuestion.Content = worksheet.Cells[row, 3].Value.ToString().Trim();
                            newQuestion.No = no;
                            newQuestion.KVRRQuestionCategories = (KVRRQuestionCategories)CheckCategoryExisted(categoryCell);
                            
                            for (int col = 4; col <= 22; col += 2)
                            {
                                KVRRAnswer newAnswer = new KVRRAnswer();
                                if(worksheet.Cells[row, col].Value != null)
                                {
                                    newAnswer.Content = worksheet.Cells[row, col].Value.ToString().Trim();
                                }
                                if (worksheet.Cells[row, col + 1].Value != null)
                                {
                                    newAnswer.Mark = int.Parse(worksheet.Cells[row, col + 1].Value.ToString().Trim());
                                }
                                if(newAnswer.Content != null && newAnswer.Mark != null)
                                {
                                    newAnswer.KVRRQuestion = newQuestion;
                                    listAnswers.Add(newAnswer);
                                }
                            }
                            KVRRQuestion oldQuestion = IsQuestionExisted(newQuestion.Content);
                            //check if question is not existed
                            if (oldQuestion != null)
                            {
                                listQuestions.Remove(newQuestion);

                                oldQuestion.KVRRAnswers = listAnswers;
                                oldQuestion.KVRRQuestionCategories = newQuestion.KVRRQuestionCategories;
                                await Update(oldQuestion);
                            }
                            else
                            {
                                newQuestion.KVRRAnswers = listAnswers;
                                listQuestions.Add(newQuestion);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    _unitOfWork.KVRRQuestionRepository.BulkInsert(listQuestions);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            return true;
        }

        public KVRRQuestionCategories CheckCategoryExisted(string text)
        {
            KVRRQuestionCategories category = new KVRRQuestionCategories();

            if (text == KVRRQuestionCategories.BigExpenses.GetDisplayName() || text == KVRRQuestionsCategory.BigExpenses)
                category = (KVRRQuestionCategories)0;
            else if (text == KVRRQuestionCategories.Inflationary.GetDisplayName() || text == KVRRQuestionsCategory.Inflationary)
                category = (KVRRQuestionCategories)1;
            else if (text == KVRRQuestionCategories.Investment.GetDisplayName() || text == KVRRQuestionsCategory.Investment)
                category = (KVRRQuestionCategories)2;
            else if (text == KVRRQuestionCategories.PriceFluctuations.GetDisplayName() || text == KVRRQuestionsCategory.PriceFluctuations)
                category = (KVRRQuestionCategories)3;
            else if (text == KVRRQuestionCategories.RiskVersusProfit.GetDisplayName() || text == KVRRQuestionsCategory.RiskVersusProfit)
                category = (KVRRQuestionCategories)4;
            else if (text == KVRRQuestionCategories.Discount.GetDisplayName() || text == KVRRQuestionsCategory.Discount)
                category = (KVRRQuestionCategories)5;
            else if (text == KVRRQuestionCategories.UnderstandingTheRisks.GetDisplayName() || text == KVRRQuestionsCategory.UnderstandingTheRisks)
                category = (KVRRQuestionCategories)6;
            else if (text == KVRRQuestionCategories.PersonalTime.GetDisplayName() || text == KVRRQuestionsCategory.PersonalTime)
                category = (KVRRQuestionCategories)7;
            else if (text == KVRRQuestionCategories.LongTermInvestment.GetDisplayName() || text == KVRRQuestionsCategory.LongTermInvestment)
                category = (KVRRQuestionCategories)8;
            else category = (KVRRQuestionCategories)400;
            return category;
        }

        public KVRRQuestionCategories CheckNeededFields(IFormFile file)
        {
            KVRRQuestionCategories category = new KVRRQuestionCategories();
            using (var stream = new MemoryStream())
            {
                file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        //check category
                        var categoryCell = "";
                        if (worksheet.Cells[row, 2].Value == null)
                        {
                            return (KVRRQuestionCategories)404;
                        }
                        else
                        {
                            categoryCell = worksheet.Cells[row, 2].Value.ToString().Trim();
                        }

                        //check if question content is null
                        if (worksheet.Cells[row, 3].Value == null)
                        {
                            return (KVRRQuestionCategories)401;
                        }
                        else
                        {
                            string contentQuestion = worksheet.Cells[row, 3].Value.ToString().Trim();
                        }

                        //check if question content is not existed
                        if (CheckCategoryExisted(categoryCell) == (KVRRQuestionCategories)400)
                        {
                            return (KVRRQuestionCategories)400;
                        }
                        List<KVRRAnswerMarkValidate> listAnswer = new List<KVRRAnswerMarkValidate>();
                        for (int col = 4; col <= 22; col += 2)
                        {
                            string answerContent = "";
                            string mark = "";

                            if (worksheet.Cells[row, col].Value == null)
                            {
                                if (worksheet.Cells[row, col + 1].Value != null)
                                    return (KVRRQuestionCategories)405;
                            }
                            if (worksheet.Cells[row, col + 1].Value == null)
                            {
                                if (worksheet.Cells[row, col].Value != null)
                                    return (KVRRQuestionCategories)405;
                            }

                            if (worksheet.Cells[row, col + 1].Value != null && worksheet.Cells[row, col].Value != null)
                            {
                                mark = worksheet.Cells[row, col + 1].Value.ToString().Trim();
                                if (!IsPositiveNumber(mark))
                                {
                                    return (KVRRQuestionCategories)402;
                                }
                                else
                                {
                                    answerContent = worksheet.Cells[row, col].Value.ToString().Trim();
                                    KVRRAnswerMarkValidate answer = new KVRRAnswerMarkValidate()
                                    {
                                        AnswerContent = answerContent,
                                        Mark = mark
                                    };
                                    listAnswer.Add(answer);
                                }
                            }
                        }
                        if(listAnswer.Count < 2)
                        {
                            return (KVRRQuestionCategories)403;
                        }

                    }
                }
            }
            return category;
        }

        private bool IsPositiveNumber(string input)
        {
            if (Regex.IsMatch(input, @"^\d+$") && int.Parse(input) >= 0)
                return true;
            return false;
        }
        

        public KVRRQuestion IsQuestionExisted(string comparedContent)
        {
            var checkQuestion = _unitOfWork.KVRRQuestionRepository.GetAsync(x => x.Content == comparedContent, "KVRRAnswers");
            if (checkQuestion != null)
                return checkQuestion.Result;
            return null;
        }

        public byte[] ExportExampleKVRRs()
        {
            try
            {
                var comlumHeadrs = new string[]
                {
                    Model.Resources.Common.STT,
                    Model.Resources.Common.PortfolioContent,
                    Model.Resources.Common.KVRRQuestion,
                    Model.Resources.Common.AnswerNumber + " 1",
                    Model.Resources.Common.Mark,
                    Model.Resources.Common.AnswerNumber + " 2",
                    Model.Resources.Common.Mark,
                    Model.Resources.Common.AnswerNumber + " 3",
                    Model.Resources.Common.Mark,
                    Model.Resources.Common.AnswerNumber + " 4",
                    Model.Resources.Common.Mark,
                };

                var predicate = PredicateBuilder.New<KVRRQuestion>(true);

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("FAQ");
                    worksheet.Column(1).BestFit = true;

                    using (var cells = worksheet.Cells[1, 1, 1, 11])
                    {
                        cells.Style.Font.Bold = true;
                        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    for (var i = 0; i < comlumHeadrs.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    }
                    //STT
                    var j = 2;
                    
                    worksheet.Cells["A" + j].Value = 1;
                    worksheet.Cells["B" + j].Value = KVRRQuestionsCategory.BigExpenses;
                    worksheet.Cells["C" + j].Value = "Cau hoi 1";
                    worksheet.Cells["D" + j].Value = "Dap an 1";
                    worksheet.Cells["E" + j].Value = 1;
                    worksheet.Cells["F" + j].Value = "Dap an 2";
                    worksheet.Cells["G" + j].Value = 2;
                    worksheet.Cells["A" + j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export KVRR: " + ex.Message);
            }

        }

    }
    public class KVRRAnswerMarkValidate
    {
        public string AnswerContent { get; set; }
        public string Mark { get; set; }
    }
}
