import Global from '../global/global';

let customersTablePageIndex = 1;
const ManageCustomers = {
  initTableCustomer(index, size) {
    const spinner = document.querySelector('.table-manage-customers .table-manage-customers__overlay');
    if (spinner) {
      spinner.classList.remove('hide');
    }
    const pageIndex = index || 1;
    const pageSize = size || 10;
    customersTablePageIndex = Number(pageIndex);
    // count = 0;
    const apiHolder = document.querySelector('#manageCustomersInputHidden');
    if (apiHolder) {
      const dataTableCustomer = document.querySelector('.table-manage-customers--wrapper');
      const apiGetCustomer = `${apiHolder.value}?pageSize=${pageSize}&pageIndex=${pageIndex}`;
      const fullNameHolder = document.querySelector('.table-filter-conditions .table-filter-conditions__item__input__type--name');
      const phoneHolder = document.querySelector('.table-filter-conditions .table-filter-conditions__item__input__type--phone');
      const emailHolder = document.querySelector('.table-filter-conditions .table-filter-conditions__item__input__type--email');
      const dateHolder = document.querySelector('.table-filter-conditions .table-filter-conditions__item__input__type--date');
      const statusHolder = document.querySelector('.table-filter-conditions .table-filter-conditions__item__input__type--status');
      const model = {
        FullName: fullNameHolder ? fullNameHolder.value : '',
        PhoneNumber: phoneHolder ? phoneHolder.value : '',
        Email: emailHolder ? emailHolder.value : '',
        CreatedDate: dateHolder ? dateHolder.value : '',
        ActiveStatus: statusHolder ? statusHolder.value : 'None'
      };
      Global.getDataFromUrlPost(apiGetCustomer, model).then((data) => {
        const customerInnerHTML = data;
        spinner.classList.add('hide');
        if (data && data.length > 0) {
          dataTableCustomer.innerHTML = customerInnerHTML;
          const tableElm = document.querySelector('#manageCustomers');
          if (tableElm) {
            const table = $(tableElm).DataTable({
              info: false,
              paging: false,
              searching: false,
              sort: false,
              columnDefs: [{
                targets: 1,
                orderable: false
              }],
              drawCallback: () => {
                // Global.hidePagingIfOnepage(tableElm);
                Global.initAllCheckBox(tableElm);
              }
            });
          }
          ManageCustomers.initPageinatorForCustomerTable();
          const totalCustomer = document.querySelector('#totalCustomer');
          if (totalCustomer) {
            const showTotalCustomer = document.querySelector('.manager-customers__TotalCount span');
            showTotalCustomer.textContent = `${totalCustomer.value} `;
          }
        }
      });
    }
  },
  initDateRange: () => {
    Global.initDatePicker('.table-filter-conditions .table-filter-conditions__item__input__type--date');
  },
  initSearchButton() {
    const btnSearch = document.querySelector('.table-filter-conditions .table-filter-conditions__item .buttons-search');
    const dropDownPageSize = document.querySelector('.table-filter-actions .table-filter-actions__pagesize__select');
    if (btnSearch && dropDownPageSize) {
      btnSearch.addEventListener('click', () => {
        ManageCustomers.initTableCustomer(1, dropDownPageSize.value);
      });
    }
  },
  initPageSizeForCustomerTable() {
    const dropDownPageSize = document.querySelector('.table-filter-actions .table-filter-actions__pagesize__select');
    if (dropDownPageSize) {
      dropDownPageSize.addEventListener('change', () => {
        ManageCustomers.initTableCustomer(1, dropDownPageSize.value);
      });
    }
  },
  initPageinatorForCustomerTable() {
    const totalPage = document.querySelector('.manager-customers .totalPage');
    let paginators = document.querySelectorAll('.table-manage-customers .paging_simple_numbers .paginate_button');
    const dropDownPageSize = document.querySelector('.table-filter-actions .table-filter-actions__pagesize__select');
    if (paginators && dropDownPageSize) {
      paginators = Array.from(paginators);
      paginators.forEach((btn) => {
        const $self = btn;
        const tempClassList = Array.from($self.classList);
        btn.addEventListener('click', () => {
          if (tempClassList.indexOf('previous') > -1) {
            if (customersTablePageIndex > 1) {
              ManageCustomers.initTableCustomer(customersTablePageIndex - 1, dropDownPageSize.value);
            }
          } else if (tempClassList.indexOf('next') > -1) {
            if (customersTablePageIndex < totalPage.value) {
              ManageCustomers.initTableCustomer(customersTablePageIndex + 1, dropDownPageSize.value);
            }
          } else {
            ManageCustomers.initTableCustomer($self.innerText, dropDownPageSize.value);
          }
        });
      });
    }
  }
};

export default ManageCustomers;
