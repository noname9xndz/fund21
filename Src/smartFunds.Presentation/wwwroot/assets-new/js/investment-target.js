// INFO : screen investor target
let expectedMoney = $('.user-expected-amount');
let duration = $('.user-period-invesment');
let frequency = $('.user-frequency-invesment');
let amountMoneyOne = $('#user-expected-amount-one');
(function ($) {

    'use strict';

    $.InvestmentTarget = {
        init: function () {
            onChangeExpectMoney();
            onChangeDurationFrequency();
        }
    };
  

    function onChangeExpectMoney() {
        if (expectedMoney.length) {
            expectedMoney.on('change', (e) => {
                callApiChangeExpectMoney();
            })
        }
    }

    function onChangeDurationFrequency() {
        if (expectedMoney.length) {
            $('.user-period-invesment , .user-frequency-invesment').on('change', (e) => {
                callApiChangeExpectMoney();
            })
        }
    }

    function callApiChangeExpectMoney() {
        let expectValue = expectedMoney.val().split(",").join("");
        //console.log(expectValue);
        let durationValue = duration.val();
        let frequencyValue = frequency.val();
        if (expectValue == '') {
            expectValue = 0;
        }
        $.ajax({
            method: 'POST',
            url: "/investment-target/GetOneTimeAmount?expectedAmount=" + expectValue + "&duration=" + durationValue + "&frequency=" + frequencyValue
        }).done(res => {
            amountMoneyOne.val(numeral(res).format('0,0'));
        })
    }

    $.InvestmentTarget.init();
})(jQuery);

  
   


