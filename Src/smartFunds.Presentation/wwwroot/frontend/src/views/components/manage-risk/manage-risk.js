import Global from '../global/global';

const manageRisk = {
  initFunction: () => {
    Global.initDataTable('#manageRiskTable', {
      searching: false,
      paging: false,
      lengthChange: false,
      info: false,
      sort: false
    });
  }
};

export default manageRisk;
