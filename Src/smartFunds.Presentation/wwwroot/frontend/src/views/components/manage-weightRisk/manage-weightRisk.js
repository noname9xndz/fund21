import Global from '../global/global';
import DatatableLanguages from '../datatables/datatables-language';

const weightRisk = {
  initFunction: () => {
    const tableElm = document.querySelector('#weightRiskTable');
    if (tableElm) {
      Global.initDataTable('#weightRiskTable', {
        searching: false,
        info: false,
        paging: false,
        language: DatatableLanguages,
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
  }
};

export default weightRisk;
