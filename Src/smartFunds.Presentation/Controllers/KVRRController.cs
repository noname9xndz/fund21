using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using smartFunds.Model.Common;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers
{
    /// <summary>
    /// This is a test controller that demo the flow of application.
    /// </summary>
    public class KVRRController : Controller
    {
        private readonly IKVRRService _kvrrService;

        public KVRRController(IKVRRService kvrrService)
        {
            _kvrrService = kvrrService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var questions = GetListKVRRQuestion();
            return View("Index", questions);
        }

        public ActionResult AddKVRRQuestion()
        {
            return View("DefineKVRRDetail", new KVRRQuestion());
        }

        public IActionResult SaveDefineKVRRQuestion(KVRRQuestion kvrrQuestion)
        {
            try
            {
                var result = _kvrrService.SaveKvrrQuestion(kvrrQuestion);
                return Json(new { result = result });
            }
            catch (System.Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        public IActionResult DeleteAnswer(int id)
        {
            try
            {
                var result = _kvrrService.DeleteAnswer(id);
                return Json(new { result = result });
            }
            catch (System.Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        public IActionResult DeleteQuestion(int id)
        {
            try
            {
                var result = _kvrrService.DeleteQuestion(id);
                return Json(new { result = result });
            }
            catch (System.Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        private IEnumerable<KVRRQuestion> GetListKVRRQuestion()
        {
            try
            {
                return _kvrrService.GetKVRR();
            }
            catch (System.Exception ex)
            {

            }

            return null;
        }
    }
}
