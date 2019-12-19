// eslint-disable-next-line import/no-cycle
import Global from '../global/global';

let isSubmitted = false;
const numeral = require('numeraljs');

const invesmentTarget = {
  initSelectDropdown: () => {
    Global.initSelectDropdown('.user-invesment-target select');
  },
  initSuggestionDropdown: () => {
    const apiHolder = document.querySelector('#user-expected-amount');
    const amountMoney = document.querySelector('.user-expected-money-amount input');
    const amountMoneyOne = document.getElementById('user-expected-amount-one');
    if (amountMoney.value.length === 0) {
      amountMoneyOne.value = ' ';
    }
    if (amountMoney.value.length > 0) {
      if (apiHolder) {
        const expectedAmount = document.querySelector('.user-invesment-target .form-control-number');
        const duration = document.querySelector('.user-invesment-target .user-period-invesment');
        const frequency = document.querySelector('.user-invesment-target .user-frequency-invesment');
        const model = {
          expectedAmount: expectedAmount ? numeral(expectedAmount.value).value() : '',
          duration: duration ? duration.value : '',
          frequency: frequency ? frequency.value : ''
        };
        // eslint-disable-next-line no-use-before-define
        const apiGetTransaction = `${apiHolder.value}?expectedAmount=${expectedAmount ? numeral(expectedAmount.value).value() : ''}&duration=${duration ? duration.value : ''}&frequency=${frequency ? frequency.value : ''}`;
        Global.getDataFromUrlPost(apiGetTransaction, model).then((data) => {
          amountMoneyOne.value = numeral(data).format('0,0');
        });
      }
    }
  },
  loadingSubmitFormTarget: () => {
    const submitForm = $('.user-invesment-target form');
    const imgLoad = $('.user-invesment-target #pageloader');
    const inputElm = $('.user-invesment-target form.user-form-group input');
    const btnSubmit = $('.user-invesment-target button.btn-actions');
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
export default invesmentTarget;
