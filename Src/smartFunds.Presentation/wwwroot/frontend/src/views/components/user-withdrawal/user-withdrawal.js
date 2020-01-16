import Global from '../global/global';

const checkboxGetAll = $('.user-withdrawal-invest .input-form-checkbox');
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
        if (!checkboxGetAll.is(':checked')) {
          Global.getDataFromUrlPost(apiGetTransaction, model).then((data) => {
            costWithdrawal.value = numeral(data).format('0,0');
          });
        }
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
            setTimeout(() => {
              imgLoad.css('display', 'none');
            }, 2000);
            isSubmitted = true;
            e.currentTarget.submit();
          }
        }
      });
    }
  },

  checkBoxClick: () => {
    const apiGetAllMoney = $('#get-all-money').val();
    const amountMoney = $('.user-expected-money-amount input');
    const costWithdrawal = $('.user-withdrawal-invest .cost-withdrawal');
    const type = $('.user-withdrawal-invest .type-withdrawal');
    const selectWidthdrawalType = $('.user-withdrawal-invest .type-withdrawal');
    selectWidthdrawalType.on('selectmenuchange', (event, ui) => {
      if (checkboxGetAll.is(':checked')) {
        Global.getDataFromUrlPost(`${apiGetAllMoney}?type=${type.val()}`).then((res) => {
          const { amount, fee } = JSON.parse(res);
          amountMoney.val(numeral(amount).format('0,0'));
          costWithdrawal.val(numeral(fee).format('0,0'));
        }).catch(err => new Error(err));
      }
    });

    checkboxGetAll.change((event) => {
      const $this = event.currentTarget;
      if ($($this).is(':checked')) {
        Global.getDataFromUrlPost(`${apiGetAllMoney}?type=${type.val()}`).then((res) => {
          const { amount, fee } = JSON.parse(res);
          amountMoney.val(numeral(amount).format('0,0'));
          costWithdrawal.val(numeral(fee).format('0,0'));
          amountMoney.css('pointer-events', 'none');
          costWithdrawal.css('pointer-events', 'none');
        }).catch(err => new Error(err));
      } else {
        amountMoney.val('');
        costWithdrawal.val('');
        amountMoney.css('pointer-events', 'initial');
        costWithdrawal.css('pointer-events', 'initial');
      }
    });
  }

};
export default userWithDrawal;
