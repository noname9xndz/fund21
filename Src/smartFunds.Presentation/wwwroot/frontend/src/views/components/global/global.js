/* eslint-disable func-names */
import MicroModal from 'micromodal';
import { Validation } from '../bunnyjs/src/Validation';
import DatatableLanguages from '../datatables/datatables-language';
import invesmentTarget from '../user-invesments-target-create/user-invesments-target-create';
import userWithDrawal from '../user-withdrawal/user-withdrawal';

let rowSelectedOfCustomerTable = [];
const numeral = require('numeraljs');

const Global = {
  callAjaxRequest: (urlIn, dataIn) => new Promise((resolve) => {
    $.ajax({
      method: 'POST',
      url: urlIn,
      data: dataIn,
      cache: false,
      contentType: false,
      processData: false,
      success: (responseData) => {
        resolve(responseData);
      },
      error: (error) => {
        alert('Something Wrong !');
      }
    });
  }),

  getDataFromAjaxCall: async (url, data) => {
    const dataResult = await Global.callAjaxRequest(url, data);
    return dataResult;
  },

  requestMoreItems: url => new Promise((resolve) => {
    const xhrHttp = new XMLHttpRequest();
    let getItemsurl = document.querySelector('input#getMoreInsightItemsUrl').value;
    getItemsurl += '?pageIndex=1&pageSize=6';
    xhrHttp.onreadystatechange = () => {
      if ((xhrHttp.readyState === 4 && xhrHttp.status === 200)) {
        resolve(xhrHttp.response);
      }
    };
    xhrHttp.open('GET', getItemsurl, true);
    xhrHttp.send();
  }),

  getDataFromUrlGet: async (url) => {
    const resultDataList = await Global.requestMoreItems(url);
    return resultDataList;
  },

  requestDataFromUrlPost: (url, data) => new Promise((resolve) => {
    const xhrHttp = new XMLHttpRequest();
    const jsonData = JSON.stringify(data);
    xhrHttp.open('POST', url, true);
    xhrHttp.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');
    xhrHttp.onload = () => {
      if (xhrHttp.readyState === 4 && xhrHttp.status === 200 && xhrHttp.response !== null) {
        resolve(xhrHttp.response);
      }
    };
    xhrHttp.send(jsonData);
  }),

  getDataFromUrlPost: async (url, data) => {
    const dataResult = await Global.requestDataFromUrlPost(url, data);
    return dataResult;
  },

  isInViewport: (elements) => {
    const scroll = window.scrollY || window.pageYOffset;
    const boundsTop = elements.getBoundingClientRect().top + scroll;
    const boundGap = window.innerWidth >= 768 ? 500 : 300;

    const viewport = {
      top: scroll,
      bottom: scroll + window.innerHeight
    };

    const bounds = {
      top: boundsTop,
      bottom: boundsTop + elements.clientHeight
    };
    return (
      (bounds.bottom >= viewport.top && bounds.bottom <= viewport.bottom)
      || (bounds.top <= viewport.bottom - boundGap && bounds.top >= viewport.top)
    );
  },

  animatedOnScroll: () => {
    const elements = document.querySelectorAll(
      '.animated-on-scroll, .animated-on-scroll-slow, .animated-on-scroll-fast'
    );
    elements.forEach((ele) => {
      const isIsViewport = Global.isInViewport(ele);
      if (isIsViewport) {
        const animationType = ele.getAttribute('animation-type');
        ele.classList.add(animationType);
      }
    });
  },

  initBunnyValidationFormConfig: () => {
    Validation.lang = {
      emailRegex: document.querySelector('#emailRegexFormMsg') ? document.querySelector('#emailRegexFormMsg').value : '',
      email: document.querySelector('#emailRegexFormMsg') ? document.querySelector('#emailRegexFormMsg').value : '',
      required: document.querySelector('#requiredFormMsg') ? document.querySelector('#requiredFormMsg').value : ''
    };
    Validation.validators.emailRegex = (input) => {
      const emailRegex = new Promise((valid, invalid) => {
        if (input.hasAttribute('emailRegex')) {
          if (Global.isValidEmail(input.value)) {
            valid();
          } else {
            invalid();
          }
        } else {
          valid();
        }
      });
      return emailRegex;
    };
  },

  isValidEmail: (email) => {
    const re = /[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/i;
    return re.test(email);
  },

  initDataTable: (id, options) => {
    options.language = DatatableLanguages;
    $(id).DataTable(options);
  },
  initDatePicker: (dateHolder) => {
    const dateValue = document.querySelector(dateHolder);
    if (dateValue) {
      $(dateValue).datepicker({
        dateFormat: 'dd/mm/yy',
        changeYear: true,
        maxDate: 0,
        beforeShow: () => {
          $('.ui-datepicker').addClass('ui-datepicker--customs');
        }
      });
    }
  },
  changeEditableInput(buttonType) {
    const classBtn = buttonType === 'edit' ? '.edit-table-info' : '.save-table-info';
    const allBtns = document.querySelectorAll(classBtn);
    if (allBtns) {
      allBtns.forEach((eachBtn) => {
        eachBtn.addEventListener('click', (evt) => {
          evt.preventDefault();
          Global.changeDisabledToEditableInput(buttonType, eachBtn);
        });
      });
    }
  },

  changeDisabledToEditableInput(buttonType, btnElm) {
    const disabledValue = !(buttonType === 'edit');
    const detailBlock = Global.getParentElemetWithClassName(btnElm, 'detail-block');
    if (detailBlock) {
      const allEditableInputs = detailBlock.querySelectorAll('input.is-editable');
      if (allEditableInputs) {
        allEditableInputs.forEach((eachInput) => {
          eachInput.disabled = disabledValue;
        });
      }
    }
  },

  getParentElemetWithClassName(childElm, classParentName) {
    let parentElm = childElm;
    while (!parentElm.classList.contains(classParentName) && parentElm.tagName !== 'HTML') {
      parentElm = parentElm.parentElement;
    }
    return parentElm.tagName === 'HTML' ? null : parentElm;
  },

  getParentElemetWithTagName(childElm, tagParentName) {
    let parentElm = childElm.parentElement;
    while (parentElm.tagName.toLowerCase() !== tagParentName && parentElm.tagName !== 'HTML') {
      parentElm = parentElm.parentElement;
    }
    return parentElm.tagName === 'HTML' ? null : parentElm;
  },

  initCollapse: (parent, btn, content) => {
    const carouselItemWrappers = document.querySelectorAll(`${parent}`);
    if (carouselItemWrappers) {
      carouselItemWrappers.forEach((eachCarousel) => {
        const allButtons = eachCarousel.querySelectorAll(`${btn}`);
        const allTexts = eachCarousel.querySelectorAll(`${content}`);
        allButtons.forEach((eachButton) => {
          const currentContentElm = eachButton.parentElement.querySelector(`${content}`);
          eachButton.addEventListener('click', () => {
            if (!eachButton.classList.contains('active')) {
              allButtons.forEach(x => x.classList.remove('active'));
              eachButton.classList.add('active');
              allTexts.forEach(x => $(x).slideUp());
              $(currentContentElm).slideDown();
              currentContentElm.classList.add('active');
            } else {
              $(currentContentElm).slideUp();
              eachButton.classList.remove('active');
              currentContentElm.classList.remove('active');
            }
          });
        });
      });
    }
  },

  scrollToElm: (eachButton) => {
    setTimeout(() => {
      const offSetTop = $(eachButton).offset().top;
      const headerHeight = window.innerWidth >= 768 ? 72 : 40;
      $('html, body').stop().animate({
        scrollTop: offSetTop - headerHeight
      }, 350);
    }, 500);
  },

  addClassCurrentHeader: (currentPage, currentMenu) => {
    if ($(currentPage) && $(currentPage).length > 0) {
      $('.headerUser_login .main-menu .menu-items').removeClass('current');
      $(`.headerUser_login .main-menu .menu-items${currentMenu}`).addClass('current');
    }
  },

  showMobileHeaderUser: () => {
    $('.headerUser .mobile-header .mobile-top-header .hamburger').on('click', () => {
      $('.headerUser .mobile-header .navbar-mobile').css('left', '15px');
      $('.headerUser .mobile-header .overlay').css('display', 'block');
      $('body').css('overflow', 'hidden');
    });
    $('.headerUser .mobile-header .overlay').on('click', () => {
      $('.headerUser .mobile-header .navbar-mobile').css('left', '-100%');
      $('.headerUser .mobile-header .overlay').css('display', 'none');
      $('body').css('overflow', 'auto');
    });
  },

  uploadPreviewImage: () => {
    const allInputImgType = document.querySelectorAll('input[type="file"][accept="image/*"]');
    if (allInputImgType) {
      allInputImgType.forEach((eachInput) => {
        eachInput.addEventListener('change', () => {
          Global.readImageURL(eachInput);
        });
      });
    }
  },

  readImageURL(input) {
    if (input.files && input.files[0]) {
      const reader = new FileReader();
      const imgElm = input.parentElement.querySelector('.form-group__image-wrapper img');
      if (imgElm) {
        reader.onload = (e) => {
          if (e.target.result.includes('data:image/')) {
            imgElm.setAttribute('src', e.target.result);
          }
        };
        reader.readAsDataURL(input.files[0]);
      }
    }
  },
  initMicroModal() {
    MicroModal.init({
      onShow: () => {
      },
      // onClose: () => Global.removefixBodyScrollWhenCloseModal(),
      awaitCloseAnimation: true
    });
  },

  initClickEditTableDirectly: () => {
    const allEditBtn = document.querySelectorAll('.edit-directly');
    if (allEditBtn.length > 0) {
      allEditBtn.forEach((eachEditBtn) => {
        eachEditBtn.addEventListener('click', (evt) => {
          const tableId = eachEditBtn.getAttribute('data-table-id');
          const tableElm = document.querySelector(`#${tableId}`);
          const tableTrInput = tableElm.querySelectorAll('.editable-input');
          evt.preventDefault();
          tableTrInput.forEach((item) => {
            if (item.getAttribute('readonly')) {
              item.removeAttribute('readonly');
            } else {
              item.setAttribute('readonly', 'readonly');
            }
          });
        });
      });
    }
  },
  initPageLengthChange(tableElm) {
    const sectionElm = Global.getParentElemetWithTagName(tableElm, 'section');
    const selectPageLength = sectionElm.querySelector('.table-filter-actions__pagesize__select');
    if (selectPageLength) {
      selectPageLength.addEventListener('change', () => {
        Global.drawTableWithLength(tableElm, selectPageLength.value);
      });
    }
  },

  drawTableWithLength(tableElm, pageLength = 10) {
    const leng = parseInt(pageLength, 10);
    $(tableElm).DataTable().page.len(leng).draw();
  },

  initAllCheckBox(tableElm) {
    Global.resetCheckboxAll(tableElm);
    const checkAllInput = tableElm.querySelector('thead th .select-all-btn');
    const arrInput = tableElm.querySelectorAll('tbody td input[type="checkbox"]:not(:disabled)');
    if (checkAllInput && arrInput.length > 0) {
      const allInputNotDisabled = Array.from(arrInput);
      allInputNotDisabled.forEach((input) => {
        if (input.checked) {
          rowSelectedOfCustomerTable.push(input.value);
        }
      });
      Global.initCheckAllTable(checkAllInput, tableElm);
      Global.initCheckBoxesTable(checkAllInput, tableElm);
    }
  },

  resetCheckboxAll(tableElm) {
    rowSelectedOfCustomerTable = [];
    const arrInput = tableElm.querySelectorAll('input[type="checkbox"]');
    Array.from(arrInput).forEach((eachInput) => { eachInput.checked = false; });

    const sectionElm = Global.getParentElemetWithTagName(tableElm, 'section');
    const actionsElm = sectionElm.querySelector('.table-filter-actions');
    if (actionsElm) {
      actionsElm.classList.remove('active-delete-btn');
    }
  },

  initCheckAllTable(checkAllInput) {
    checkAllInput.removeEventListener('change', Global.eventClickAll);
    checkAllInput.addEventListener('change', Global.eventClickAll);
  },
  eventClickAll(evt) {
    const tableElm = evt.target.closest('section');
    const checkAllInput = tableElm.querySelector('thead input[type="checkbox"]');
    const arrInput = tableElm.querySelectorAll('tbody td input[type="checkbox"]:not(:disabled)');
    rowSelectedOfCustomerTable = [];
    if (checkAllInput.checked) {
      arrInput.forEach((input) => {
        input.checked = true;
        rowSelectedOfCustomerTable.push(input.value);
      });
    } else {
      arrInput.forEach((input) => {
        input.checked = false;
      });
    }
    Global.updateDeleteButtonStatus(tableElm);
  },
  initCheckBoxesTable(checkAllInput, tableElm) {
    const arrInput = tableElm.querySelectorAll('tbody td input[type="checkbox"]:not(:disabled)');
    arrInput.forEach((eachInput) => {
      eachInput.removeEventListener('change', Global.eventInvidualInput);
      eachInput.addEventListener('change', Global.eventInvidualInput);
    });
  },
  eventInvidualInput(evt) {
    const inputElm = evt.target;
    const tableElm = evt.target.closest('section');
    const checkAllInput = tableElm.querySelector('thead input[type="checkbox"]');
    const arrInput = tableElm.querySelectorAll('tbody td input[type="checkbox"]:not(:disabled)');
    if (inputElm.checked) {
      rowSelectedOfCustomerTable.push(inputElm.value);
    } else {
      rowSelectedOfCustomerTable.splice(rowSelectedOfCustomerTable.indexOf(inputElm.value), 1);
    }
    if (rowSelectedOfCustomerTable.length === arrInput.length) {
      checkAllInput.checked = true;
    } else {
      checkAllInput.checked = false;
    }
    Global.updateDeleteButtonStatus(tableElm);
  },
  updateDeleteButtonStatus(tableElm) {
    const allInputCheckboxs = tableElm.querySelectorAll('tbody input[type="checkbox"]');
    const sectionElm = Global.getParentElemetWithTagName(tableElm, 'section');
    const filterActionBlock = sectionElm.querySelector('.table-filter-actions');
    if (filterActionBlock) {
      if (Global.hasAtLeastOneCheckedRow(allInputCheckboxs)) {
        filterActionBlock.classList.add('active-delete-btn');
      } else {
        filterActionBlock.classList.remove('active-delete-btn');
      }
    }
  },
  hasAtLeastOneCheckedRow(allInputCheckboxs) {
    return Array.from(allInputCheckboxs).some(x => !!x.checked);
  },
  hidePagingIfOnepage(tableElm) {
    if (tableElm.id !== 'invesment-detail') {
      const tableInfo = $(tableElm).DataTable().page.info();
      const sectionElm = Global.getParentElemetWithTagName(tableElm, 'section');
      if (tableInfo.pages <= 1) {
        sectionElm.classList.add('hide-paging');
      } else {
        sectionElm.classList.remove('hide-paging');
      }
    }
  },
  tooglePassword: (iconElm) => {
    const btnShowPassWord = document.querySelectorAll(iconElm);
    btnShowPassWord.forEach((btn) => {
      btn.addEventListener('click', () => {
        Global.showOrHidePassword(btn);
      });
    });
  },
  showOrHidePassword: (btn) => {
    const inputPasswordParent = btn.parentElement.querySelector('input');
    if (inputPasswordParent) {
      if (inputPasswordParent.type === 'password') {
        inputPasswordParent.type = 'text';
      } else {
        inputPasswordParent.type = 'password';
      }
    }
  },
  initDateRangePicker: (fromDate, toDate) => {
    const dateRangeFrom = document.querySelector(fromDate);
    const dateRangeTo = document.querySelector(toDate);
    if (dateRangeFrom && dateRangeTo) {
      $(dateRangeFrom).datepicker({
        dateFormat: 'dd/mm/yy',
        changeYear: true,
        maxDate: 0,
        beforeShow: () => {
          $('.ui-datepicker').addClass('ui-datepicker--customs');
        }
      });
      $(dateRangeTo).datepicker({
        dateFormat: 'dd/mm/yy',
        changeYear: true,
        maxDate: 0,
        beforeShow: () => {
          $('.ui-datepicker').addClass('ui-datepicker--customs');
        }
      });
      $(dateRangeFrom).on('change', (e) => {
        $(dateRangeTo).datepicker('option', 'minDate', $(e.currentTarget).datepicker('getDate'));
      });
      $(dateRangeTo).on('change', (e) => {
        $(dateRangeFrom).datepicker('option', 'maxDate', $(e.currentTarget).datepicker('getDate'));
      });
    }
  },
  inputFloatNumber: () => {
    const formControlNumber = document.querySelectorAll('.form-control-float');
    if (formControlNumber.length > 0) {
      formControlNumber.forEach((item) => {
        $(item).on(('input paste'), function e(evt) {
          if (evt.which >= 37 && evt.which <= 40) return;
          $(this).val((index, value) => value
            .replace(/[^0-9\.,]/g, ''));
        });
      });
    }
  },
  inputValid: () => {
    const formControlNumber = document.querySelectorAll('.form-control-number');
    if (formControlNumber.length > 0) {
      formControlNumber.forEach((item) => {
        $(item).on(('input paste'), function e(evt) {
          if (evt.which >= 37 && evt.which <= 40) return;
          $(this).val((index, value) => value
            .replace(/\D/g, '')
            .replace(/\B(?=(\d{3})+(?!\d))/g, ','));
        });
      });
    }
  },
  inputInvesmentFund: () => {
    const formControlNumber = document.querySelectorAll('.form-control-number--fund');
    if (formControlNumber.length > 0) {
      formControlNumber.forEach((item) => {
        $(item).on(('input paste'), function e(evt) {
          if (evt.which >= 37 && evt.which <= 40) return;
          $(this).val((index, value) => value
            .replace(/[^0-9.,]*/g, ''));
        });
      });
    }
  },
  inputValidPhone: () => {
    const formControlNumber = document.querySelectorAll('.form-control-number--phone');
    if (formControlNumber.length > 0) {
      formControlNumber.forEach((item) => {
        $(item).on(('input paste'), function e(evt) {
          if (evt.which >= 37 && evt.which <= 40) return;
          $(this).val((index, value) => value
            .replace(/\D/g, '')
            .replace(/\B(?=(\d{3})+(?!\d))/g, ''));
        });
      });
    }
  },
  inputDeleteSpace: () => {
    const formControlSpace = document.querySelectorAll('.form-control-space');
    if (formControlSpace.length > 0) {
      formControlSpace.forEach((item) => {
        $(item).on(('input paste'), function e(evt) {
          if (evt.which >= 37 && evt.which <= 40) return;
          $(this).val((index, value) => value
            .replace(/ /g, ''));
        });
      });
    }
  },
  initSelectDropdown: (selector) => {
    $(selector).selectmenu({
      appendTo: '.body-content-endUser',
      create: () => {
        $('.ui-selectmenu-button').addClass('input-form-control');
      },
      select: () => {
        if (document.querySelector('.user-invesment-target')) {
          invesmentTarget.initSuggestionDropdown();
        }
        if (document.querySelector('.user-withdrawal-invest .type-withdrawal')) {
          userWithDrawal.initCostWithDrawal();
        }
      }
    });
  },
  getFile: () => {
    const btnAddFile = document.querySelector('#btn-add-file');
    const fileName = document.querySelector('#file-name');
    if (btnAddFile && fileName) {
      document.querySelector('#input-file').addEventListener('change', function x() {
        fileName.textContent = this.files[0].name;
      });
      btnAddFile.onclick = function c() {
        $('#input-file').trigger('click');
      };
    }
  },
  initSuggestionMoney: () => {
    const amountMoney = document.querySelector('.user-expected-money-amount input');
    const userWithdrawal = document.querySelector('.user-withdrawal-invest');
    const userInvesment = document.querySelector('.user-invesment-target');
    let listSuggestion = [];
    if (amountMoney) {
      $(amountMoney).on('input paste change', () => {
        if (amountMoney.value.length > 0 && amountMoney.value.length < 17) {
          listSuggestion = [];
          const a = (numeral(amountMoney.value).value());
          listSuggestion.push(numeral(a).format('0,0'));
          for (let i = 0; i < 3; i++) {
            const b = numeral(listSuggestion[listSuggestion.length - 1]).value() * 10;
            listSuggestion.push(numeral(b).format('0,0'));
          }
          const tags = listSuggestion.map(item => item.toString());
          $(amountMoney).autocomplete({
            appendTo: '.body-content-endUser',
            autoFocus: true,
            disabled: false,
            minLength: 0,
            source: (request, response) => {
              response(tags);
            },
            close: () => {
              if (userInvesment) {
                invesmentTarget.initSuggestionDropdown();
              } if (userWithdrawal) {
                userWithDrawal.initCostWithDrawal();
              }
            },
            open: () => {
              if (userInvesment) {
                invesmentTarget.initSuggestionDropdown();
              } if (userWithdrawal) {
                userWithDrawal.initCostWithDrawal();
              }
            }
            // select: () => {
            //   if (userInvesment) {
            //     invesmentTarget.initSuggestionDropdown();
            //   } if (userWithdrawal) {
            //     userWithDrawal.initCostWithDrawal();
            //   }
            // }
          });
          if (userInvesment) {
            invesmentTarget.initSuggestionDropdown();
          }
          if (userWithdrawal) {
            userWithDrawal.initCostWithDrawal();
          }
        }
        if (amountMoney.value.length === 0 || amountMoney.value.length >= 17) {
          $(amountMoney).autocomplete('disable');
        }
      });
      $(amountMoney).autocomplete();
    }
  },
  initDateForInput: (fromDate, toDate) => {
    const inputFromDate = document.querySelector(fromDate);
    const inputToDate = document.querySelector(toDate);
    if (inputFromDate && inputToDate) {
      $(inputFromDate).datepicker('setDate', '-1m');
      $(inputToDate).datepicker('setDate', 'today');
    }
  },
  initClickPaging: (item) => {
    const ajaxPaging = item.querySelector('.ajax-paging');
    if (ajaxPaging) {
      const btnPrevious = ajaxPaging.querySelector('.previous');
      const btnNext = ajaxPaging.querySelector('.next');
      let btnPages = ajaxPaging.querySelectorAll('span a');
      btnPages = Array.from(btnPages);
      const tableData = item.querySelector('.table__wrapper table');

      if (btnPrevious && btnNext && btnPages.length === 5) {
        btnPrevious.addEventListener('click', () => {
          const firstBtnPaging = btnPages[0];
          if (firstBtnPaging.classList.contains('current')) {
            btnPages.forEach((item) => {
              if ((parseInt(firstBtnPaging.dataset.dtIdx, 10) !== 1)) {
                item.textContent = parseInt(item.textContent, 10) - 1;
                item.dataset.dtIdx = parseInt(item.dataset.dtIdx, 10) - 1;
              }
            });
          }
        });

        btnNext.addEventListener('click', () => {
          const lastBtnPaging = btnPages[btnPages.length - 1];
          if (lastBtnPaging.classList.contains('current')) {
            if (parseInt(lastBtnPaging.dataset.dtIdx, 10) < ajaxPaging.dataset.dtIdx) {
              btnPages.forEach((item) => {
                item.textContent = parseInt(item.textContent, 10) + 1;
                item.dataset.dtIdx = parseInt(item.dataset.dtIdx, 10) + 1;
              });
              ajaxPaging.dataset.pageCurrent = lastBtnPaging.textContent;
              $(lastBtnPaging).trigger('click');
            }
            if (lastBtnPaging.dataset.dtIdx === ajaxPaging.dataset.dtIdx) {
              btnNext.classList.add('disabled');
            }
          }
        });
      }
    }
  },
  showPopupMobile: () => {
    $('.headerUser .mobile-header .link-show-popup').on('click', () => {
      $('.headerUser .mobile-header .navbar-mobile').css('left', '-100%');
      $('.headerUser .mobile-header .overlay').css('display', 'none');
      $('body').css('overflow', 'auto');
    });
  }
};

export default Global;
