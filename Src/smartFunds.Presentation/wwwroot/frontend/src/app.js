/* eslint-disable func-names */
import Global from './views/components/global/global';
import Header from './views/components/header/header';
import Footer from './views/components/footer/footer';
import Signin from './views/components/signin/signin';
import InvestmentPortfolio from './views/components/investment-portfolio/investment-portfolio';
import InvesmentDetail from './views/components/investment-portfolio-detail/investment-portfolio-detail';
import manageRiskDetail from './views/components/manage-risk/manage-riskDetail/manage-riskDetail';
import transactionHistory from './views/components/transaction-history/transaction-history';
import Resgister from './views/components/resgisterUser/resgisterUser';
import CustomerShowChart from './views/components/customer-chart/customer-chart';
import manageInvestFund from './views/components/manage-InvestFund/manage-InvestFund';
import Validate from './views/components/user_login/user_login';
import ManagerRiskQuestions from './views/components/manager-risk__questions/manager-risk__questions';
import userResgisterOTP from './views/components/user-resgister-otp/user-resgister-otp';
import FooterUser from './views/components/footerUser/footerUser';
import Wallet from './views/components/user-wallet/user-wallet';
import InvestmentFunds from './views/components/investment-funds/investment-funds';
import weightRisk from './views/components/manage-weightRisk/manage-weightRisk';
import ManagerRiskQuestionsAdd from './views/components/manager-risk__questions__data/manager-risk__questions__data';
import ManageUser from './views/components/manage-user/manage-user';
import userInvestmentPortfolio from './views/components/user-investment-portfolio/user-investment-portfolio';
import userQuestion from './views/components/user-questions/user-questions';
import ManageCustomers from './views/components/manager-customers/manager-customers';
import CustomerTransactionHistory from './views/components/customer-transition-history/customer-transition-history';
import Slide from './views/components/user-hp-slide/user-hp-slide';
import SlidePartner from './views/components/user-hp-partner/user-hp-partner';
import manageRisk from './views/components/manage-risk/manage-risk';
import UserProfileShowChart from './views/components/user-profile__chart/user-profile__chart';
import invesmentTarget from './views/components/user-invesments-target-create/user-invesments-target-create';
import userWithDrawal from './views/components/user-withdrawal/user-withdrawal';
import UserProfileInfo from './views/components/user-profile__info/user-profile__info';
import ContactPage from './views/components/user-contact/user-contact';
// Admin Phrase 2

import clientDistribution from './views/components/client-distribution/client-distribution';
import AddclientDistribution from './views/components/add-client-distribution/add-client-distribution';
import myAccountEditPass from './views/components/myAccountEditPass/myAccountEditPass';
import ResetPass from './views/components/reset-pass/reset-pass';
import myMissionAccountant from './views/components/myMission-accountant/myMission-accountant';
import TradeBalance from './views/components/trade-balance/trade-balance-list/trade-balance-list';
import TradeBalanceDetail from './views/components/trade-balance/trade-balance-detail/trade-balance-detail';
import userFAQ from './views/components/user-faq/user-faq';
import userTransactionHistory from './views/components/user-transaction-history/user-transaction-history';
import ManagerFrequentQuestion from './views/components/manager-frequent-question/manager-frequent-question-list/manager-frequent-question-list';
import ManagerFrequentQuestionAdd from './views/components/manager-frequent-question/manager-frequent-question-add/manager-frequent-question-add';
import ManagerFrequentQuestionCategory from './views/components/manager-frequent-question/manager-frequent-question-category/manager-frequent-question-category';
import ManagerSetting from './views/components/manager-settings/manager-settings';
import ManagerSettingContact from './views/components/manager-settings/manager-settings-contact/manager-settings-contact';
import ManagerSettingIntroduce from './views/components/manager-settings/manager-settings-introduce/manager-settings-introduce';
import myMissionAdmin from './views/components/myMission-admin/myMission-admin';
import HideLongText from './views/components/user-fund/user-fund';
import ManagerCheckInvest from './views/components/manager-check-invest/manager-check-invest';
// CSS
require('normalize.css');
require('slick-carousel/slick/slick.scss');
require('slick-carousel/slick/slick-theme.scss');
require('jquery-modal/jquery.modal.min.css');
require('datatables.net-dt/css/jquery.dataTables.min.css');
require('@fortawesome/fontawesome-free/css/all.css');
require('perfect-scrollbar/css/perfect-scrollbar.css');
// JS
require('slick-carousel/slick/slick');
require('jquery-modal/jquery.modal.min.js');
require('./views/components/datatables/datatables');
require('jquery-validation');
require('./views/components/datatables/jquery-steps');
require('./assets/js/jquery-ui/jquery-ui');
require('lodash');
require('numeraljs');

if (window.NodeList && !NodeList.prototype.forEach) {
  NodeList.prototype.forEach = function (callback, thisArg) {
    thisArg = thisArg || window;
    for (let i = 0; i < this.length; i++) {
      callback.call(thisArg, this[i], i, this);
    }
  };
}

// Polyfill for Cloest

if (!Element.prototype.matches) {
  Element.prototype.matches = Element.prototype.msMatchesSelector
    || Element.prototype.webkitMatchesSelector;
}

if (!Element.prototype.closest) {
  Element.prototype.closest = function (s) {
    let el = this;

    do {
      if (el.matches(s)) return el;
      el = el.parentElement || el.parentNode;
    } while (el !== null && el.nodeType === 1);
    return null;
  };
}

