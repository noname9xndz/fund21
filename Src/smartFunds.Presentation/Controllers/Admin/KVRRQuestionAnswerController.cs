using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using smartFunds.Model.Common;
using smartFunds.Service.Services;
using smartFunds.Common;
using smartFunds.Common.Exceptions;
using System.IO;
using OfficeOpenXml;
using System;
using smartFunds.Model.Resources;

namespace smartFunds.Presentation.Controllers.Admin
{   
    [Route("admin/kvrrquestionanswer")]
    public class KVRRQuestionAnswerController : Controller
    {
        private readonly IKVRRQuestionAnswerService _kvrrQuestionAnswerService;

        public KVRRQuestionAnswerController(IKVRRQuestionAnswerService kvrrQuestionAnswerService)
        {
            _kvrrQuestionAnswerService = kvrrQuestionAnswerService;
        }

        [Authorize(Policy = "CustomerManagerNotAccess")]
        [Route("list")]
        public async Task<IActionResult> List()
        {
            try
            {
                var questions = await _kvrrQuestionAnswerService.GetKVRRQuestions();
                var result = questions.OrderBy(x => x.No).ToList();
                return View(result);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "AdminInvestmentManagerAccess")]
        [Route("new")]
        [HttpGet]
        public IActionResult New()
        {
            return View(new KVRRQuestion
            {
                Id = 0,
                KVRRAnswers = new List<KVRRAnswer> {new KVRRAnswer(), new KVRRAnswer() },
                Content = string.Empty,
                No = 0
            });
        }

