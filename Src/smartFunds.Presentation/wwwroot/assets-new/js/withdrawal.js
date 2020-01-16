const amountMoney = $('.form-withdrawal .user-expected-money-amount input');
const type = $('.form-withdrawal .type-withdrawal');
const costWithdrawal = $('.form-withdrawal .cost-withdrawal');
const checkboxGetAll = $('.form-withdrawal .input-form-checkbox input');
let typeGetAll = false;
const messageWithdrawalSlow = $('.form-withdrawal #message-withdrawal-slow');
const messageWithdrawalFast = $('.form-withdrawal #message-withdrawal-fast');
let isSubmitted = false;
(function ($) {

    'use strict';

    $.Withdrawal = {
        init: function () {
            messageWithdrawalSlow.css("display", "none");
            onChangeAmountMoney();
            onChangeType();
            onChangeCheckbox();
            loadingSubmitFormInvestment();
            loadingSubmitFormWithdrawal();
        }   
       
    };


    function onChangeAmountMoney() {
        amountMoney.on("change", (e) => {
            callApiChangeWitdrawal();
        })

    }

     function onChangeType() {
         type.on("change", (e) => {
             if (type.val() == 0) {
                 messageWithdrawalFast.css("display", "block");
                 messageWithdrawalSlow.css("display", "none");
             } else {
                 messageWithdrawalSlow.css("display", "block");
                 messageWithdrawalFast.css("display", "none");
             }
            if (typeGetAll) {
                 callApiGetAllMoney();

            } else {
                callApiChangeWitdrawal()
            }
        })
        
     }

    function onChangeCheckbox() {
        checkboxGetAll.change((event) => {
            type.val('0');
            messageWithdrawalFast.css("display", "block");
            messageWithdrawalSlow.css("display", "none");
            const this$ = event.currentTarget;
            if ($(this$).is(':checked')) {
                typeGetAll = true;
                callApiGetAllMoney();
            } else {
                typeGetAll = false;
                amountMoney.val('');
                amountMoney.css('pointer-events', 'initial');
                costWithdrawal.val('');
            }
        });

    }
    function callApiChangeWitdrawal() {
        const amountMoneyValue = amountMoney.val();
        const typeValue = type.val();
        const costWithdrawalValue = costWithdrawal.val();

        if (amountMoneyValue == '') {
            amountMoneyValue = 0;
        }

        $.ajax({
            method: 'POST',
            url: "/withdrawal/GetWithdrawalFee?amount=" + amountMoneyValue + "&type=" + typeValue
        }).done(res => {
            costWithdrawal.val((numeral(res).format('0,0')));
        });
    }

    function callApiGetAllMoney() {
        const typeValue = type.val();
        $.ajax({
            method: 'POST',
            url: `/withdrawal/WithdrawalAll?type=${typeValue}`,
        }).done(res => {
            amountMoney.val((numeral(res.amount).format('0,0')));
            amountMoney.css('pointer-events', 'none');
            costWithdrawal.val((numeral(res.fee).format('0,0')));
        })
    }

    function loadingSubmitFormInvestment() {
        const submitForm = $('.user-withdrawal-invest form');
        const imgLoad = $('.user-withdrawal-invest #pageloader');
        const btnSubmit = $('.user-withdrawal-invest .btn-actions');
        if (submitForm.length && imgLoad.length && btnSubmit.length) {
            submitForm.on('submit', () => {
                if (!isSubmitted) {
                    imgLoad.css('display', 'block');
                    setTimeout(() => {
                        imgLoad.css('display', 'none');
                    }, 2000);
                    isSubmitted = true;
                }
            });
        }
    }

    function loadingSubmitFormWithdrawal() {
        const submitForm = $('.user-withdrawal-main form');
        const imgLoad = $('.user-withdrawal-main #pageloader');
        const btnSubmit = $('.user-withdrawal-main .btn-actions');

        if (submitForm.length && imgLoad.length && btnSubmit.length) {
            submitForm.on('submit', (e) => {
                if (document.querySelector('.g-recaptcha-response').value) {
                    if (!isSubmitted) {
                        imgLoad.css('display', 'block');
                        isSubmitted = true;
                    }
                }
                else {
                    e.preventDefault();
                    document.querySelector('#captcha-valid-message').innerText = "Vui lòng nhập captcha";
                }

            });
        }
    }
    $.Withdrawal.init();
})(jQuery);





