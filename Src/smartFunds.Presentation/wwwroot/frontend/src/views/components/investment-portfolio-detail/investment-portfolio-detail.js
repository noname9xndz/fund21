import Global from '../global/global';

const tableMess = document.querySelector('#table-mess-valid span');
const tableElm = document.querySelector('#invesment-detail');

const invesmentDetail = {
  initFunction: () => {
    invesmentDetail.saveButton();
  },
  initTable: () => {
    if (tableElm) {
      Global.initDataTable(tableElm, {
        searching: false,
        info: false,
        paging: false,
        order: false,
        sort: false,
        columnDefs: [{
          targets: 1,
          orderable: false
        }],
        drawCallback: () => {
          Global.hidePagingIfOnepage(tableElm);
          Global.initAllCheckBox(tableElm);
        }
      });
    }
  },
  checkTableMess: () => {
    if (tableMess) {
      if (tableMess.textContent) {
        const inputTable = tableElm.querySelectorAll('tr td input');
        inputTable.forEach((item) => {
          item.removeAttribute('readonly');
        });
      }
    }
  },
  saveButton: () => {
    const saveButton = document.querySelector('.investment-detail__Save--button');
    // const table = document.querySelector('.investment-funds__table');
    if (saveButton) {
      saveButton.addEventListener('click', () => {
        // saveButton.setAttribute('disabled', 'disabled');
        document.querySelector('.investment-funds__table__overlay').classList.remove('hide');
        // table.querySelector('.edit-table-info').setAttribute('disabled', 'disabled');
      });
    }
  }
};

export default invesmentDetail;
