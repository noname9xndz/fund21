using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using smartFunds.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Common.Helpers;
using smartFunds.Data.Models;
using smartFunds.Model.Common;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Authorize(Policy = "CustomerManagerNotAccess")]
    [Route("admin/kvrrmark")]
    public class KVRRMarkController : Controller
    {
        private readonly IKVRRMarkService _kvrrMarkService;
        private readonly IKVRRService _kvrrService;

        public KVRRMarkController(IKVRRMarkService kvrrMarkService, IKVRRService kvrrService)
        {
            _kvrrMarkService = kvrrMarkService;
            _kvrrService = kvrrService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List(int pageSize = 0, int pageIndex = 0)
        {
            KVRRMarksModel model = new KVRRMarksModel();
            model.KVRRMarks = await _kvrrMarkService.GetKVRRMarks(pageSize, pageIndex);

            return View("List", model);
        }

        [HttpGet("new")]
        public async Task<IActionResult> New()
        {
            var mark = new KVRRMarkModel();
            var kvrrUnUser = await _kvrrService.GetKVRRMarkUnUse();

            var kvrrs = new List<KVRRModel>
            {
                new KVRRModel
                {
                    Id = 0,
                    Name = Model.Resources.Common.KVRRType
                }
            };
            kvrrs.AddRange(kvrrUnUser);
            ViewBag.KVRRSelectList = kvrrs;
            return View("New", mark);
        }

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> New(KVRRMarkModel mark)
        {
            try
            {
                if (mark == null) throw new InvalidParameterException();
                bool isValid = true;

                if (mark.MarkFrom > mark.MarkTo)
                {
                    isValid = false;
                    ModelState.AddModelError("ValidMark", Model.Resources.ValidationMessages.CompareMarkFromMarkTo);
                }
                else
                {
                    var isValidMarkFrom = await _kvrrMarkService.ValidMark(mark);
                    if (!isValidMarkFrom)
                    {
                        isValid = false;
                        ModelState.AddModelError("ValidMark", Model.Resources.ValidationMessages.MarkExisted);
                    }
                }

                if (!isValid)
                {
                    var kvrrUnUser = await _kvrrService.GetKVRRMarkUnUse();

                    var kvrrs = new List<KVRRModel>
                    {
                        new KVRRModel
                        {
                            Id = 0,
                            Name = Model.Resources.Common.KVRRType
                        }
                    };
                    kvrrs.AddRange(kvrrUnUser);
                    ViewBag.KVRRSelectList = kvrrs;
                    return View("New", mark);
                }

                var result = await _kvrrMarkService.Save(mark);

                return RedirectToAction("List");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var mark = await _kvrrMarkService.GetKVRRMarkById(id);
                if (mark == null) throw new NotFoundException();

                var kvrrs = new List<KVRRModel>();
                var kvrrUnUser = await _kvrrService.GetKVRRMarkUnUse();

                if (mark.KVRRId == null || mark.KVRRId == 0)
                {
                    kvrrs.Add(new KVRRModel { Id = 0, Name = Model.Resources.Common.KVRRType });
                }
                else
                {
                    var currentKVRR = _kvrrService.GetKVRRById(mark.KVRRId);
                    if (currentKVRR == null) throw new NotFoundException();
                    kvrrs.Add(currentKVRR);
                }
                kvrrs.AddRange(kvrrUnUser);
                ViewBag.KVRRSelectList = kvrrs;
                return View("Edit", mark);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(KVRRMarkModel mark)
        {
            try
            {
                if (mark == null) throw new InvalidParameterException();
                mark.EntityState = FormState.Edit;

                bool isValid = true;

                if (mark.MarkFrom > mark.MarkTo)
                {
                    isValid = false;
                    ModelState.AddModelError("ValidMark", Model.Resources.ValidationMessages.CompareMarkFromMarkTo);
                }
                else
                {
                    var isValidMark= await _kvrrMarkService.ValidMark(mark);
                    if (!isValidMark)
                    {
                        isValid = false;
                        ModelState.AddModelError("ValidMark", Model.Resources.ValidationMessages.MarkExisted);
                    }           
                }

                if (!isValid)
                {
                    var kvrrUnUser = await _kvrrService.GetKVRRMarkUnUse();

                    var kvrrs = new List<KVRRModel>
                    {
                        new KVRRModel
                        {
                            Id = mark.KVRRId,
                            Name = _kvrrService.GetKVRRById(mark.KVRRId)?.Name ?? string.Empty
                        }
                    };
                    kvrrs.AddRange(kvrrUnUser);
                    ViewBag.KVRRSelectList = kvrrs;
                    return View("Edit", mark);
                }

                await _kvrrMarkService.Update(mark);
                return RedirectToAction("List");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("detail")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var mark = await _kvrrMarkService.GetKVRRMarkById(id);
                var kvrrUnUser = await _kvrrService.GetKVRRMarkUnUse();
                kvrrUnUser?.ToList().Insert(0, new KVRRModel { Id = 0, Name = Model.Resources.Common.KVRRType });
                ViewBag.KVRRSelectList = kvrrUnUser;
                return View("Detail", mark);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(IFormCollection collection)
        {
            List<int> ids = new List<int>();
            foreach (var key in collection.Keys)
            {
                if (key.Contains("checkbox_kvrrmark"))
                {
                    ids.Add(int.Parse(key.Replace("checkbox_kvrrmark", "")));
                }
            }

            await _kvrrMarkService.Delete(ids.ToArray());
            return RedirectToAction("List");
        }

        [HttpPost("KVRRIsNotEmpty")]
        public async Task<IActionResult> KVRRIsNotEmpty(int kvrrId, int initKvrrId)
        {
            return Json(await _kvrrMarkService.KVRRIsNotEmpty(kvrrId, initKvrrId));
        }
    }
}
