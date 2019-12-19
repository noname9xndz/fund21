import Global from '../global/global';
import DatatableLanguages from '../datatables/datatables-language';

let transactionsTablePageIndex = 1;
const transactionHistory = {
  initFunction: () => {
    transactionHistory.initTableTransactions();
    transactionHistory.initSearchButton();
    transactionHistory.initPageSizeForTransactionTable();
  },
  initTableTransactions: (index, size) => {
    const spinner = document.querySelector('.transaction-history__table .transaction-history__table__overlay');
    if (spinner) {
      spinner.classList.remove('hide');
    }
    const pageIndex = index || 1;
    const pageSize = size || 10;
    transactionsTablePageIndex = Number(pageIndex);
    // count = 0;
    const apiHolder = document.querySelector('#manageTransactionInputHidden');
    const transactionHistoryWrapper = document.querySelector('.transaction-history');
    const customerID = document.querySelector('#customerID');
    if (apiHolder && transactionHistoryWrapper) {
      const dataTableTransaction = document.querySelector('.transaction-history__table--wrapper');
      const fullNameHolder = document.querySelector('.transaction-history__sort-form input[name=CustomerName]');
      const phoneHolder = document.querySelector('.transaction-history__sort-form input[name=PhoneNumber]');
      const emailHolder = document.querySelector('.transaction-history__sort-form input[name=EmailAddress]');
      const statusHolder = document.querySelector('.transaction-history__sort-form select[name=Status]');
      const transactionTypeHolder = document.querySelector('.transaction-history__sort-form select[name=TransactionType]');
      const fromMoney = document.querySelector('.transaction-history__sort-form input[name=AmountFrom]');
      const toMoney = document.querySelector('.transaction-history__sort-form input[name=AmountTo]');
      const fromDate = document.querySelector('.transaction-history__sort-form input[name="TransactionDateFrom"]');
      const toDate = document.querySelector('.transaction-history__sort-form input[name="TransactionDateTo"]');
      let apiGetTransaction = `${apiHolder.value}`;
      if (transactionHistoryWrapper.classList.contains('transaction-history__customer-details')) {
        apiGetTransaction = `${apiHolder.value}?pageSize=${pageSize}&pageIndex=${pageIndex}&customerId=${customerID.value}&type=${transactionTypeHolder ? transactionTypeHolder.value : ''}&status=${statusHolder ? statusHolder.value : ''}&transactionDateFrom=${fromDate ? fromDate.value : ''}&transactionDateTo=${toDate ? toDate.value : ''}`;
      } else {
        apiGetTransaction = `${apiHolder.value}?pageSize=${pageSize}&pageIndex=${pageIndex}&type=${transactionTypeHolder ? transactionTypeHolder.value : ''}&status=${statusHolder ? statusHolder.value : ''}&transactionDateFrom=${fromDate ? fromDate.value : ''}&transactionDateTo=${toDate ? toDate.value : ''}`;
      }
      const model = {
        CustomerName: fullNameHolder ? fullNameHolder.value : '',
        PhoneNumber: phoneHolder ? phoneHolder.value : '',
        EmailAddress: emailHolder ? emailHolder.value : '',
        Status: statusHolder ? statusHolder.value : 0,
        TransactionType: transactionTypeHolder ? transactionTypeHolder.value : 0,
        AmountFrom: fromMoney ? transactionHistory.changeToNumber(fromMoney) : '',
        AmountTo: toMoney ? transactionHistory.changeToNumber(toMoney) : '',
        TransactionDateFrom: fromDate ? fromDate.value : '',
        TransactionDateTo: toDate ? toDate.value : ''
      };
      Global.getDataFromUrlPost(apiGetTransaction, model).then((data) => {
        const transactionInnerHTML = data;
        spinner.classList.add('hide');
        if (data && data.length > 0) {
          dataTableTransaction.innerHTML = transactionInnerHTML;
          const tableElm = document.querySelector('.transaction-history .transaction-history__table table');
          if (tableElm) {
            if (transactionHistoryWrapper.classList.contains('transaction-history__customer-details')) {
              const table = $(tableElm).DataTable({
                info: false,
                paging: false,
                searching: false,
                order: false,
                sort: false,
                language: DatatableLanguages,
                columnDefs: [{
                  targets: 1,
                  orderable: false
                }],
                drawCallback: () => {
                  // Global.hidePagingIfOnepage(tableElm);
                  Global.initAllCheckBox(tableElm);
                }
              });
            } else {
              const table = $(tableElm).DataTable({
                info: false,
                paging: false,
                searching: false,
                sort: false,
                language: DatatableLanguages,
                drawCallback: () => {
                  // Global.hidePagingIfOnepage(tableElm);
                }
              });
            }
          }
          transactionHistory.initPageinatorForTransactionTable();
        }
      });
    }
  },
  initSearchButton() {
    const btnSearch = document.querySelector('.transaction-history #transaction-history-form .btn-part button');
    const message = document.querySelector('#transaction-history-form .form-group span');
    if (btnSearch) {
      btnSearch.addEventListener('click', () => {
        const dropDownPageSize = document.querySelector('.transaction-history__sort-form-actions .transaction-history__sort-form-actions__pagesize__select');
        if (message) {
          const fromMoney = document.querySelector('.transaction-history__sort-form input[name=AmountFrom]');
          const toMoney = document.querySelector('.transaction-history__sort-form input[name=AmountTo]');
          const moneyFrom = transactionHistory.changeToNumber(fromMoney);
          const moneyTo = transactionHistory.changeToNumber(toMoney);
          if (moneyFrom > moneyTo) {
            message.style.display = 'block';
          } else {
            message.style.display = 'none';
            transactionHistory.initTableTransactions(1, dropDownPageSize.value);
          }
        } else {
          transactionHistory.initTableTransactions(1, dropDownPageSize.value);
        }
      });
    }
  },
  initPageSizeForTransactionTable() {
    const dropDownPageSize = document.querySelector('.transaction-history__sort-form-actions .transaction-history__sort-form-actions__pagesize__select');
    if (dropDownPageSize) {
      dropDownPageSize.addEventListener('change', () => {
        transactionHistory.initTableTransactions(1, dropDownPageSize.value);
      });
    }
  },
  initPageinatorForTransactionTable() {
    const totalPage = document.querySelector('.transaction-history .totalPage');
    let paginators = document.querySelectorAll('.transaction-history .paging_simple_numbers .paginate_button');
    const dropDownPageSize = document.querySelector('.transaction-history__sort-form-actions .transaction-history__sort-form-actions__pagesize__select');
    const transactionHistoryWrapper = document.querySelector('.transaction-history');
    let pagesize = 10;
    if (paginators) {
      paginators = Array.from(paginators);
      paginators.forEach((btn) => {
        const $self = btn;
        const tempClassList = Array.from($self.classList);
        btn.addEventListener('click', () => {
          if (transactionHistoryWrapper && !transactionHistoryWrapper.classList.contains('transaction-history__customer-details')) {
            if (dropDownPageSize) {
              pagesize = dropDownPageSize.value;
            }
          }
          if (tempClassList.indexOf('previous') > -1) {
            if (transactionsTablePageIndex > 1) {
              transactionHistory.initTableTransactions(transactionsTablePageIndex - 1, pagesize);
            }
          } else if (tempClassList.indexOf('next') > -1) {
            if (transactionsTablePageIndex < totalPage.value) {
              transactionHistory.initTableTransactions(transactionsTablePageIndex + 1, pagesize);
            }
          } else {
            transactionHistory.initTableTransactions($self.innerText, pagesize);
          }
        });
      });
    }
  },
  changeToNumber(ele) {
    const fromMoney = ele.value.split(',').join('');
    return parseInt(fromMoney, 10);
  }
};

export default transactionHistory;
