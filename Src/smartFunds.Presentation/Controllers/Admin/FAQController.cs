using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Service.Services;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers.Admin
{   
    [Route("admin/faq")]
    public class FAQController : Controller
    {
        private readonly IFAQService _faqService;
        private readonly IConfiguration _configuration;

        public FAQController(IFAQService faqService, IConfiguration configuration)
        {
            _faqService = faqService;
            _configuration = configuration;
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("detail/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {            
            FAQModel model = await _faqService.GetFAQ(id);
            return View(model);
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            FAQsModel model = new FAQsModel();
            model.FAQs = await _faqService.GetFAQs();
            return View(model);
        }

        #region Add FAQ
        [Authorize(Policy = "AccountantInvestmentManagerAccess")]
        [Route("new")]
        [HttpGet]
        public async Task<IActionResult> New()
        {
            return View();
        }

        [Route("new")]
        [HttpPost]        
        public async Task<IActionResult> New(FAQModel faqModel)
        {
            if (ModelState.IsValid)
            {
                var savedFAQ = await _faqService.SaveFAQ(faqModel);
                return RedirectToAction(nameof(FAQController.List));
            }
            return View(faqModel);            
        }
        #endregion Add FAQ

        #region Update FAQ
        [Authorize(Policy = "AccountantInvestmentManagerAccess")]
        [Route("edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            FAQModel model = await _faqService.GetFAQ(id);
            return View(model);
        }

        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(FAQModel faqModel)
        {
            if (ModelState.IsValid)
            {
                await _faqService.UpdateFAQ(faqModel);                
                var id = faqModel.Id;
                return RedirectToAction(nameof(FAQController.List));
            }
            return View(faqModel);
        }
        #endregion Update FAQ

        [Authorize(Policy = "AccountantInvestmentManagerAccess")]
        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _faqService.DeleteFAQ(id);
            return RedirectToAction(nameof(FAQController.List));
        }

        [Authorize(Policy = "AccountantInvestmentManagerAccess")]
        [Route("import")]
        [HttpPost]
        public async Task<IActionResult> ImportList(IFormFile file)
        {
            bool IsInvalidFile = false;
            if (file != null)
            {
                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    string extension = Path.GetExtension(fileName).ToLower();
                    if (extension != ".xlsx")
                    {
                        TempData["Error"] = Model.Resources.ValidationMessages.WrongExcelFile;
                        return RedirectToAction(nameof(FAQController.List));
                    }
                    IsInvalidFile = true;
                }
            }
            else
            {
                TempData["Error"] = Model.Resources.ValidationMessages.NoFile;
                return RedirectToAction(nameof(FAQController.List));
            }

            if (IsInvalidFile == true)
            {
                //check if data "Danh muc" Column in excel file was in enum type
                //if not return to "List action"
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        //Check if user choose the wrong file (Ex: FAQ instead of KVRR)
                        //question column
                        if(worksheet.Dimension == null)
                        {
                            TempData["Error"] = ValidationMessages.DataIsEmpty;
                            return RedirectToAction(nameof(FAQController.List));
                        }
                        var questionCol = worksheet.Cells["B1"].Value?.ToString().Trim();
                        if (!questionCol.Equals(Model.Resources.Common.QuestionContent))
                        {
                            TempData["Error"] = ValidationMessages.FileIsWrongType;
                            return RedirectToAction(nameof(FAQController.List));
                        }

                        var rowCount = worksheet.Dimension.Rows;
                        FAQCategory category = new FAQCategory();
                        List<string> listQuestionContent = new List<string>();
                        for (int row = 2; row <= rowCount; row++)
                        {
                            string dataCategory = "";
                            string dataQuestions = "";
                            string dataAnswer = "";
                            dataCategory = worksheet.Cells[row, 4].Value?.ToString().Trim();
                            dataQuestions = worksheet.Cells[row, 2].Value?.ToString().Trim();
                            dataAnswer = worksheet.Cells[row, 3].Value?.ToString().Trim();

                            try
                            {
                                if(dataQuestions == null)
                                {
                                    TempData["Error"] = ValidationMessages.QuestionTitleIsEmpty;
                                    return RedirectToAction(nameof(FAQController.List));
                                }

                                if (dataAnswer == null)
                                {
                                    TempData["Error"] = ValidationMessages.AnswerContentIsEmpty;
                                    return RedirectToAction(nameof(FAQController.List));
                                }

                                if (dataCategory == null)
                                {
                                    TempData["Error"] = ValidationMessages.CategoryIsEmpty;
                                    return RedirectToAction(nameof(FAQController.List));
                                }
                                else
                                {
                                    category = _faqService.IsMapData(dataCategory);
                                    if (category == (FAQCategory)404)
                                    {
                                        TempData["Error"] = string.Format(ValidationMessages.CategoryIsNotExisted,dataCategory);
                                        return RedirectToAction(nameof(FAQController.List));
                                    }
                                }

                                listQuestionContent.Add(dataQuestions);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        for (int row = 2; row <= rowCount; row++)
                        {
                            string questionsTitle = "";
                            questionsTitle = worksheet.Cells[row, 2].Value.ToString().Trim();
                            //if check > 1 then data duplicate
                            var check = listQuestionContent.FindAll(x => x == questionsTitle).Count();
                            if(check > 1)
                            {
                                TempData["Error"] = ValidationMessages.DuplicatedQuestionImport + " '" + questionsTitle + "'";
                                return RedirectToAction(nameof(FAQController.List));
                            }
                        }
                    }
                }
                var result = await _faqService.ImportFAQs(file);
            }
            TempData["import_ok"] = ValidationMessages.ImportSuccessful;
            return RedirectToAction(nameof(FAQController.List));
        }

       [Authorize(Policy = "CustomerManagerNotAccess")]
        [Route("export")]
        [HttpGet]
        public IActionResult Export()
        {
            var fileContent = _faqService.ExportFAQs();
            return File(fileContent?.Result, "application/ms-excel", $"FAQ.xlsx");
        }

        [Authorize(Policy = "CustomerManagerNotAccess")]
        [Route("exportexamplefile")]
        [HttpGet]
        public IActionResult ExportExampleFile()
        {
            var fileContent = _faqService.ExportExampleFile();
            return File(fileContent, "application/ms-excel", $"FAQExample.xlsx");
        }
    }
}
