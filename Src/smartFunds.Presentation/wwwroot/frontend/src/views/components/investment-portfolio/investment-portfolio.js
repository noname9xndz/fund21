import Global from '../global/global';
import DatatableLanguages from '../datatables/datatables-language';

const InvestmentPortfolio = {
  initDatatable() {
    const tableElm = document.querySelector('#invesment-table');
    if (tableElm) {
      const $table = $(tableElm).DataTable({
        info: false,
        searching: false,
        lengthChange: false,
        language: DatatableLanguages,
        sort: false,
        drawCallback: () => {
          Global.hidePagingIfOnepage(tableElm);
        }
      });
      Global.drawTableWithLength(tableElm);
      Global.initPageLengthChange(tableElm);
    }
  }
};

export default InvestmentPortfolio;
