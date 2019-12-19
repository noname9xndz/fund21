import Global from '../../global/global';
import DatatableLanguages from '../../datatables/datatables-language';

const TradeBalanceDetail = {
  initDatatable() {
    const tableElm = document.querySelector('#trade-balance-detail');
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

export default TradeBalanceDetail;
