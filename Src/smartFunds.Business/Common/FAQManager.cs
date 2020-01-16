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
    public interface IFAQManager
    {
        Task<FAQ> GetFAQ(int? faqId);
        Task<List<FAQ>> GetFAQs();
        Task<FAQ> SaveFAQ(FAQ faq);
        Task UpdateFAQ(FAQ faq);
        Task DeleteFAQ(int? faqId);

        Task DeleteFAQs(int[] faqIds);
        Task<List<FAQ>> GetAllFAQs();
        Task<List<FAQ>> GetFAQsByCategory(FAQCategory category);
        Task<List<FAQ>> GetFAQsByCategory(FAQCategory category, string searchValue);
        Task<bool> ImportFAQs(IFormFile file);
        Task<byte[]> ExportFAQs();
        byte[] ExportExampleFile();
        FAQCategory IsMapData(string text);
    }
    public class FAQManager : IFAQManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManager _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FAQManager(IUnitOfWork unitOfWork, IUserManager userManager, IHostingEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<FAQ> GetFAQ(int? faqId)
        {
            if (faqId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var faq = await _unitOfWork.FAQRepository.GetAsync(m => m.Id == faqId);
                if (faq != null)
                {
                    return faq;
                }
                throw new NotFoundException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<FAQ>> GetFAQsByCategory(FAQCategory category)
        {
            try
            {
                IQueryable<FAQ> iListFaqs;
                List<FAQ> listFaqs = new List<FAQ>();
                if (category == FAQCategory.All)
                {
                    iListFaqs = await _unitOfWork.FAQRepository.GetAllAsync();
                }
                else
                {
                    iListFaqs = await _unitOfWork.FAQRepository.FindByAsync(x => x.Category == category);
                }
                listFaqs = iListFaqs.ToList();
                return listFaqs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FAQ>> GetFAQsByCategory(FAQCategory category, string searchValue)
        {
            try
            {
                IQueryable<FAQ> faqs;
                if(category == FAQCategory.All)
                {
                    if (string.IsNullOrEmpty(searchValue))
                        faqs = await _unitOfWork.FAQRepository.GetAllAsync();
                    else
                        faqs = await _unitOfWork.FAQRepository.FindByAsync(x => x.Title.ToLower().Contains(searchValue.ToLower()));
                }
                else
                {
                    if (string.IsNullOrEmpty(searchValue))
                        faqs = await _unitOfWork.FAQRepository.FindByAsync(x => x.Category == category);
                    else
                        faqs = await _unitOfWork.FAQRepository.FindByAsync(x => x.Category == category && (x.Title.ToLower().Contains(searchValue.ToLower())));
                }

                return faqs?.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FAQ>> GetFAQs()
        {
            
            try
            {
                var allFaqs = await _unitOfWork.FAQRepository.GetAllAsync();
                List<FAQ> faqs = allFaqs.ToList();
                return faqs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FAQ> SaveFAQ(FAQ faq)
        {
            try
            {
                faq.DateLastUpdated = DateTime.Now;
                faq.LastUpdatedBy = _userManager.CurrentUser();
                faq.Title = faq.Title.Replace("\r\n", "<br/>");
                faq.Content = HttpUtility.HtmlDecode(faq.Content);
                var savedFAQ = _unitOfWork.FAQRepository.Add(faq);
                await _unitOfWork.SaveChangesAsync();
                return savedFAQ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateFAQ(FAQ faq)
        {
            try
            {
                faq.DateLastUpdated = DateTime.Now;
                faq.LastUpdatedBy = _userManager.CurrentUser();
                faq.Title = faq.Title.Replace("\r\n", "<br/>");
                faq.Content = HttpUtility.HtmlDecode(faq.Content);
                _unitOfWork.FAQRepository.Update(faq);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteFAQ(int? faqId)
        {
            if (faqId == null)
            {
                throw new InvalidParameterException();
            }
            try
            {
                var faq = _unitOfWork.FAQRepository.GetAsync(m => m.Id == faqId).Result;
                if (faq == null)
                {
                    throw new NotFoundException();
                }
                _unitOfWork.FAQRepository.Delete(faq);
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteFAQs(int[] faqIds)
        {
            if (faqIds == null || !faqIds.Any())
            {
                throw new InvalidParameterException();
            }
            try
            {
                var faqs = await _unitOfWork.FAQRepository.FindByAsync(i => faqIds.Contains(i.Id));
                if (faqs != null && faqs.Any())
                {
                    _unitOfWork.FAQRepository.BulkDelete(faqs.ToList());
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FAQ>> GetAllFAQs()
        {
            try
            {
                var faqs = await _unitOfWork.FAQRepository.GetAllAsync();
                return faqs?.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FAQ IsQuestionExisted(string comparedContent)
        {
            var checkQuestion = _unitOfWork.FAQRepository.GetAsync(x => x.Title == comparedContent);
            if (checkQuestion.Result != null)
                return checkQuestion.Result;
            return null;
        }
        public async Task<bool> ImportFAQs(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                throw new FileLoadException();
            }
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new FormatException();
            }

            var list = new List<FAQ>();


            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        string dataCategory = "";
                        dataCategory = worksheet.Cells[row, 4].Value?.ToString().Trim();
                        FAQCategory category = new FAQCategory();
                        category = IsMapData(dataCategory);
                        //Preserving new line string
                        string dataContent = worksheet.Cells[row, 3].Value?.ToString();
                        var title = worksheet.Cells[row, 2].Value?.ToString().Trim().Replace("\n", "<br/>");
                        string content = dataContent.ToString().Trim().Replace("\n", "<br/>");
                        try
                        {
                            FAQ oldQuestion = IsQuestionExisted(title);
                            //check if question is not existed
                            if (oldQuestion != null)
                            {
                                oldQuestion.Title = title;
                                oldQuestion.Content = content;
                                oldQuestion.Category = category;
                                await UpdateFAQ(oldQuestion);
                            }
                            else
                            {
                                list.Add(new FAQ
                                {
                                    Title = title,
                                    Content = content,
                                    DateLastUpdated = DateTime.Now,
                                    LastUpdatedBy = _userManager.CurrentUser(),
                                    Category = category,
                                });
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            
            _unitOfWork.FAQRepository.BulkInsert(list);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public FAQCategory IsMapData(string text)
        {
            FAQCategory category = new FAQCategory();

            if (string.Equals(text, FAQCategory.All.GetDisplayName()))
                category = (FAQCategory)0;
            else if (string.Equals(text, FAQCategory.StartWithSaveNow.GetDisplayName()))
                category = (FAQCategory)1;
            else if (string.Equals(text, FAQCategory.KVRR.GetDisplayName()))
                category = (FAQCategory)2;
            else if (string.Equals(text, FAQCategory.PortfolioFund.GetDisplayName()))
                category = (FAQCategory)3;
            else if (string.Equals(text, FAQCategory.InvestAndWithdraw.GetDisplayName()))
                category = (FAQCategory)4;
            else category = (FAQCategory)404;
            return category;
        }

        public async Task<byte[]> ExportFAQs()
        {
            try
            {
                var comlumHeadrs = new string[]
                {
                    Model.Resources.Common.STT,
                    Model.Resources.Common.QuestionContent,
                    Model.Resources.Common.AnswerContent,
                    Model.Resources.Common.Category
                };

                var predicate = PredicateBuilder.New<FAQ>(true);

                var listFAQ = await _unitOfWork.FAQRepository.GetAllAsync();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("FAQ");
                    worksheet.Column(1).BestFit = true;
                    worksheet.Column(2).Width = 50;
                    worksheet.Column(3).Width = 50;
                    double minimumSize = 10;
                    double maximumSize = 50;
                    
                    using (var cells = worksheet.Cells[1, 1, 1, 4])
                    {
                        cells.Style.Font.Bold = true;
                        cells.AutoFitColumns(minimumSize, maximumSize);
                        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    for (var i = 0; i < comlumHeadrs.Count(); i++)
                    {
                        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                    }
                    //STT
                    int index = 1;
                    var j = 2;
                    foreach (var FAQ in listFAQ)
                    {
                        string data = "";
                        foreach (FAQCategory cate in Enum.GetValues(typeof(FAQCategory)))
                        {
                            if (FAQ.Category == cate)
                            {
                                data = cate.GetDisplayName();
                            }
                        }
                        worksheet.Cells["A" + j].Value = index;
                        worksheet.Cells["B" + j].Value = FAQ.Title;
                        worksheet.Cells["C" + j].Value = Regex.Replace(FAQ.Content, @"<[^>]*>", String.Empty);
                        worksheet.Cells["D" + j].Value = data;
                        worksheet.Cells["A" + j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        j++;
                        index++;
                    }
                    
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export FAQ: " + ex.Message);
            }
        }

        public byte[] ExportExampleFile()
        {
            try
            {
                var comlumHeadrs = new string[]
                {
                    Model.Resources.Common.STT,
                    Model.Resources.Common.QuestionContent,
                    Model.Resources.Common.AnswerContent,
                    Model.Resources.Common.Category
                };

                var predicate = PredicateBuilder.New<FAQ>(true);

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("FAQ");
                    worksheet.Column(1).BestFit = true;

                    using (var cells = worksheet.Cells[1, 1, 1, 4])
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
                    worksheet.Cells["B" + j].Value = "Cau hoi 1";
                    worksheet.Cells["C" + j].Value = "Câu trả lời 1";
                    worksheet.Cells["D" + j].Value = FAQCategory.InvestAndWithdraw.GetDisplayName();
                    
                    worksheet.Cells["A" + j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                    return package.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Export FAQ: " + ex.Message);
            }
        }
    }
}
