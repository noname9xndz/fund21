(function ($) {
    const transactionStartDate = $('.transaction-history-main .transition-start-date');
    const transactionEndDate = $('.transaction-history-main .transition-end-date');
    const transactionType = $('.transaction-history-main .transaction-select-type');
    const transactionStatus = $('.transaction-history-main .transaction-select-status');
    const transactionButtonSearch = $('.transaction-history-main .transaction-button-search');
    const tableResponse = $('.transaction-history-main .transaction-table-response');
    const buttonPrevPage = $('.transaction-history-main .prev-page');
    const buttonNextPage = $('.transaction-history-main .next-page');

    let transactionStartDateValue;
    let transactionEndDateValue;
    let transactionTypeValue;
    let transactionStatusValue;
    let currentPage = 1;
    let totalPage;
    'use strict';
    $.TransactionHistory = {
        init: function () {
            if ($('.transaction-history-main').length) {
                onChangeDate();
                callApiSearchTransaction();
                onSearchTransaction();
                prevPage();
                nextPage();
            }
        }
    };
    function onChangeDate() {
        transactionStartDate.datepicker({
            dateFormat: 'dd/mm/yy',
            maxDate: 'today',
            onSelect: function () {
                let fullDate = $(this).datepicker('getDate').toLocaleDateString('en-SG');
                transactionStartDate.val(fullDate);
            }
        });
        transactionEndDate.datepicker({
            dateFormat: 'dd/mm/yy',
            maxDate: 'today',
            onSelect: function () {
                let fullDate = $(this).datepicker('getDate').toLocaleDateString('en-SG');
                transactionEndDate.val(fullDate);
            }
        });
    }
    function onSearchTransaction() {
        transactionButtonSearch.on('click', () => {
            currentPage = 1;
            callApiSearchTransaction();
        })
    }
    function callApiSearchTransaction() {
        let pageSize = 10;
        transactionStartDateValue = transactionStartDate.val() 
        transactionEndDateValue = transactionEndDate.val();
        transactionTypeValue = transactionType.val();
        transactionStatusValue = transactionStatus.val();
        $.ajax({
            method: 'POST',
            url: `/transaction-history/list?type=${transactionTypeValue}&status=${transactionStatusValue}&transactionDateFrom=${transactionStartDateValue}&transactionDateTo=${transactionEndDateValue}&pageIndex=${currentPage}&pageSize=${pageSize}`,
            contentType: "application/json; charset=utf-8",
        }).done(res => {
            tableResponse.empty();
            tableResponse.append(res);
            totalPage = Number($('.transaction-history-main .totalPage').val());
            checkStateNavPage();
        });
    }
    function checkStateNavPage() {
        if (currentPage == 1) {
            buttonPrevPage.css({
                'pointer-events': 'none',
                'opacity': '0.5'
            })
        } else {
            buttonPrevPage.css({
                'pointer-events': 'initial',
                'opacity': 'initial'
            })
        }

        if (currentPage == totalPage) {
            buttonNextPage.css({
                'pointer-events': 'none',
                'opacity': '0.5'
            })
        } else {
            buttonNextPage.css({
                'pointer-events': 'initial',
                'opacity': 'initial'
            })
        }

    }
    function prevPage() {
        buttonPrevPage.on('click', (event) => {
            if (currentPage == 1) {
                currentPage = 1
            } else {
                currentPage = currentPage - 1
            }
            callApiSearchTransaction();
        })

    }
    function nextPage() {
        buttonNextPage.on('click', (event) => {
            if (currentPage == totalPage) {
                currentPage = totalPage
            } else {
                currentPage = currentPage + 1;
            }
            callApiSearchTransaction();
        })
    }
    $.TransactionHistory.init();
})(jQuery);