import Global from '../global/global';
import DatatableLanguages from '../datatables/datatables-language';

const manageInvestFund = {
  initFunction: () => {
    Global.initDataTable('#invest-fund', {
      searching: false,
      info: false,
      language: DatatableLanguages
    });
  }
};

export default manageInvestFund;
