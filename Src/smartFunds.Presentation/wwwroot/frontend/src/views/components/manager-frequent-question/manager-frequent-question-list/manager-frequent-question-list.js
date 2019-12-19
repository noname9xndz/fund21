import Global from '../../global/global';

const ManagerFrequentQuestion = {
  initFunction: () => {
    ManagerFrequentQuestion.initTable();
  },
  initTable: () => {
    Global.initDataTable('.table-list-category', {
      searching: false,
      info: false,
      paging: false,
      sort: false,
      columnDefs: [{
        targets: 1,
        orderable: false
      }]
    });
  }
};

export default ManagerFrequentQuestion;