        [Route("newquestiondefindkvrr")]
        [HttpPost]
        public async Task<IActionResult> NewQuestionDefindKVRR([FromBody]KVRRQuestion data)
        {
            try
            {
                    await _kvrrQuestionAnswerService.Save(data);
                    return Json(new { Success = true });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "AdminInvestmentManagerAccess")]
        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var question = await _kvrrQuestionAnswerService.GetKVRRQuestionById(id);
                if (question.KVRRAnswers == null || !question.KVRRAnswers.Any())
                {
                    question.KVRRAnswers = new List<KVRRAnswer>{ new KVRRAnswer() };
                    question.KVRRAnswers.Add(new KVRRAnswer());
                }
                return View(question);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Route("editquestiondefindkvrr")]
        [HttpPost]
        public async Task<IActionResult> EditQuestionDefindKVRR([FromBody]KVRRQuestion question)
        {
            try
            {
                await _kvrrQuestionAnswerService.Update(question);
                return Json(new { Success = true });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "AdminInvestmentManagerAccess")]
        [Route("deleteanswer")]
        [HttpPost]
        public async Task<IActionResult> DeleteAnswer(IFormCollection collection)
        {
            try
            {
                List<int> ids = new List<int>();
                foreach (var key in collection.Keys)
                {
                    if (key.Contains("defind-anwser"))
                    {
                        ids.Add(int.Parse(key.Replace("defind-anwser", "")));
                    }
                }

                await _kvrrQuestionAnswerService.DeleteAnswer(ids);
                return RedirectToAction("List");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "AdminInvestmentManagerAccess")]
        [Route("deletequestion")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            try
            {
                await _kvrrQuestionAnswerService.DeleteQuestion(id);
                return RedirectToAction("List");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "AdminInvestmentManagerAccess")]
        [Route("UpdateQuestionOrder")]
        [HttpPost]
        public async Task<IActionResult> UpdateQuestionOrder(int questionId, int newOrder, int currentOrder)
        {
            try
            {
                if(questionId < 0 || newOrder < 0 || currentOrder < 0 || newOrder == currentOrder)
                    throw new InvalidParameterException();

                var secondQuestion = await _kvrrQuestionAnswerService.GetKVRRQuestionByNo(newOrder);
                secondQuestion.No = currentOrder;
                await _kvrrQuestionAnswerService.UpdateQuestionOrder(secondQuestion);

                var currentQuestion = await _kvrrQuestionAnswerService.GetKVRRQuestionById(questionId);
                currentQuestion.No = newOrder;
                await _kvrrQuestionAnswerService.UpdateQuestionOrder(currentQuestion);
                return Json(new {success = true});
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        //[Authorize(Policy = )]
        [Route("ImportList")]
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
                    
                    if (extension != ".xlsx" )
                    {
                        TempData["Error"] = ValidationMessages.WrongExcelFile;
                        return RedirectToAction(nameof(KVRRQuestionAnswerController.List));
                    }

                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                            if (worksheet.Dimension == null)
                            {
                                TempData["Error"] = ValidationMessages.DataIsEmpty;
                                return RedirectToAction(nameof(KVRRQuestionAnswerController.List));
                            }
                            //Check if user choose the wrong file (Ex: KVRR instead of FAQ)
                            //question column
                            var questionCol = worksheet.Cells["B1"].Value?.ToString().Trim();
                            if (!questionCol.Equals(Model.Resources.Common.PortfolioContent))
                            {
                                TempData["Error"] = ValidationMessages.FileIsWrongType;
                                return RedirectToAction(nameof(FAQController.List));
                            }

                            KVRRQuestionCategories checkValidateInsideFile = new KVRRQuestionCategories();
                            checkValidateInsideFile = _kvrrQuestionAnswerService.CheckNeededFields(file);
                            if (checkValidateInsideFile == (KVRRQuestionCategories)404)
                            {
                                TempData["Error"] = string.Format(ValidationMessages.FieldEmpty, "Danh mục");
                                return RedirectToAction(nameof(KVRRQuestionAnswerController.List));
                            }

                            var rowCount = worksheet.Dimension.Rows;
                            List<string> listQuestionContent = new List<string>();
                            for (int row = 2; row <= rowCount; row++)
                            {
                                string questionsTitle = "";
                                questionsTitle = worksheet.Cells[row, 3].Value?.ToString().Trim();
                                listQuestionContent.Add(questionsTitle);
                                //if check > 1 then data duplicate
                                var check = listQuestionContent.FindAll(x => x == questionsTitle).Count();
                                if (check > 1)
                                {
                                    TempData["Error"] = ValidationMessages.DuplicatedQuestionImport + " '" + questionsTitle + "'";
                                    return RedirectToAction(nameof(KVRRQuestionAnswerController.List));
                                }
                            }
                            if (checkValidateInsideFile == (KVRRQuestionCategories)400)
                            {
                                TempData["Error"] = string.Format(ValidationMessages.CategoryIsNotExisted, "");
                                return RedirectToAction(nameof(KVRRQuestionAnswerController.List));
                            }
                            if (checkValidateInsideFile == (KVRRQuestionCategories)401)
                            {
                                TempData["Error"] = string.Format(ValidationMessages.FieldEmpty, "Câu hỏi");
                                return RedirectToAction(nameof(KVRRQuestionAnswerController.List));
                            }
                            if (checkValidateInsideFile == (KVRRQuestionCategories)402)
                            {
                                TempData["Error"] = string.Format(ValidationMessages.MarkMustBeNumber);
                                return RedirectToAction(nameof(KVRRQuestionAnswerController.List));
                            }
                            if (checkValidateInsideFile == (KVRRQuestionCategories)403)
                            {
                                TempData["Error"] = string.Format(ValidationMessages.LessThan2Answers);
                                return RedirectToAction(nameof(KVRRQuestionAnswerController.List));
                            }
                            if (checkValidateInsideFile == (KVRRQuestionCategories)405)
                            {
                                TempData["Error"] = string.Format(ValidationMessages.MarkOrAnswerIsNull);
                                return RedirectToAction(nameof(KVRRQuestionAnswerController.List));
                            }
                            IsInvalidFile = true;
                        }
                        
                    }
                }
            }
            else
            {
                TempData["Error"] = ValidationMessages.NoFile;
                return RedirectToAction(nameof(KVRRQuestionAnswerController.List));
            }

            if (IsInvalidFile == true)
            {
                TempData["import_ok"] = ValidationMessages.ImportSuccessful;
                await _kvrrQuestionAnswerService.ImportListQuestions(file);
            }
            return RedirectToAction(nameof(KVRRQuestionAnswerController.List));
        }

        [Authorize(Policy = "AdminInvestmentManagerAccess")]
        [Route("export")]
        [HttpGet]
        public IActionResult ExportExampleFile()
        {
            var fileContent = _kvrrQuestionAnswerService.ExportExampleKVRRs();
            return File(fileContent, "application/ms-excel", $"ListKVRRExample.xlsx");
        }

    }
}