// ----------------------------------- //

$(() => {
  // Code here
  ManageUser.initDatatable();
  Global.uploadPreviewImage();
  Global.initMicroModal();
  console.log(document.querySelector('a[data-micromodal-trigger="delete-purchase-1"]'));
});


document.addEventListener('DOMContentLoaded', () => {
  Global.animatedOnScroll();
  Global.initBunnyValidationFormConfig();
  InvestmentFunds.initDatatable();
  InvestmentPortfolio.initDatatable();
  userQuestion.initJquerySteps();
  userQuestion.changeButtonColorOnInputClick();
});

window.addEventListener('load', () => {
  Footer.submitFormSubscribe();
});

window.onload = () => {
  Resgister.toogleViewPass();
  Resgister.validate();
  Resgister.submitForm();
  ManageCustomers.initSearchButton();
  userInvestmentPortfolio.invesmentChecked();
  userInvestmentPortfolio.sliderInvsement();
  userInvestmentPortfolio.modalDetailInvesment();
  UserProfileShowChart.initChartProfileData();
  userFAQ.initPagination();
  userFAQ.initPagesizeOption();
  userTransactionHistory.initTableTransactions();
  userTransactionHistory.initSearchButton();
  userTransactionHistory.initDatepicker();
  userTransactionHistory.initSelectOption();
  invesmentTarget.initSelectDropdown();
  Global.initSuggestionMoney();
  userQuestion.detectInputChecked();
  HideLongText.hiddenLongText();
  userWithDrawal.loadingSubmitForm();
  invesmentTarget.loadingSubmitFormTarget();
  userInvestmentPortfolio.loadingSubmitKVRR();
  ContactPage.loadingSubmitForm();
};

window.addEventListener('resize', () => {
  clearTimeout(window.resizedFinished);
  window.resizedFinished = setTimeout(() => {
    Validate.hideHeaderFooterOnDesktop();
    userResgisterOTP.setHeightOTP();
    Wallet.setHeight();
    userInvestmentPortfolio.sliderInvsement();
    userInvestmentPortfolio.invesmentChecked();
    UserProfileShowChart.initChartProfileData();
  }, 250);
});

window.addEventListener('scroll', () => {
  Global.animatedOnScroll();
});

$(document).ready(() => {
  Signin.initFunction();
  Global.initDateRangePicker('#date-range-from', '#date-range-to');
  Global.initDateRangePicker('#property-fluctuations-from', '#property-fluctuations-to');
  Global.inputValid();
  Global.inputInvesmentFund();
  Global.inputFloatNumber();
  Global.inputValidPhone();
  Global.inputDeleteSpace();
  Global.initDateRangePicker('#TransactionDateFrom', '#TransactionDateTo');
  Global.initClickEditTableDirectly();
  Global.getFile();
  Validate.showPasswordFormLogin();
  Validate.checkLengthInputToHidePlaceholder();
  transactionHistory.initFunction();
  Header.toogleMenu();
  InvesmentDetail.initTable();
  InvesmentDetail.checkTableMess();
  InvestmentFunds.initFunction();
  weightRisk.initFunction();
  ManagerRiskQuestions.initFunction();
  manageRiskDetail.Edit();
  Resgister.toogleClickInput();
  CustomerShowChart.initChartCustomerData();
  manageInvestFund.initFunction();
  Global.addClassCurrentHeader('.wallet_page', '#wallet');
  Global.addClassCurrentHeader('.investment_portfolio_page', '#investment_portfolio');
  Global.showMobileHeaderUser();
  FooterUser.showItemFooter();
  Validate.hideHeaderFooterOnDesktop();
  userResgisterOTP.setHeightOTP();
  Wallet.setHeight();
  ManagerRiskQuestionsAdd.initFunction();
  CustomerTransactionHistory.initFunction();
  Slide.showSlideHomepageUser();
  SlidePartner.showSlidePartnerHomepageUser();
  userInvestmentPortfolio.sliderInvsement();
  ManageCustomers.initTableCustomer();
  ManageCustomers.initPageSizeForCustomerTable();
  ManageCustomers.initDateRange();
  manageRisk.initFunction();
  Header.checkToogle();
  Header.activeCMSDropDown();
  // Admin Phrase 2
  clientDistribution.initFunction();
  AddclientDistribution.initFunction();
  myAccountEditPass.initFunction();
  ResetPass.initFunction();
  UserProfileShowChart.showDate();
  myMissionAdmin.initFuction();
  // UserProfileShowChart.initChartProfileData();
  myMissionAccountant.initFuction();
  TradeBalance.initDatatable();
  TradeBalanceDetail.initDatatable();
  userFAQ.initPagesizeOption();
  ManagerFrequentQuestion.initFunction();
  ManagerFrequentQuestionAdd.initFunction();
  ManagerFrequentQuestionCategory.initFunction();
  ManagerSetting.initFunction();
  userWithDrawal.initSelectDropdown();
  ManagerSettingContact.initFunction();
  ManagerSettingIntroduce.initFunction();
  UserProfileInfo.modalEditInfo();
  UserProfileInfo.validateEditInfo();
  UserProfileInfo.editInfo();
  ContactPage.focusInput();
  InvesmentDetail.initFunction();
  UserProfileShowChart.initValueForInput();
  ManagerCheckInvest.initFunction();
  UserProfileInfo.showPopup();
  Global.showPopupMobile();
  userWithDrawal.checkBoxClick();
});
