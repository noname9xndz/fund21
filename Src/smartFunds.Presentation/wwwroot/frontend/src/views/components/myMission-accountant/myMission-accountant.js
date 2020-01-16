import MicroModal from 'micromodal';
import Global from '../global/global';
import transactionHistory from '../transaction-history/transaction-history';

let transactionsTablePageIndex = 1;

const tableMission = document.querySelectorAll('.my-mission__item');


const myMissionAccountant = {
  initFuction: () => {
    myMissionAccountant.initTable();
  },
  initTable: () => {
    tableMission.forEach((item) => {
      if (item.dataset.apiLink) {
        myMissionAccountant.initTableUI(item);
      }
      const btnSearch = item.querySelector('.my-mission__item__form-sort__align-item .btn-action');
      const message = item.querySelector('.form-group span');
      if (btnSearch && message) {
        btnSearch.addEventListener('click', () => {
          const dropDownPageSize = item.querySelector('.my-mission__item__form-sort .table-filter-actions__pagesize__select');
          const fromMoney = item.querySelector('.my-mission-inputFrom');
          const toMoney = item.querySelector('.my-mission-inputTo');
          const moneyFrom = transactionHistory.changeToNumber(fromMoney);
          const moneyTo = transactionHistory.changeToNumber(toMoney);
          if (moneyFrom > moneyTo) {
            message.style.display = 'block';
          } else {
            message.style.display = 'none';
            myMissionAccountant.initTableUI(item, 1, dropDownPageSize.value);
          }
        });
      }

      const dropDownPageSize = item.querySelector('.my-mission__item__form-sort .table-filter-actions__pagesize__select');
      if (dropDownPageSize) {
        dropDownPageSize.addEventListener('change', () => {
          myMissionAccountant.initTableUI(item, 1, dropDownPageSize.value);
        });
      }
    });
  },
  initTableUI: (item, index, size) => {
    const spinner = item.querySelector('.table__overlay');
    if (spinner) {
      spinner.classList.remove('hide');
    }
    const pageIndex = index || 1;
    const pageSize = size || 20;
    transactionsTablePageIndex = Number(pageIndex);
    const apiHolder = item.dataset.apiLink;
    if (apiHolder) {
      const dataTableTransaction = item.querySelector('.table__wrapper');
      const customerName = item.querySelector('.my-mission__item__form-sort .form-control[name=CustomerName]');
      const fromMoney = item.querySelector('.my-mission__item__form-sort .form-control[name=AmountFrom]');
      const toMoney = item.querySelector('.my-mission__item__form-sort .form-control[name=AmountTo]');
      const apiGetTransaction = `${apiHolder}?pageSize=${pageSize}&pageIndex=${pageIndex}`;
      const model = {
        CustomerName: customerName ? customerName.value : '',
        AmountFrom: fromMoney ? transactionHistory.changeToNumber(fromMoney) : '',
        AmountTo: toMoney ? transactionHistory.changeToNumber(toMoney) : ''
      };
      Global.getDataFromUrlPost(apiGetTransaction, model).then((data) => {
        const transactionInnerHTML = data;
        if (data && data.length > 0) {
          dataTableTransaction.innerHTML = transactionInnerHTML;
          spinner.classList.add('hide');
          const tableElm = item.querySelector('.table__wrapper table');
          if (tableElm) {
            const table = $(tableElm).DataTable({
              info: false,
              paging: false,
              lengthChange: false,
              searching: false,
              order: false,
              sort: false,
              columnDefs: [{
                targets: 1,
                orderable: false
              }],
              drawCallback: () => {
                // Global.hidePagingIfOnepage(tableElm);
              }
            });
          }
          Global.initMicroModal();
          // Global.initClickPaging(item);
          myMissionAccountant.onUpdate(item);
          myMissionAccountant.initPaginator(item);
        }
      });
    }
  },
  onUpdate: (item) => {
    const updatePart = item.querySelectorAll('.confirm-popup');
    if (updatePart.length > 0) {
      updatePart.forEach((popup) => {
        const btnUpdate = popup.querySelector('button[type="button"]');
        const updateLink = popup.querySelector('input[type="hidden"]');
        var firstTime = true;
        btnUpdate.addEventListener('click', () => {
          if (firstTime) {
            firstTime = false;
            Global.getDataFromAjaxCall(updateLink.value).then((data) => {
              if (data.success) {
                myMissionAccountant.initTable();
              } else {
                alert(data.message);
              }
            });
          }
        });
      });
    }
  },
  initPaginator: (item) => {
    const totalPage = item.querySelector('.totalPage');
    const dropDownPageSize = item.querySelector('.my-mission__item__form-sort .table-filter-actions__pagesize__select');
    if (dropDownPageSize) {
      dropDownPageSize.addEventListener('change', () => {
        myMissionAccountant.initTableUI(item, 1, dropDownPageSize.value);
      });
    }
    let paginators = item.querySelectorAll('.paging_simple_numbers .paginate_button');
    // console.log('paginatorssss1', paginators);
    if (paginators) {
      paginators = Array.from(paginators);
      paginators.forEach((btn) => {
        const $self = btn;
        const tempClassList = Array.from($self.classList);
        btn.addEventListener('click', () => {
          if (tempClassList.indexOf('previous') > -1) {
            if (transactionsTablePageIndex > 1) {
              myMissionAccountant.initTableUI(item, transactionsTablePageIndex - 1, dropDownPageSize.value);
            }
          } else if (tempClassList.indexOf('next') > -1) {
            if (transactionsTablePageIndex < totalPage.value) {
              myMissionAccountant.initTableUI(item, transactionsTablePageIndex + 1, dropDownPageSize.value);
            }
          } else {
            myMissionAccountant.initTableUI(item, $self.innerText, dropDownPageSize.value);
          }
        });
      });
    }
  }
};

export default myMissionAccountant;
