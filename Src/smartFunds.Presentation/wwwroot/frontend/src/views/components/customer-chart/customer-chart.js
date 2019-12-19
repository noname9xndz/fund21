import Chart from 'chart.js';
import Global from '../global/global';

const CustomerShowChart = {
  initChartCustomerData: () => {
    CustomerShowChart.getDataCustomer();
    CustomerShowChart.initButtonSearchChartByDate();
    CustomerShowChart.removeDisableStateOfSearchChartBtn();
    CustomerShowChart.setValueInput();
  },
  getDataCustomer: () => {
    const customerDataApiUrl = document.querySelector('#customerDataApiUrl');
    const customerID = document.querySelector('#customerID');
    if (customerDataApiUrl) {
      const fromDate = document.querySelector('.customer-chart__date-range input[name="property-fluctuations-from"]');
      const toDate = document.querySelector('.customer-chart__date-range input[name="property-fluctuations-to"]');
      const apiGetPropertyFluctuations = `${customerDataApiUrl.value}?customerId=${customerID.value}&dateFrom=${fromDate ? fromDate.value : ''}&dateTo=${toDate ? toDate.value : ''}`;
      const model = {
        dateFrom: fromDate ? fromDate.value : '',
        dateTo: toDate ? toDate.value : ''
      };
      Global.getDataFromUrlPost(apiGetPropertyFluctuations, model).then((data) => {
        let propertyFluctuationsModel = data;
        propertyFluctuationsModel = JSON.parse(propertyFluctuationsModel);
        const noData = document.querySelector('#checkChartData');
        const newDateArray = [];
        const newAmountArray = [];
        const checkLengthData = (propertyFluctuationsModel.listProperty).length;
        if (noData && checkLengthData > 0) {
          noData.style.display = 'none';
        } else {
          noData.style.display = 'block';
        }
        propertyFluctuationsModel.listProperty.forEach((item) => {
          const currentDate = new Date(item.date);
          const day = currentDate.getDate();
          const newMonth = currentDate.getMonth() + 1;
          const newYear = currentDate.getFullYear();
          item.newDay = day;
          newDateArray.push({
            day: item.newDay,
            month: newMonth,
            year: newYear
          });
          newAmountArray.push({
            amount: item.amount
          });
        });
        const labelArray = newDateArray.map(x => (`${x.day} / ${x.month}`));
        const labelYear = newDateArray.map(x => (`${x.day} / ${x.month} / ${x.year}`));
        const numberArray = newAmountArray.map(x => x.amount);
        CustomerShowChart.initChart(labelArray, numberArray, labelYear);
      });
    }
  },
  initChart: (label, dataChart, labelYear) => {
    const customerChart = document.querySelector('#chart-customer');
    if (customerChart) {
      if (window.myChart !== undefined) {
        window.myChart.destroy();
      }
      window.myChart = new Chart(customerChart, {
        type: 'line',
        data: {
          labels: labelYear,
          datasets: [{
            label: 'Tài sản',
            data: dataChart,
            backgroundColor: 'transparent',
            borderColor: '#222d32',
            borderWidth: 1
          }]
        },
        options: {
          scales: {
            xAxes: [{
              type: 'category',
              labels: label
            }],
            yAxes: [{
              ticks: {
                callback(labels) {
                  return labels.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                },
                beginAtZero: true
              }
            }]
          },
          tooltips: {
            callbacks: {
              label(tooltipItem) {
                return `${tooltipItem.yLabel.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',')} VNĐ`;
              },
              title(tooltipItem, data) {
                return data.labels[tooltipItem[0].index];
              }
            }
          }
        }
      });
    }
  },
  initButtonSearchChartByDate: () => {
    const btnSearch = document.querySelector('.customer-chart__date-range .btn-part');
    if (btnSearch) {
      btnSearch.addEventListener('click', () => {
        const fromDate = document.querySelector('.customer-chart__date-range input[name="property-fluctuations-from"]');
        const toDate = document.querySelector('.customer-chart__date-range input[name="property-fluctuations-to"]');
        if (fromDate && toDate && fromDate.value !== '' && toDate.value !== '') {
          CustomerShowChart.getDataCustomer();
        }
      });
    }
  },
  removeDisableStateOfSearchChartBtn: () => {
    const btnSearch = document.querySelector('.customer-chart__date-range .btn-part');
    const fromDate = document.querySelector('.customer-chart__date-range input[name="property-fluctuations-from"]');
    const toDate = document.querySelector('.customer-chart__date-range input[name="property-fluctuations-to"]');
    if (btnSearch) {
      if (fromDate && toDate && fromDate.value !== '' && toDate.value !== '') {
        btnSearch.classList.remove('disabled');
      }
    }
  },
  setValueInput() {
    Global.initDateForInput('.customer-chart__date-range #property-fluctuations-from', '.customer-chart__date-range #property-fluctuations-to');
  }
};

export default CustomerShowChart;
