import Global from '../global/global';

const addDetailBtn = document.querySelector('#add-detail-icon');
const apiLinkQuestionDate = document.querySelector('#manager-risk__questions__url');
const questionlink = document.querySelector('#questionList');
const questionNo = document.querySelector('#questionNo');
const showError = document.querySelector('.manager-risk__questions__show-error');

const ManagerRiskQuestionsAdd = {
  initFunction: () => {
    ManagerRiskQuestionsAdd.addDetail();
    ManagerRiskQuestionsAdd.closeAnswear();
    ManagerRiskQuestionsAdd.submitForm();
    ManagerRiskQuestionsAdd.inputEventValid();
  },
  addDetail: () => {
    if (addDetailBtn) {
      addDetailBtn.addEventListener('click', () => {
        const listQuestions = document.querySelectorAll('.form-answers');
        const html = `          
          <div class="form-group form-answers">
            <div class="close-answers">
              <i class="fa fa-window-close" data-micromodal-trigger="delete-answer-${listQuestions.length + 1}"></i>
              <div class="modal micromodal-slide" id="delete-answer-${listQuestions.length + 1}" aria-hidden="true">
                <div class="modal__overlay" tabindex="-1" data-micromodal-close="">
                  <div class="modal__container" role="dialog" aria-modal="true" aria-labelledby="delete-answer-${listQuestions.length + 1}-title">
                    <header class="modal__header">
                      <h2 class="modal__title" id="delete-answer-${listQuestions.length + 1}-title">Bạn có chắc chắn muốn xóa không?</h2>
                      <button class="modal__close" aria-label="Close modal" data-micromodal-close=""></button>
                    </header>
                    <footer class="modal__footer">
                      <button class="modal__btn modal__btn-primary" data-micromodal-close="" type="button">Tiếp tục</button>
                      <button class="modal__btn" data-micromodal-close="" aria-label="Close this dialog window">Hủy bỏ</button>
                    </footer>
                  </div>
                </div>
              </div>
            </div>
            <label>Đáp án thứ ${listQuestions.length + 1}</label>
            <textarea class="form-control input-control" name="KVRRAnswers[${listQuestions.length}].Content"></textarea>
            <span class="errorMSG">Trường Đáp Án không được để trống</span>
            <span>Trọng Số</span>
            <input class="form-control input-control inputNumber" type="number" name="KVRRAnswers[${listQuestions.length}].Mark">
            <span class="errorMSG">Trường Trọng Số không được để trống</span>
          </div>
        `;
        $('#add-list').append(html);
        ManagerRiskQuestionsAdd.closeAnswear();
        ManagerRiskQuestionsAdd.inputEventValid();
        Global.initMicroModal();
      });
    }
  },

  closeAnswear: () => {
    const closeAnswearBtn = document.querySelectorAll('.close-answers');
    if (closeAnswearBtn.length > 0) {
      closeAnswearBtn.forEach((item) => {
        const btnDelete = item.querySelector('button[type="button"]');
        btnDelete.addEventListener('click', () => {
          const listQuestions = document.querySelectorAll('.form-answers');
          if (listQuestions.length > 2) {
            const itemParent = item.parentElement;
            $(itemParent).remove();
            const lisAnswers = document.querySelectorAll('.form-answers');
            lisAnswers.forEach((itemAnswer, index) => {
              const labelName = itemAnswer.querySelector('label');
              const textArea = itemAnswer.querySelector('textarea');
              const inputNumber = itemAnswer.querySelector('input');

              labelName.textContent = `Đáp án thứ ${index + 1}`;
              textArea.name = `KVRRAnswers[${index}].Content`;
              inputNumber.name = `KVRRAnswers[${index}].Mark`;
            });
          }
        });
      });
    }
  },

  inputEventValid: () => {
    const formControl = document.querySelectorAll('.input-control');
    const inputNumberManageRisk = document.querySelectorAll('.inputNumber');
    formControl.forEach((x) => {
      $(x).on(('input paste'), () => {
        if (x.value === '') {
          x.nextElementSibling.style.display = 'block';
        } else {
          x.nextElementSibling.style.display = 'none';
        }
      });
    });
    inputNumberManageRisk.forEach((a) => {
      $(a).keyup(() => {
        if (a.value === '') {
          a.nextElementSibling.style.display = 'block';
        } else {
          a.nextElementSibling.style.display = 'none';
        }
        if (a.value.includes('-')) {
          a.value = a.value.replace(/-/g, '');
        }
      });
    });
    inputNumberManageRisk.forEach((inputNumberElement) => {
      $(inputNumberElement).on(('input paste'), () => {
        if (inputNumberElement.value.includes('.')) {
          inputNumberElement.value = Math.round(inputNumberElement.value);
        }
      });
    });
  },

  submitForm: () => {
    if (apiLinkQuestionDate) {
      document.querySelector('.formAnswers').addEventListener('submit', (evt) => {
        evt.preventDefault();
        const errorMSG = document.querySelectorAll('.errorMSG');
        const newErrorMSG = Array.from(errorMSG);
        newErrorMSG.forEach((item) => {
          if (item.previousElementSibling.value === '') {
            item.style.display = 'block';
          } else {
            item.style.display = 'none';
          }
        });
        const checkValidate = newErrorMSG.filter(x => x.style.display === 'none');
        if (checkValidate.length === newErrorMSG.length) {
          ManagerRiskQuestionsAdd.sendAjax();
        }
      });
    }
  },
  sendAjax: () => {
    const formAnswers = document.querySelectorAll('.form-answers');
    const answersArray = [];
    formAnswers.forEach((item, index) => {
      const content = item.querySelector(`textarea[name="KVRRAnswers[${index}].Content"]`).value;
      const number = item.querySelector(`input[name="KVRRAnswers[${index}].Mark"]`).value;
      answersArray.push({
        Content: content,
        Mark: parseInt(number, 10)
      });
    });
    if (apiLinkQuestionDate.value.includes('New')) {
      const model = {
        KVRRQuestionCategories: document.querySelector('.form-control[name="KVRRQuestionCategories"]').value,
        Content: document.querySelector('.form-control[name="Content"]').value,
        KVRRAnswers: answersArray,
        No: 0,
        AnswerSelected: null
      };
      Global.getDataFromUrlPost(apiLinkQuestionDate.value, model).then((data) => {
        const dataBack = JSON.parse(data);
        if (dataBack.success) {
          window.open(questionlink.value, '_self');
        }
        if (!dataBack.success) {
          if (dataBack.message) {
            if (showError) {
              showError.textContent = dataBack.message;
            }
          }
        }
      });
    } else {
      const model = {
        Id: document.querySelector('#questionId').value,
        KVRRQuestionCategories: document.querySelector('.form-control[name="KVRRQuestionCategories"]').value,
        Content: document.querySelector('.form-control[name="Content"]').value,
        KVRRAnswers: answersArray,
        No: parseInt(questionNo.value, 10)
      };
      Global.getDataFromUrlPost(apiLinkQuestionDate.value, model).then((data) => {
        const dataBack = JSON.parse(data);
        if (dataBack.success) {
          window.open(questionlink.value, '_self');
        }
        if (!dataBack.success) {
          if (dataBack.message) {
            if (showError) {
              showError.textContent = dataBack.message;
            }
          }
        }
      });
    }
  }
};

export default ManagerRiskQuestionsAdd;
