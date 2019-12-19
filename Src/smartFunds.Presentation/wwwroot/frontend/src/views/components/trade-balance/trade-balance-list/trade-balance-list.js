import Global from '../../global/global';
import DatatableLanguages from '../../datatables/datatables-language';

const TradeBalance = {
  initDatatable() {
    const tableElm = document.querySelector('#trade-balance-table');
    if (tableElm) {
      const $table = $(tableElm).DataTable({
        info: false,
        searching: false,
        lengthChange: false,
        sort: false,
        language: DatatableLanguages,
        drawCallback: () => {
          Global.hidePagingIfOnepage(tableElm);
        }
      });
      Global.drawTableWithLength(tableElm);
      Global.initPageLengthChange(tableElm);
    }
  }
};

export default TradeBalance;
