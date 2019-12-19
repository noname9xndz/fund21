import Global from '../global/global';

let isSubmitted = false;
const numeral = require('numeraljs');

const userWithDrawal = {
  initSelectDropdown: () => {
    Global.initSelectDropdown('.user-withdrawal-invest select');
    $('.user-withdrawal-invest select').on('selectmenuselect', () => {
      const warningMessageFast = document.querySelector('#message-withdrawal-fast');
      const warningMessageSlow = document.querySelector('#message-withdrawal-slow');
      const withdrawalValue = $('.user-withdrawal-invest select').val();
      if (parseInt(withdrawalValue, 10) === 0) {
        warningMessageFast.classList.remove('d-none');
        warningMessageSlow.classList.add('d-none');
      } else {
        warningMessageFast.classList.add('d-none');
        warningMessageSlow.classList.remove('d-none');
      }
    });
  },
  initCostWithDrawal: () => {
    const apiHolder = document.querySelector('.user-withdrawal-invest .get-cost-withdrawal');
    const costWithdrawal = document.querySelector('.user-withdrawal-invest .cost-withdrawal');
    const amountMoney = document.querySelector('.user-expected-money-amount input');
    const type = document.querySelector('.user-withdrawal-invest .type-withdrawal');
    if (costWithdrawal && amountMoney) {
      if (amountMoney.value.length === 0) {
        costWithdrawal.value = ' ';
      }
    }

    if (amountMoney.value.length > 0) {
      if (apiHolder && costWithdrawal) {
        const model = {
          amountMoney: amountMoney ? numeral(amountMoney.value).value() : '',
          type: type ? type.value : ''
        };
        const apiGetTransaction = `${apiHolder.value}?amount=${amountMoney ? numeral(amountMoney.value).value() : ''}&type=${type ? type.value : ''}`;
        Global.getDataFromUrlPost(apiGetTransaction, model).then((data) => {
          costWithdrawal.value = numeral(data).format('0,0');
        });
      }
    }
  },
  loadingSubmitForm: () => {
    const submitForm = $('.user-withdrawal-invest form');
    const imgLoad = $('.user-withdrawal-invest #pageloader');
    const inputElm = $('.user-withdrawal-invest form.user-form-group input');
    const btnSubmit = $('.user-withdrawal-invest button.btn-actions');
    if (submitForm.length && imgLoad.length && inputElm.length && btnSubmit.length) {
      submitForm.on('submit', (e) => {
        e.preventDefault();
        if (!isSubmitted) {
          if (inputElm.hasClass('input-validation-error')) {
            imgLoad.css('display', 'none');
          } else {
            imgLoad.css('display', 'block');
            isSubmitted = true;
            e.currentTarget.submit();
          }
        }
      });
    }
  }
};
export default userWithDrawal;
