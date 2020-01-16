using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Controllers.Client
{
    [Route("investment-target")]
    public class InvestmentTargetController : Controller
    {
        private readonly InvestmentTargetService _investmentTargetService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;
        private readonly IGlobalConfigurationService _globalConfigurationService;

        public InvestmentTargetController(InvestmentTargetService investmentTargetService, IUserService userService, IConfiguration configuration, IOrderService orderService, IGlobalConfigurationService globalConfigurationService)
        {
            _investmentTargetService = investmentTargetService;
            _userService = userService;
            _configuration = configuration;
            _orderService = orderService;
            _globalConfigurationService = globalConfigurationService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Detail()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await _userService.GetCurrentUser();

            if (currentUser.KVRR == null)
            {
                return RedirectToAction(nameof(AccountController.MyPortfolio), "Account");
            }

            var investmentTargetModel = await _investmentTargetService.GetInvestmentTarget(currentUser.Id);
            if (investmentTargetModel == null)
            {
                return RedirectToAction(nameof(InvestmentTargetController.New), "InvestmentTarget");
            }

            if (investmentTargetModel.Status == Common.EditStatus.Updating || investmentTargetModel.InvestmentStatus == Model.Resources.Common.Completed)
            {
                await _investmentTargetService.DeleteInvestmentTarget(investmentTargetModel);
                return RedirectToAction(nameof(InvestmentTargetController.New), "InvestmentTarget");
            }

            return View(investmentTargetModel);
        }

        [Route("new")]
        [HttpGet]
        public async Task<IActionResult> New()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var config = await _globalConfigurationService.GetValueConfig(Constants.Configuration.ProgramLocked);
            if (config.Contains("true"))
            {
                return View("~/Views/Lock.cshtml");
            }

            var currentUser = await _userService.GetCurrentUser();

            var model = new InvestmentTargetModel() { Duration = Common.Duration.TwelfthMonth, Frequency = Common.Frequency.OneWeek, User = currentUser };

            return View(model);
        }

        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> New(InvestmentTargetModel model)
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var config = await _globalConfigurationService.GetValueConfig(Constants.Configuration.ProgramLocked);
            if (config.Contains("true"))
            {
                return View("~/Views/Lock.cshtml");
            }
            var currentUser = await _userService.GetCurrentUser();
            model.User = currentUser;

            if (ModelState.IsValid)
            {
                if (model.ExpectedAmount == null)
                {
                    ModelState.AddModelError("ExpectedAmount", string.Format(ValidationMessages.FieldEmpty, Model.Resources.Common.ExpectedAmount));
                }
                else if (model.ExpectedAmount < _configuration.GetValue<decimal>("InvestmentValidation:MinTargetAmount"))
                {
                    ModelState.AddModelError("ExpectedAmount", string.Format(ValidationMessages.InvestmentTargetAmountInvalid, _configuration.GetValue<decimal>("InvestmentValidation:MinTargetAmount")));
                }
                else if (model.ExpectedAmount <= currentUser.CurrentAccountAmount)
                {
                    ModelState.AddModelError("ExpectedAmount", ValidationMessages.InvestmentTargetAmountInvalid2);
                }
                else
                {
                    var oneTimeAmount = await OneTimeAmount((decimal)model.ExpectedAmount, (int)model.Duration, (int)model.Frequency);
                    if (oneTimeAmount <= 0)
                    {
                        ModelState.AddModelError("ExpectedAmount", ValidationMessages.InvestmentTargetAmountInvalid3);
                        return View(model);
                    }
                    model.Status = Common.EditStatus.Updating;
                    var newInvestmentTarget = await _investmentTargetService.AddInvestmentTarget(model);

                    var orderModel = new OrderModel()
                    {
                        Desc = "Dau tu Savenow",
                        MerchantCode = _configuration.GetValue<string>("PaymentParam:merchant_code"),
                        Msisdn = currentUser.PhoneNumber,
                        TransAmount = newInvestmentTarget.OneTimeAmount.ToString("0"),
                        Version = _configuration.GetValue<string>("PaymentParam:version"),
                        IsInvestmentTarget = true
                    };

                    var order = await _orderService.SaveOrder(orderModel);

                    var check_sum = Helpers.CreateCheckSum(_configuration.GetValue<string>("PaymentSecurity:AccessCode"), _configuration.GetValue<string>("PaymentSecurity:SecretKey"),
                        order.Id.ToString(), _configuration.GetValue<string>("PaymentParam:command"), order.MerchantCode, order.Id.ToString(), order.TransAmount, order.Version);

                    var paymentLink = _configuration.GetValue<bool>("PaymentLink:IsLive") ? _configuration.GetValue<string>("PaymentLink:Live") : _configuration.GetValue<string>("PaymentLink:Test");
                    var paymentParameters = string.Format("billcode={0}&command={1}&desc={2}&merchant_code={3}&sender_msisdn={4}&order_id={5}&return_url={6}&cancel_url={7}&trans_amount={8}&version={9}&check_sum={10}",
                        order.Id.ToString(), _configuration.GetValue<string>("PaymentParam:command"), orderModel.Desc, _configuration.GetValue<string>("PaymentParam:merchant_code"), currentUser.PhoneNumber,
                        order.Id.ToString(), Url.Action(action: nameof(InvestmentController.DoPayment), controller: "Investment", values: null, protocol: Request.Scheme), Url.Action(action: nameof(InvestmentController.CancelPayment), controller: "Investment", values: null, protocol: Request.Scheme),
                        orderModel.TransAmount, _configuration.GetValue<string>("PaymentParam:version"), check_sum);

                    return Redirect(paymentLink + paymentParameters);
                }
            }

            return View(model);
        }

        [Route("update")]
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var config = await _globalConfigurationService.GetValueConfig(Constants.Configuration.ProgramLocked);
            if (config.Contains("true"))
            {
                return View("~/Views/Lock.cshtml");
            }

            var currentUser = await _userService.GetCurrentUser();

            var model = new InvestmentTargetModel() { Duration = Common.Duration.TwelfthMonth, Frequency = Common.Frequency.OneWeek, User = currentUser };

            return View(model);
        }

        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> Update(InvestmentTargetModel model)
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var config = await _globalConfigurationService.GetValueConfig(Constants.Configuration.ProgramLocked);
            if (config.Contains("true"))
            {
                return View("~/Views/Lock.cshtml");
            }
            var currentUser = await _userService.GetCurrentUser();
            model.User = currentUser;

            if (ModelState.IsValid)
            {
                if (model.ExpectedAmount == null)
                {
                    ModelState.AddModelError("ExpectedAmount", string.Format(ValidationMessages.FieldEmpty, Model.Resources.Common.ExpectedAmount));
                }
                else if (model.ExpectedAmount < _configuration.GetValue<decimal>("InvestmentValidation:MinTargetAmount"))
                {
                    ModelState.AddModelError("ExpectedAmount", string.Format(ValidationMessages.InvestmentTargetAmountInvalid, _configuration.GetValue<decimal>("InvestmentValidation:MinTargetAmount")));
                }
                else if (model.ExpectedAmount <= currentUser.CurrentAccountAmount)
                {
                    ModelState.AddModelError("ExpectedAmount", ValidationMessages.InvestmentTargetAmountInvalid2);
                }
                else
                {
                    var oneTimeAmount = await OneTimeAmount((decimal)model.ExpectedAmount, (int)model.Duration, (int)model.Frequency);
                    if (oneTimeAmount <= 0)
                    {
                        ModelState.AddModelError("ExpectedAmount", ValidationMessages.InvestmentTargetAmountInvalid3);
                        return View(model);
                    }
                    var currentInvestmentTarget = await _investmentTargetService.GetInvestmentTarget(currentUser.Id);
                    await _investmentTargetService.DeleteInvestmentTarget(currentInvestmentTarget);

                    model.Status = Common.EditStatus.Updating;
                    var newInvestmentTarget = await _investmentTargetService.AddInvestmentTarget(model);


                    var orderModel = new OrderModel()
                    {
                        Desc = "Dau tu Savenow",
                        MerchantCode = _configuration.GetValue<string>("PaymentParam:merchant_code"),
                        Msisdn = currentUser.PhoneNumber,
                        TransAmount = newInvestmentTarget.OneTimeAmount.ToString("0"),
                        Version = _configuration.GetValue<string>("PaymentParam:version"),
                        IsInvestmentTarget = true
                    };

                    var order = await _orderService.SaveOrder(orderModel);

                    var check_sum = Helpers.CreateCheckSum(_configuration.GetValue<string>("PaymentSecurity:AccessCode"), _configuration.GetValue<string>("PaymentSecurity:SecretKey"),
                        order.Id.ToString(), _configuration.GetValue<string>("PaymentParam:command"), order.MerchantCode, order.Id.ToString(), order.TransAmount, order.Version);

                    var paymentLink = _configuration.GetValue<bool>("PaymentLink:IsLive") ? _configuration.GetValue<string>("PaymentLink:Live") : _configuration.GetValue<string>("PaymentLink:Test");
                    var paymentParameters = string.Format("billcode={0}&command={1}&desc={2}&merchant_code={3}&sender_msisdn={4}&order_id={5}&return_url={6}&cancel_url={7}&trans_amount={8}&version={9}&check_sum={10}",
                        order.Id.ToString(), _configuration.GetValue<string>("PaymentParam:command"), orderModel.Desc, _configuration.GetValue<string>("PaymentParam:merchant_code"), currentUser.PhoneNumber,
                        order.Id.ToString(), Url.Action(action: nameof(InvestmentController.DoPayment), controller: "Investment", values: null, protocol: Request.Scheme), Url.Action(action: nameof(InvestmentController.CancelPayment), controller: "Investment", values: null, protocol: Request.Scheme),
                        orderModel.TransAmount, _configuration.GetValue<string>("PaymentParam:version"), check_sum);

                    return Redirect(paymentLink + paymentParameters);
                }
            }

            return View(model);
        }

        [Route("GetOneTimeAmount")]
        [HttpPost]
        public async Task<IActionResult> GetOneTimeAmount(string expectedAmount, int duration, int frequency)
        {
            var currentUser = await _userService.GetCurrentUser();

            if ((decimal.Parse(expectedAmount) - currentUser.CurrentAccountAmount <= 0))
            {
                return Json((decimal)0);
            }

            var oneTimeAmount = await OneTimeAmount(decimal.Parse(expectedAmount), duration, frequency);

            if (Decimal.Round(oneTimeAmount) <= 0)
            {
                return Json((decimal)0);
            }

            return Json(oneTimeAmount);
        }

        private async Task<decimal> OneTimeAmount(decimal expectedAmount, int duration, int frequency)
        {
            var currentUser = await _userService.GetCurrentUser();
            var interestRate = await _investmentTargetService.GetInterestRate(currentUser.KVRR.Id, (Duration)duration);

            var rt = (double)(1 + (double)interestRate / duration);
            var d = (double)duration;

            if((decimal)(1 - (Math.Pow(rt, -d))) == 0 || (decimal)(Math.Pow(rt, d)) == 0)
            {
                return 0;
            }

            var oneTimeAmount = ((interestRate / duration) * (expectedAmount / (decimal)(Math.Pow(rt, d)) - currentUser.CurrentAccountAmount) / (decimal)(1 - (Math.Pow(rt, -d)))) / frequency;
            return oneTimeAmount;
        }
    }
}
