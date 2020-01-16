import Chart from 'chart.js';
import Global from '../global/global';

const UserProfileShowChart = {
  showDate: () => {
    Global.initDateRangePicker('.user_asset_from', '.user_asset_to');
  },
  initValueForInput: () => {
    Global.initDateForInput('.user-chart-section input[name="user_asset_from"]', '.user-chart-section input[name="user_asset_to"]');
  },
  initChartProfileData: () => {
    UserProfileShowChart.getDataProfile();
    UserProfileShowChart.initButtonSearchChart();
  },
  getDataProfile: () => {
    const profileDataApiUrl = document.querySelector('#profileDataApiUrl');
    if (profileDataApiUrl) {
      const fromDate = document.querySelector('.user-chart-section input[name="user_asset_from"]');
      const toDate = document.querySelector('.user-chart-section input[name="user_asset_to"]');
      const apiGetPropertyFluctuations = `${profileDataApiUrl.value}?dateFrom=${fromDate ? fromDate.value : ''}&dateTo=${toDate ? toDate.value : ''}`;
      const model = {
        dateFrom: fromDate ? fromDate.value : '',
        dateTo: toDate ? toDate.value : ''
      };
      Global.getDataFromUrlPost(apiGetPropertyFluctuations, model).then((data) => {
        let propertyFluctuationsModel = data;
        propertyFluctuationsModel = JSON.parse(propertyFluctuationsModel);
        const noData = document.querySelector('.user-chart-section .user-chart-content__container .chart span');
        const newDateArray = [];
        const newAmountArray = [];
        const checkLengthData = (propertyFluctuationsModel.listProperty).length;
        if (checkLengthData > 0) {
          noData.style.display = 'none';
        } else {
          noData.style.display = 'block';
        }
        propertyFluctuationsModel.listProperty.forEach((item) => {
          const currentDate = new Date(item.date);
          const day = currentDate.getDate();
          const newMonth = currentDate.getMonth() + 1;
          const newYear = currentDate.getFullYear();
          // debugger;
          const newDay = day;
          newDateArray.push({
            day: newDay,
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
        UserProfileShowChart.initChart(labelArray, numberArray, labelYear);
      });
    }
  },
  initChart: (label, dataChart, labelYear) => {
    const profileChart = document.querySelector('.user-chart-section #chart-profile');
    if (profileChart) {
      if (window.myChart !== undefined) {
        window.myChart.destroy();
      }
      window.myChart = new Chart(profileChart, {
        type: 'line',
        data: {
          labels: labelYear,
          datasets: [{
            label: 'Triệu VNĐ',
            data: dataChart,
            backgroundColor: 'rgb(255, 230, 213)',
            borderColor: '#F77314',
            borderWidth: 1
          }]
        },
        options: {
          legend: {
            onClick: null,
            position: 'top',
            labels: {
              padding: 0,
              boxWidth: 0,
              fontStyle: 'normal',
              fontColor: '#000'
            }
          },
          layout: {
            padding: {
              left: 20,
              right: 40,
              top: 30,
              bottom: 17
            }
          },
          scales: {
            xAxes: [{
              gridLines: {
                display: false
              },
              type: 'category',
              labels: label
            }],
            yAxes: [{
              ticks: {
                callback(labels) {
                  if (labels > 1) {
                    const label = labels / 1000000;
                    return label.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                  }
                  return labels.toPrecision(1);
                },
                beginAtZero: true
              },
              gridLines: {
                display: false
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
  initButtonSearchChart: () => {
    const btnSearch = document.querySelector('.user-chart-section .btn-search');
    if (btnSearch) {
      btnSearch.addEventListener('click', () => {
        const fromDate = document.querySelector('.user-chart-section input[name="user_asset_from"]');
        const toDate = document.querySelector('.user-chart-section input[name="user_asset_to"]');
        if (fromDate && toDate) {
          UserProfileShowChart.getDataProfile();
        }
      });
    }
  }
};
export default UserProfileShowChart;
