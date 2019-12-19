using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using smartFunds.Model.Common;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Authorize(Policy = "OnlyAdminAccess")]
    [Route("admin/customerlevel")]
    public class CustomerLevelController : Controller
    {
        private readonly ICustomerLevelService _customerLevelService;
        private readonly IConfiguration _configuration;

        public CustomerLevelController(ICustomerLevelService customerLevelService, IConfiguration configuration)
        {
            _customerLevelService = customerLevelService;
            _configuration = configuration;
        }

        [Route("list")]
        public IActionResult List(int pageSize = 0, int pageIndex = 0)
        {
            try
            {
                CustomerLevelsModel model = new CustomerLevelsModel();
                model.CustomerLevels = _customerLevelService.GetCustomerLevels(pageSize, pageIndex)?.ToList();
                if(model.CustomerLevels.Count > 0)
                {
                    foreach (var item in model.CustomerLevels)
                    {
                        if (item.MaxMoney == Common.Constants.MaxDecimal.MaxMoney) item.MaxMoney = 0;
                    }
                }

                    return View(model);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Route("new")]
        [HttpGet]
        public IActionResult New()
        {
            try
            {
                @ViewBag.CheckValue = string.Empty;
                @ViewBag.CheckName = string.Empty;
                return View(new CustomerLevelModel());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> New(CustomerLevelModel customerLevel)
        {
            try
            {
                if (customerLevel == null) return Json(false, new JsonSerializerSettings());
                if (!ModelState.IsValid)
                {
                    return View(customerLevel);
                }

                @ViewBag.CheckValue = string.Empty;
                @ViewBag.CheckName = string.Empty;

                if (string.IsNullOrEmpty(customerLevel.NameCustomerLevel))
                {
                    @ViewBag.CheckName = "Tên phân cấp khách hàng không được để trống";
                    return View(customerLevel);
                }

                var checkName = _customerLevelService.IsDuplicateName(customerLevel.NameCustomerLevel, customerLevel.IDCustomerLevel).Result;

                if (!checkName)
                {
                    @ViewBag.CheckName = "Tên phân cấp khách hàng đã tồn tại";
                    return View(customerLevel);
                }
                if (customerLevel.MinMoney == null && customerLevel.MaxMoney == null)
                {
                    @ViewBag.CheckValue = "Tổng số tiền đầu tư không được để trống";
                    return View(customerLevel);
                }
                if (customerLevel.MinMoney == 0 && customerLevel.MaxMoney == 0)
                {
                    @ViewBag.CheckValue = "Giá trị không hợp lệ";
                    return View(customerLevel);
                }
                if (customerLevel.MinMoney > customerLevel.MaxMoney)
                {
                    @ViewBag.CheckValue = "Số tiền Từ phải nhỏ hơn Số tiền Đến";
                    return View(customerLevel);
                }
                var checkStatus = await _customerLevelService.IsCheckSumInvest(customerLevel);
                if (!checkStatus)
                {
                    @ViewBag.CheckValue = "Giá trị đã tồn tại";
                    return View(customerLevel);
                }
                //
                if (customerLevel.MaxMoney == null) customerLevel.MaxMoney = Common.Constants.MaxDecimal.MaxMoney;

                var result = await _customerLevelService.Save(customerLevel);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("List");
        }

        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                @ViewBag.CheckValue = string.Empty;
                @ViewBag.CheckName = string.Empty;
                var customerLevel = _customerLevelService.GetCustomerLevelById(id);
                //
                if (customerLevel.MaxMoney == Common.Constants.MaxDecimal.MaxMoney) customerLevel.MaxMoney = null;
                return View(customerLevel);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Route("editcustomerlevel")]
        [HttpPost]
        public async Task<IActionResult> EditCustomerLevel(CustomerLevelModel customerLevel)
        {
            try
            {
                if (customerLevel == null) return Json(false, new JsonSerializerSettings());
                if (!ModelState.IsValid)
                {
                    return View("Edit", customerLevel);
                }
                @ViewBag.CheckValue = string.Empty;
                @ViewBag.CheckName = string.Empty;

                if (string.IsNullOrEmpty(customerLevel.NameCustomerLevel))
                {
                    @ViewBag.CheckName = "Tên phân cấp khách hàng không được để trống";
                    return View(customerLevel);
                }

                var checkName = _customerLevelService.IsDuplicateName(customerLevel.NameCustomerLevel, customerLevel.IDCustomerLevel).Result;

                if (!checkName)
                {
                    @ViewBag.CheckName = "Tên phân cấp khách hàng đã tồn tại";
                    return View("Edit", customerLevel);
                }
                if (customerLevel.MinMoney == null && customerLevel.MaxMoney == null)
                {
                    @ViewBag.CheckValue = "Tổng số tiền đầu tư không được để trống";
                    return View("Edit", customerLevel);
                }
                if (customerLevel.MinMoney == 0 && customerLevel.MaxMoney == 0)
                {
                    @ViewBag.CheckValue = "Giá trị không hợp lệ";
                    return View("Edit", customerLevel);
                }
                if (customerLevel.MinMoney > customerLevel.MaxMoney)
                {
                    @ViewBag.CheckValue = "Số tiền Từ phải nhỏ hơn Số tiền Đến";
                    return View("Edit", customerLevel);
                }
                var checkStatus = await _customerLevelService.IsCheckSumInvest(customerLevel);
                if (!checkStatus)
                {
                    @ViewBag.CheckValue = "Giá trị đã tồn tại";
                    return View("Edit", customerLevel);
                }
                //
                if (customerLevel.MaxMoney == null) customerLevel.MaxMoney = Common.Constants.MaxDecimal.MaxMoney;

                var result = await _customerLevelService.Update(customerLevel);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("List");
        }

        [Route("detail/{id}")]
        [HttpGet]
        public IActionResult Detail(int id)
        {
            try
            {
                var customerLevel = _customerLevelService.GetCustomerLevelById(id);
                return View(customerLevel);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(IFormCollection collection)
        {
            List<int> lstCustomerLevelIds = new List<int>();
            foreach (var key in collection.Keys)
            {
                if (key.Contains("checkbox_customerLevel"))
                {
                    lstCustomerLevelIds.Add(int.Parse(key.Replace("checkbox_customerLevel", "")));
                }
            }

            await _customerLevelService.DeleteCustomerLevels(lstCustomerLevelIds.ToArray());
            return RedirectToAction("List");
        }
    }
}