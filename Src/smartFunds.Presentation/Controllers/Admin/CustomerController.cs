using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Model.Admin;
using smartFunds.Model.Common;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Route("admin/customer")]   
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IInvestmentTargetService _investmentTargetService;
        private readonly IConfiguration _configuration;

        public CustomerController(ICustomerService customerService, IUserService userService, ITransactionHistoryService transactionHistoryService, IInvestmentTargetService investmentTargetService, IConfiguration configuration)
        {
            _customerService = customerService;
            _userService = userService;
            _transactionHistoryService = transactionHistoryService;
            _configuration = configuration;
            _investmentTargetService = investmentTargetService;
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("detail")]
        [HttpGet]
        public async Task<IActionResult> Detail(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ApplicationException("Can't get customer with id null");
            }

            DetailCustomerViewModel model = new DetailCustomerViewModel();
            model.Customer = await _customerService.GetCustomerById(customerId);

            if (model.Customer == null)
            {
                return RedirectToAction(nameof(ErrorController.Error404), "Error");
            }

            var size = 3;
            var page = 1;
            model.ListTransactionHistoryModel = await _transactionHistoryService.GetListTransactionHistory(size, page, customerId);

            var dateFrom = DateTime.Now.AddDays(-30).Date;
            var dateTo = DateTime.Now.Date;
            model.PropertyFluctuations = await _transactionHistoryService.GetPropertyFluctuations(customerId, dateFrom, dateTo);

            model.InvestmentTarget = await _investmentTargetService.GetInvestmentTarget(customerId);

            model.UserPortfolios = await _userService.GetUserPorfolio(customerId);

            return View(model);
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> List(int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            ViewBag.SearchCustomerViewModel = new SearchCustomer();

            CustomersModel model = new CustomersModel();
            model = await _customerService.GetAllCustomer();
            model.PageIndex = page;
            model.PageSize = size;
            return View(model);
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("list")]
        [HttpPost]
        public async Task<IActionResult> List([FromBody]SearchCustomer model, int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            var customersModel = await _customerService.GetListCustomer(size, page, model);
            customersModel.PageIndex = page;
            customersModel.PageSize = size;

            return PartialView("Views/Customer/ListCustomerPartial.cshtml", customersModel);
        }

        [Authorize(Policy = "OnlyAdminAccess")]
        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(List<string> CustomerIds = null)
        {
            if (CustomerIds == null || !CustomerIds.Any())
            {
                TempData["Message"] = Model.Resources.ValidationMessages.CustomerNotSelect;
                return RedirectToAction(nameof(CustomerController.List), "Customer");
            }

            var deleteCustomer = await _customerService.DeleteCustomerByIds(CustomerIds);
            if (deleteCustomer)
            {
                TempData["Message"] = Model.Resources.ValidationMessages.DeleteCustomerSuccess;
            }
            else
            {
                TempData["Message"] = Model.Resources.ValidationMessages.DeleteCustomerError;
            }

            return RedirectToAction(nameof(CustomerController.List), "Customer");
        }

        [Authorize(Policy = "OnlyAdminAccess")]
        [Route("edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ApplicationException("Can't edit customer with id null");
            }

            var customer = await _customerService.GetCustomerById(customerId);

            if (customer != null)
            {
                var model = new EditCustomerViewModel();
                model.CustomerId = customerId;
                model.FullName = customer.FullName;
                model.PhoneNumber = customer.PhoneNumber;
                model.Created = customer.Created;
                model.LastLogin = customer.LastLogin;
                return View(model);
            }

            //return to error page
            ModelState.AddModelError(string.Empty, Model.Resources.ValidationMessages.ResourceManager.GetString("NotFoundCustomer"));
            return Redirect("/error");
        }

        [Authorize(Policy = "OnlyAdminAccess")]
        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrWhiteSpace(model.CustomerId))
            {
                throw new ApplicationException("Can't edit customer with id null");
            }

            var customer = await _customerService.GetCustomerById(model.CustomerId);
            if (customer != null)
            {
                customer.FullName = model.FullName;
                var updateCustomer = await _customerService.UpdateCustomer(customer);
                if (updateCustomer)
                {
                    var customerId = model.CustomerId;
                    return RedirectToAction(nameof(CustomerController.Detail), "Customer", new { customerId });
                }
            }

            ModelState.AddModelError(string.Empty, Model.Resources.ValidationMessages.ResourceManager.GetString("EditCustomerError"));
            return View();
        }

        [Authorize(Policy = "CustomerManagerNotAccess")]
        [Route("export")]
        [HttpPost]
        public async Task<IActionResult> Export(SearchCustomer searchModel)
        {
            var fileContent = await _customerService.ExportCustomer(searchModel);

            return File(fileContent, "application/ms-excel", $"Customers.xlsx");
        }

        [Route("PropertyFluctuations")]
        [HttpPost]
        public async Task<IActionResult> PropertyFluctuations(string customerId, string dateFrom, string dateTo)
        {
            var model = new PropertyFluctuations();

            DateTime from = new DateTime();
            DateTime to = new DateTime();
            if (string.IsNullOrWhiteSpace(dateFrom) || string.IsNullOrWhiteSpace(dateTo) || !DateTime.TryParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out from) || !DateTime.TryParseExact(dateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out to) || from > to)
            {
                from = DateTime.Now.AddDays(-30).Date;
                to = DateTime.Now.Date;
            }

            model = await _transactionHistoryService.GetPropertyFluctuations(customerId, from, to);

            return Ok(model);
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("SearchTransactionHistory")]
        [HttpPost]
        public async Task<IActionResult> SearchTransactionHistory(string customerId, int type = 0, int status = 0, string transactionDateFrom = null, string transactionDateTo = null, int pageIndex = 1)
        {
            var model = new ListTransactionHistoryModel();

            DateTime from = new DateTime();
            DateTime to = new DateTime();
            if (!string.IsNullOrWhiteSpace(transactionDateFrom) && !string.IsNullOrWhiteSpace(transactionDateTo) && DateTime.TryParseExact(transactionDateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out from) && DateTime.TryParseExact(transactionDateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out to) && from > to)
            {
                return NoContent();
            }
            else
            {
                var searchModel = new SearchTransactionHistory()
                {
                    TransactionType = (TransactionType)type,
                    Status = (TransactionStatus)status,
                    TransactionDateFrom = transactionDateFrom,
                    TransactionDateTo = transactionDateTo
                };
                var pageSize = _configuration.GetValue<int>("PagingConfig:CustomerTransactionHistoryPageSize");
                model = await _transactionHistoryService.GetListTransactionHistory(pageSize, pageIndex, customerId, searchModel);
                model.PageIndex = pageIndex;
                model.PageSize = pageSize;
            }

            return PartialView("Views/Customer/TransactionHistoryPartial.cshtml", model);
        }
    }
}
