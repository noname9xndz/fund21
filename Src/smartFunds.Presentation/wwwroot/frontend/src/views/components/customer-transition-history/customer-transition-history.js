import Global from '../global/global';

const customerTransactionHistory = {
  initFunction: () => {
    customerTransactionHistory.initTransactionTable();
  },
  initTransactionTable: () => {
    Global.initDataTable('#customer-transaction-history-teble', {
      searching: false,
      info: false
    });
  }
};

export default customerTransactionHistory;
