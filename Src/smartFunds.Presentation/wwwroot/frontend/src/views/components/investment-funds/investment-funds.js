import numeral from 'numeraljs';
import Global from '../global/global';
import DatatableLanguages from '../datatables/datatables-language';

const NAVArray = [];
const newNAVArray = [];
let proportionArray = [];
let NewNAVAjaxArray = [];

const proportionNAV = document.querySelectorAll('.proportion__NAV');
const currentNAV = document.querySelectorAll('.current__NAV');
const newNAV = document.querySelectorAll('.new__NAV');
const apiLinkInvestmentFund = document.getElementById('investment-funds__url');

const saveButton = document.querySelector('.investment-funds--Save');
const InvestmentFunds = {
  initFunction: () => {
    InvestmentFunds.inputOnChange();
    InvestmentFunds.redWord();
    InvestmentFunds.sendAjax();
  },
  initDatatable: () => {
    const tableElm = document.querySelector('#investment-funds__table');
    if (tableElm) {
      const $table = $(tableElm).DataTable({
        info: false,
        // paging: false,
        sort: false,
        searching: false,
        lengthChange: false,
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
      Global.drawTableWithLength(tableElm);
      Global.initPageLengthChange(tableElm);
    }
  },
  redWord: () => {
    if (currentNAV) {
      currentNAV.forEach((item) => {
        const originCurrentNAV = parseInt(item.textContent.replace(/,/g, ''), 10);
        NAVArray.push({
          currentNAV: originCurrentNAV
        });
      });
    }
    if (newNAV) {
      newNAV.forEach((item) => {
        const originNewNAV = parseInt(item.value.replace(/,/g, ''), 10);
        newNAVArray.push({
          newNAV: originNewNAV
        });
      });
    }
    if (proportionNAV) {
      proportionArray = _.merge(NAVArray, newNAVArray);
      proportionArray.forEach((obj) => {
        obj.Proportion = `${(obj.newNAV / obj.currentNAV * 100).toFixed(1)}%`;
      });
      proportionNAV.forEach((proportion, index) => {
        proportion.index = index + 1;
        proportion.innerHTML = proportionArray[index].Proportion;
        const originProportion = proportion.innerHTML.replace('%', ' ');
        if (originProportion > 105) {
          proportion.classList.add('red-word');
        }
      });
    }
  },
  inputOnChange: () => {
    newNAV.forEach((elm) => {
      elm.onkeyup = function x() {
        if (elm.value === '0' || elm.value === '') {
          saveButton.setAttribute('disabled', 'disabled');
        }
        else {
          saveButton.removeAttribute('disabled');
        }
        const originCurrentNAV = parseInt(elm.parentElement.previousElementSibling.textContent.replace(',', ''), 10);
        const originNewNAV = parseInt(elm.value.replace(/,/g, ''), 10);
        const result = elm.parentElement.nextElementSibling;
        result.innerHTML = `${(originNewNAV / originCurrentNAV * 100).toFixed(1)}%`;
        const originProportion = result.innerHTML.replace('%', ' ');
        if (originProportion > 105) {
          elm.parentElement.nextElementSibling.classList.add('red-word');
        } else {
          elm.parentElement.nextElementSibling.classList.remove('red-word');
        }
      };
    });
  },
  sendAjax: () => {
    if (saveButton) {
      saveButton.addEventListener('click', () => {
        saveButton.setAttribute('disabled', 'disabled');
        document.querySelector('.investment-funds__table__overlay').classList.remove('hide');
        document.querySelector('.edit-table-info').setAttribute('disabled', 'disabled');
        NewNAVAjaxArray = [];
        newNAV.forEach((item) => {
          // eslint-disable-next-line no-param-reassign
          item.value = numeral(parseFloat(item.value.replace(/,/g, ''))).format('0,0.00');
          item.setAttribute('readonly', 'readonly');
          NewNAVAjaxArray.push({
            ID: item.parentElement.parentElement.children[2].getAttribute('data-id'),
            NAVNew: parseFloat(item.value.replace(/,/g, ''))
            // NAVNew: numeral(parseFloat(item.value.replace(/,/g, ''))).format('0,0.00')
          });
        });
        const model = {
          UpdateNav: NewNAVAjaxArray
        };
        Global.getDataFromUrlPost(apiLinkInvestmentFund.value, model).then((data) => {
          saveButton.removeAttribute('disabled');
          document.querySelector('.edit-table-info').removeAttribute('disabled');
          document.querySelector('.investment-funds__table__overlay').classList.add('hide');
          if (!data.success) {
            document.querySelector('#table-error').textContent = `(${data.message})`;
          }
        });
      });
    }
  }
};

export default InvestmentFunds;
