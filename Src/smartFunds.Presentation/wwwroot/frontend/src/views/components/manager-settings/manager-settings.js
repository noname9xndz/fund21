import Global from '../global/global';

const ManagerSetting = {
  initFunction: () => {
    ManagerSetting.clickAddMaintainTable();
    ManagerSetting.clickAddWithdrawTable();
    ManagerSetting.initClickForm();
    ManagerSetting.deleteDataTable();
    ManagerSetting.saveExpectedTable();
    ManagerSetting.checkedInput();
    ManagerSetting.uploadImage();
    ManagerSetting.optionsImage();
  },
  clickAddMaintainTable: () => {
    const allEditBtn = document.querySelectorAll('.add-maintain-table');
    if (allEditBtn.length > 0) {
      allEditBtn.forEach((eachEditBtn) => {
        eachEditBtn.addEventListener('click', (evt) => {
          evt.preventDefault();
          const tableId = eachEditBtn.getAttribute('data-table-id');
          const tableElm = document.querySelector(`#${tableId}`);
          const tableElmBody = tableElm.querySelector('tbody');
          const tableElmBodyTr = tableElmBody.querySelectorAll('tr');
          const lastTr = tableElmBodyTr[tableElmBodyTr.length - 1];
          const html = `
                        <tr data-tr-id="${lastTr.dataset.trId ? parseInt(lastTr.dataset.trId, 10) + 1 : 1}">
                          <td>
                            <input class="checkbox-item" type="checkbox" value="checkbox-${tableElmBodyTr.length - 1}">
                          </td>
                          <td> 
                            <input class="editable-input form-control-number" value="" type="text" readonly="readonly" name="ListFees[${tableElmBodyTr.length - 1}].AmountFrom">
                          </td>
                          <td> 
                            <input class="editable-input form-control-number" value="" type="text" readonly="readonly" name="ListFees[${tableElmBodyTr.length - 1}].AmountTo">
                          </td>
                          <td> 
                            <input class="editable-input form-control-float" value="" type="text" readonly="readonly" name="ListFees[${tableElmBodyTr.length - 1}].Percentage">
                          </td>
                        </tr>
                `;
          $(tableElmBody).append(html);
          const tableTrInput = tableElm.querySelectorAll('.editable-input');
          tableTrInput.forEach((item) => {
            item.removeAttribute('readonly');
          });
          Global.inputValid();
          Global.inputFloatNumber();
          ManagerSetting.checkedInput();
        });
      });
    }
  },
  clickAddWithdrawTable: () => {
    const allEditBtn = document.querySelectorAll('.add-withdraw-table');
    if (allEditBtn.length > 0) {
      allEditBtn.forEach((eachEditBtn) => {
        eachEditBtn.addEventListener('click', (evt) => {
          evt.preventDefault();
          const tableId = eachEditBtn.getAttribute('data-table-id');
          const tableElm = document.querySelector(`#${tableId}`);
          const tableElmBody = tableElm.querySelector('tbody');
          const tableElmBodyTr = tableElmBody.querySelectorAll('tr');
          const lastTr = tableElmBodyTr[tableElmBodyTr.length - 1];
          const html = `
                        <tr data-tr-id="${lastTr.dataset.trId ? parseInt(lastTr.dataset.trId, 10) + 1 : 1}">
                          <td>
                            <input class="checkbox-item" type="checkbox" value="checkbox-${tableElmBodyTr.length - 1}">
                          </td>
                          <td> 
                            <input class="editable-input form-control-number" value="" type="text" readonly="readonly" name="ListFees[${tableElmBodyTr.length - 1}].TimeInvestmentBegin">
                          </td>
                          <td> 
                            <input class="editable-input form-control-number" value="" type="text" readonly="readonly" name="ListFees[${tableElmBodyTr.length - 1}].TimeInvestmentEnd">
                          </td>
                          <td> 
                            <input class="editable-input form-control-float" value="" type="text" readonly="readonly" name="ListFees[${tableElmBodyTr.length - 1}].Percentage">
                          </td>
                        </tr>
                `;
          $(tableElmBody).append(html);
          const tableTrInput = tableElm.querySelectorAll('.editable-input');
          tableTrInput.forEach((item) => {
            item.removeAttribute('readonly');
          });
          Global.inputValid();
          Global.inputFloatNumber();
          ManagerSetting.checkedInput();
        });
      });
    }
  },
  saveExpectedTable: () => {
    const allEditBtn = document.querySelectorAll('.save-expected-table');
    if (allEditBtn.length > 0) {
      allEditBtn.forEach((item) => {
        const btnSubmit = item.querySelector('button[type="button"]');
        if (item.dataset.apiSave) {
          btnSubmit.addEventListener('click', () => {
            const tableTd = document.querySelectorAll(`#${item.dataset.tableId} tbody tr td`);
            if (tableTd.length > 0) {
              const valueFeeTable = [];
              tableTd.forEach((td) => {
                td.querySelector('input').setAttribute('readonly', 'readonly');
                if (td.dataset.id) {
                  valueFeeTable.push({
                    Id: parseInt(td.dataset.id, 10),
                    Value: parseFloat(td.querySelector('input').value)
                  });
                }
              });
              const model = {
                ListModels: valueFeeTable
              };
              Global.getDataFromUrlPost(item.dataset.apiSave, model).then((data) => {
                console.log('data', data);
                window.location.reload();
              });
            }
          });
        }
      });
    }
  },
  initClickForm: () => {
    const submitForm = document.querySelectorAll('.submit-form');
    if (submitForm.length > 0) {
      submitForm.forEach((item) => {
        const btnSubmit = item.querySelector('button[type="submit"]');
        const btnSaveTable = item.querySelector('.save-table');
        const btnTrigger = btnSaveTable.querySelector('button[type="button"]');
        btnTrigger.addEventListener('click', () => {
          const table = document.querySelector(`#${btnSaveTable.dataset.tableId}`);
          let tableInput = table.querySelectorAll('tbody input');
          tableInput = Array.from(tableInput);
          const puickFee = item.querySelector('.quick-fee input');
          if (tableInput.length > 0) {
            const newTableInput = tableInput.filter(x => x.value === '');
            if (newTableInput.length > 0) {
              document.querySelector(`${table.dataset.showError}`).textContent = document.querySelector('#messRequired').value;
            } else if (puickFee && puickFee.value === '') {
              document.querySelector(`${table.dataset.showError}`).textContent = document.querySelector('#messRequired').value;
            } else {
              $(btnSubmit).trigger('click');
            }
          }
        });
      });
    }
  },
  saveDataTable: () => {
    const allEditBtn = document.querySelectorAll('.save-fee-table');
    if (allEditBtn.length > 0) {
      allEditBtn.forEach((item) => {
        const btnSubmit = item.querySelector('button[type="button"]');
        if (item.dataset.apiSave) {
          btnSubmit.addEventListener('click', () => {
            const tableInput = document.querySelectorAll(`#${item.dataset.tableId} tbody input`);
            if (tableInput.length > 0) {
              console.log(tableInput);
              tableInput.forEach((input) => { });
            }
          });
        }
      });
    }
  },
  deleteDataTable: () => {
    const allEditBtn = document.querySelectorAll('.delete-fee-table');
    if (allEditBtn.length > 0) {
      allEditBtn.forEach((item) => {
        let deleteFeeTable = [];
        const btnSubmit = item.querySelector('button[type="button"]');
        if (item.dataset.apiDelete) {
          btnSubmit.addEventListener('click', () => {
            const tableTr = document.querySelectorAll(`#${item.dataset.tableId} tbody tr`);
            tableTr.forEach((tr) => {
              const inputCheckBox = tr.querySelector('td input[type="checkbox"]');
              if (inputCheckBox && inputCheckBox.checked) {
                $(tr).remove();
                deleteFeeTable.push({
                  Id: parseInt(tr.dataset.trId, 10)
                });
              }
            });
            deleteFeeTable = deleteFeeTable.map(x => x.Id);
            Global.getDataFromUrlPost(item.dataset.apiDelete, deleteFeeTable).then((data) => {
              console.log('data', data);
              window.location.reload();
            });
          });
        }
      });
    }
  },
  checkedInput: () => {
    const addDirectlyTrTable = document.querySelectorAll('.add-directly');
    addDirectlyTrTable.forEach((item) => {
      item.addEventListener('click', () => {
        ManagerSetting.checkedInput();
        ManagerSetting.deleteDataTable();
        ManagerSetting.initClickForm();
      });
    });
    let arrTableCurrent = document.querySelectorAll('.settings-fee');
    if (arrTableCurrent) {
      arrTableCurrent = Array.from(arrTableCurrent);
      arrTableCurrent.forEach((item) => {
        let inputControl = item.querySelectorAll('input[type=checkbox]');
        inputControl = Array.from(inputControl);
        inputControl.forEach((x) => {
          x.addEventListener('change', () => {
            let inputTable = item.querySelectorAll('input[type=checkbox]');
            inputTable = Array.from(inputTable);
            const inputChecked = inputTable.filter(z => z.checked === true);
            const btnDelete = document.querySelector(`${item.dataset.btnDelete} a`);
            if (inputChecked.length > 0) {
              btnDelete.classList.add('active');
            } else {
              btnDelete.classList.remove('active');
            }
          });
        });
      });
    }
  },
  uploadImage: () => {
    let uploadImagePart = document.querySelectorAll('.image-upload__item');
    uploadImagePart = Array.from(uploadImagePart);
    if (uploadImagePart.length > 0) {
      uploadImagePart.forEach((item) => {
        if (item.dataset.buttonUpload && item.dataset.inputFile) {
          const btnUpload = document.querySelector(item.dataset.buttonUpload);
          const inputImage = document.querySelector(item.dataset.inputFile);
          const valueDelete = document.querySelector(item.dataset.valueDelete);
          const btnSubmit = document.querySelector(item.dataset.btnSubmit);
          btnUpload.addEventListener('click', () => {
            $(inputImage).trigger('click');
          });
          inputImage.addEventListener('change', (file) => {
            const imageChild = item.querySelectorAll('.image-upload__item__image');
            const imageLastChild = imageChild[imageChild.length - 1];
            if (file.target.files && file.target.files[0]) {
              const imgSrc = URL.createObjectURL(file.target.files[0]);
              let classType = '';
              if (item.classList.contains('upload-banner-image')) {
                classType = 'banner';
              } else if (item.classList.contains('upload-big-image')) {
                classType = 'big';
              } else if (item.classList.contains('upload-single-image')) {
                classType = 'single';
              } else {
                classType = 'list';
              }
              const htmlImage = `
                            <div class="image-upload__item__image ${classType}" data-id="${imageChild.length === 0 ? 1 : parseInt(imageLastChild.dataset.id, 10) + 1}">
                              <img class="img-show" src="${imgSrc}">
                                <div class="actions">
                                  <form action="https://www.youtube.com/">
                                    <button type="submit" style="display:none;" data-edit>submit</button>
                                    <button class="btn-edit-image" type="button">
                                      <i class="fa fa-edit"></i>
                                    </button>
                                  <input class="actions__input-image" type="file" accept="image/*">
                                  </form>
                                  <form action="https://www.google.com/">
                                    <button type="submit" style="display:none;" data-delete>submit</button>
                                    <input type="hidden" value=" style="display:none;">
                                    <a class="btn-delete-image" href="javascript:void(0)" data-micromodal-trigger="delete-${item.dataset.title}-${imageChild.length + 1}">
                                      <i class="fa fa-window-close"></i>
                                    </a>
                                      <div class="modal micromodal-slide" id="delete-${item.dataset.title}-${imageChild.length + 1}" aria-hidden="true">
                                        <div class="modal__overlay" tabindex="-1" data-micromodal-close="">
                                          <div class="modal__container" role="dialog" aria-modal="true" aria-labelledby="delete-${item.dataset.title}-${imageChild.length + 1}-title">
                                            <header class="modal__header">
                                              <h2 class="modal__title" id="delete-${item.dataset.title}-${imageChild.length + 1}-title">Bạn có chắc chắn muốn xóa không?</h2>
                                              <button class="modal__close" aria-label="Close modal" data-micromodal-close=""></button>
                                            </header>
                                            <footer class="modal__footer">
                                              <button class="modal__btn modal__btn-primary" data-micromodal-close="" data-confirm>Tiếp tục</button>
                                              <button class="modal__btn" data-micromodal-close="" aria-label="Close this dialog window">Hủy bỏ</button>
                                            </footer>
                                          </div>
                                        </div>
                                      </div>
                                  </form>
                                  
                                </div>
                            </div>  
                          `;
              const htmlBanner = `
              <div class="image-upload__item__image ${classType}" data-id="${imageChild.length === 0 ? 1 : parseInt(imageLastChild.dataset.id, 10) + 1}">
                <img class="img-show" src="${imgSrc}">
                <div class="actions">
                  <button class="btn-edit-image" type="button">
                    <i class="fa fa-edit"></i>
                  </button>
                  <a class="btn-delete-image" href="javascript:void(0)" data-micromodal-trigger="delete-${item.dataset.title}-${imageChild.length + 1}">
                    <i class="fa fa-window-close"></i>
                  </a>
                  <div class="modal micromodal-slide" id="delete-${item.dataset.title}-${imageChild.length + 1}" aria-hidden="true">
                    <div class="modal__overlay" tabindex="-1" data-micromodal-close="">
                      <div class="modal__container" role="dialog" aria-modal="true" aria-labelledby="delete-${item.dataset.title}-${imageChild.length + 1}-title">
                        <header class="modal__header">
                          <h2 class="modal__title" id="delete-${item.dataset.title}-${imageChild.length + 1}-title">Bạn có chắc chắn muốn xóa không?</h2>
                          <button class="modal__close" aria-label="Close modal" data-micromodal-close=""></button>
                        </header>
                        <footer class="modal__footer">
                          <button class="modal__btn modal__btn-primary" data-micromodal-close="" data-confirm>Tiếp tục</button>
                          <button class="modal__btn" data-micromodal-close="" aria-label="Close this dialog window">Hủy bỏ</button>
                        </footer>
                      </div>
                    </div>
                  </div>
                  <input class="actions__input-image" type="file" accept="image/*">
                </div>
              </div>
            `;
              if (classType === 'banner') {
                $(item).append(htmlBanner);
              } else {
                $(item).append(htmlImage);
              }
              Global.initMicroModal();
              ManagerSetting.optionsImage();
              if (item.classList.contains('upload-single-image') || item.classList.contains('upload-banner-image') || item.classList.contains('upload-big-image')) {
                btnUpload.style.display = 'none';
                if (inputImage.nextElementSibling) {
                  inputImage.nextElementSibling.style.display = 'none';
                }
              }
              if (valueDelete) {
                valueDelete.value = 0;
              }
              if (btnSubmit) {
                $(btnSubmit).trigger('click');
              }
            }
          });
        }
      });
    }
  },
  optionsImage: () => {
    let itemImage = document.querySelectorAll('.image-upload__item__image');
    itemImage = Array.from(itemImage);
    if (itemImage.length > 0) {
      itemImage.forEach((item) => {
        const btnUpload = item.parentElement.nextElementSibling;
        const btnEdit = item.querySelector('.actions .btn-edit-image');
        const btnDelete = item.querySelector('.actions button[data-confirm]');
        const inputImage = item.querySelector('.actions .actions__input-image');
        const itemImageSrc = item.querySelector('.img-show');
        const btnSubmitDelete = item.querySelector('.actions button[data-delete]');
        const btnSubmitEdit = item.querySelector('.actions button[data-edit]');
        const valueDelete = document.querySelector(item.parentElement.dataset.valueDelete);
        if (btnEdit && btnDelete && inputImage && itemImageSrc) {
          btnEdit.addEventListener('click', () => {
            $(inputImage).trigger('click');
          });
          inputImage.addEventListener('change', (file) => {
            if (valueDelete) {
              valueDelete.value = 0;
            }
            if (file.target.files[0] && file.target.files[0]) {
              itemImageSrc.src = URL.createObjectURL(file.target.files[0]);
              if (btnSubmitEdit) {
                $(btnSubmitEdit).trigger('click');
              }
            }
          });
          btnDelete.addEventListener('click', () => {
            if (valueDelete) {
              valueDelete.value = 1;
            }
            if (btnSubmitDelete) {
              $(btnSubmitDelete).trigger('click');
            }
            $(item).remove();
            if (item.classList.contains('single') || item.classList.contains('banner')) {
              btnUpload.style.display = 'flex';
            }
          });
        }
      });
    }
  }
};

export default ManagerSetting;
