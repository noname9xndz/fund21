import Global from '../global/global';

// eslint-disable-next-line no-unused-vars
let transactionsTablePageIndex = 1;
const userTransactionHistory = {
  initSelectOption: () => {
    const dropDownPageSize = document.querySelector('.user-transaction-history-pagesize .table-filter-actions__pagesize__select');
    $('.user-transaction-history-filters__items__input select.input-form-control').selectmenu({
      appendTo: '.body-content-endUser',
      create: () => {
        $('.ui-selectmenu-button').addClass('input-form-control');
      }
    });
    $('#user-transaction-history-pagesize-select').selectmenu({
      appendTo: '.user-transaction-history-pagesize',
      width: 80,
      create: () => {
        if (dropDownPageSize) {
          userTransactionHistory.initTableTransactions(1, dropDownPageSize.value);
        }
      },
      select: () => {
        if (dropDownPageSize) {
          userTransactionHistory.initTableTransactions(1, dropDownPageSize.value);
        }
      }
    });
  },
  initDatepicker: () => {
    Global.initDateRangePicker('.user-transaction-history-from', '.user-transaction-history-to');
  },
  initTableTransactions: (index, size) => {
    const spinner = document.querySelector('.user-transaction-history-table .user-transaction-history-table__overlay');
    if (spinner) {
      spinner.classList.remove('hide');
    }
    const pageIndex = index || 1;
    const pageSize = size || 10;
    transactionsTablePageIndex = Number(pageIndex);
    // count = 0;
    const apiHolder = document.querySelector('#usermanageTransactionInputHidden');
    if (apiHolder) {
      const dataTableCustomer = document.querySelector('.user-transition-history-form--wrapper');
      const transactionDateFrom = document.querySelector('.user-transaction-history-from');
      const transactionDateTo = document.querySelector('.user-transaction-history-to');
      const type = document.querySelector('.user-transaction-history-type');
      const status = document.querySelector('.user-transaction-history-status');
      const model = {
        transactionDateFrom: transactionDateFrom ? transactionDateFrom.value : '',
        transactionDateTo: transactionDateTo ? transactionDateTo.value : '',
        type: type ? type.value : '',
        status: status ? status.value : 'None'
      };
      // eslint-disable-next-line no-use-before-define
      const apiGetTransaction = `${apiHolder.value}?type=${type ? type.value : ''}&status=${status ? status.value : ''}&transactionDateFrom=${transactionDateFrom ? transactionDateFrom.value : ''}&transactionDateTo=${transactionDateTo ? transactionDateTo.value : ''}&pageIndex=${pageIndex}&pageSize=${pageSize}`;
      Global.getDataFromUrlPost(apiGetTransaction, model).then((data) => {
        const customerInnerHTML = data;
        spinner.classList.add('hide');
        if (data && data.length > 0) {
          dataTableCustomer.innerHTML = customerInnerHTML;
          const tableElm = document.querySelector('#user-transition-history');
          if (tableElm) {
            const table = $(tableElm).DataTable({
              info: false,
              paging: false,
              searching: false,
              columnDefs: [{
                targets: 1,
                orderable: true
              }],
              ordering: false,
              language: {
                emptyTable: 'Không có dữ liệu'
              },
              drawCallback: () => {
                // Global.hidePagingIfOnepage(tableElm);
              }
            });
          }
          userTransactionHistory.initPageinatorForCustomerTable();
        }
      });
    }
  },
  initPageinatorForCustomerTable() {
    const totalPage = document.querySelector('.user-transaction-history .totalPage');
    let paginators = document.querySelectorAll('.user-transaction-history-table .paging_simple_numbers .paginate_button');
    const dropDownPageSize = document.querySelector('.user-transaction-history-pagesize .table-filter-actions__pagesize__select');
    if (paginators && dropDownPageSize) {
      paginators = Array.from(paginators);
      paginators.forEach((btn) => {
        const $self = btn;
        const tempClassList = Array.from($self.classList);
        btn.addEventListener('click', () => {
          if (tempClassList.indexOf('previous') > -1) {
            // eslint-disable-next-line no-undef
            if (transactionsTablePageIndex > 1) {
              // eslint-disable-next-line no-undef
              userTransactionHistory.initTableTransactions(transactionsTablePageIndex - 1, dropDownPageSize.value);
            }
          } else if (tempClassList.indexOf('next') > -1) {
            // eslint-disable-next-line no-undef
            if (transactionsTablePageIndex < totalPage.value) {
              // eslint-disable-next-line no-undef
              userTransactionHistory.initTableTransactions(transactionsTablePageIndex + 1, dropDownPageSize.value);
            }
          } else {
            userTransactionHistory.initTableTransactions($self.innerText, dropDownPageSize.value);
          }
        });
      });
    }
  },
  initPageSizeForCustomerTable() {
    const dropDownPageSize = document.querySelector('.user-transaction-history-pagesize .table-filter-actions__pagesize__select');
    if (dropDownPageSize) {
      dropDownPageSize.addEventListener('change', () => {
        userTransactionHistory.initTableTransactions(1, dropDownPageSize.value);
      });
    }
  },
  initSearchButton() {
    const btnSearch = document.querySelector('.user-transaction-history-filters__items .btn');
    const dropDownPageSize = document.querySelector('.user-transaction-history-pagesize .table-filter-actions__pagesize__select');
    if (btnSearch && dropDownPageSize) {
      btnSearch.addEventListener('click', () => {
        userTransactionHistory.initTableTransactions(1, dropDownPageSize.value);
      });
    }
  }
};
export default userTransactionHistory;
