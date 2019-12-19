import Global from '../global/global';
import DatatableLanguages from '../datatables/datatables-language';

const ManagerUser = {
  initDatatable() {
    const tableElm = document.querySelector('#manager-user__table');
    if (tableElm) {
      const $table = $(tableElm).DataTable({
        info: false,
        searching: false,
        lengthChange: false,
        language: DatatableLanguages,
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
      Global.drawTableWithLength(tableElm);
      Global.initPageLengthChange(tableElm);
    }
  }
};

export default ManagerUser;
