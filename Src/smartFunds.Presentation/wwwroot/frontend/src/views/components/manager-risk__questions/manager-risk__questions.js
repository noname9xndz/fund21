import Global from '../global/global';

const clickGoUp = document.querySelectorAll('.click-go-up');
const clickGoDown = document.querySelectorAll('.click-go-down');
const itemQuestion = document.querySelectorAll('.manager-risk__questions__list-questions__item');
let arrTable = document.querySelectorAll('.table-list-questions');
const updateIndexQuesApi = document.querySelector('#updateIndexQues');


const ManagerRiskQuestions = {
  initFunction: () => {
    ManagerRiskQuestions.initTable();
    ManagerRiskQuestions.initClickMoveUp();
    ManagerRiskQuestions.initClickMoveDown();
    ManagerRiskQuestions.checkDataOptions();
    ManagerRiskQuestions.checkedInput();
    ManagerRiskQuestions.initCollapse('.collapse-part', '.collapse-btn', '.collapse-content');
    ManagerRiskQuestions.initTruncate();
  },
  initCollapse: (parent, btn, content) => {
    const carouselItemWrappers = document.querySelectorAll(parent);
    if (carouselItemWrappers) {
      carouselItemWrappers.forEach((eachCarousel) => {
        const allButtons = eachCarousel.querySelectorAll(btn);
        const allTexts = eachCarousel.querySelectorAll(content);
        allButtons.forEach((eachButton) => {
          const currentContentElm = eachButton.parentElement.parentElement.querySelector(content);
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
  initTable: () => {
    Global.initDataTable('.table-list-questions', {
      searching: false,
      info: false,
      paging: false,
      sort: false,
      columnDefs: [{
        targets: 1,
        orderable: false
      }]
    });

    if (arrTable) {
      arrTable = Array.from(arrTable);
      arrTable.forEach((tableElm) => {
        Global.initAllCheckBox(tableElm);
      });
    }
  },
  initClickMoveUp: () => {
    clickGoUp.forEach((item) => {
      item.addEventListener('click', () => {
        const parentEle = item.parentElement.parentElement.parentElement.parentElement;
        const indexParent = parseInt(parentEle.dataset.index, 10);
        const previousIndex = parentEle.previousElementSibling;
        const newNumberIndex = parseInt(previousIndex.dataset.index, 10);
        if (previousIndex) {
          parentEle.style.order = newNumberIndex;
          parentEle.dataset.index = newNumberIndex;
          previousIndex.style.order = indexParent;
          previousIndex.dataset.index = indexParent;
          const parentItem = item.parentElement;
          if (parentItem.querySelector('.questionId')) {
            const questionIdInput = parentItem.querySelector('.questionId').value;
            const model = {};
            const linkApi = `${updateIndexQuesApi.value}?questionId=${parseInt(questionIdInput, 10)}&newOrder=${newNumberIndex}&currentOrder=${indexParent}`;
            Global.getDataFromUrlPost(linkApi, model).then(() => {
              window.location.reload();
            });
          }
        }
      });
    });
  },
  initClickMoveDown: () => {
    clickGoDown.forEach((item) => {
      item.addEventListener('click', () => {
        const parentEle = item.parentElement.parentElement.parentElement.parentElement;
        const indexParent = parseInt(parentEle.dataset.index, 10);
        const nextIndex = parentEle.nextElementSibling;
        const newNumberIndex = parseInt(nextIndex.dataset.index, 10);
        if (nextIndex) {
          parentEle.style.order = newNumberIndex;
          parentEle.dataset.index = newNumberIndex;
          nextIndex.style.order = indexParent;
          nextIndex.dataset.index = indexParent;
          const parentItem = item.parentElement;
          if (parentItem.querySelector('.questionId')) {
            const questionIdInput = parentItem.querySelector('.questionId').value;
            const model = {};
            const linkApi = `${updateIndexQuesApi.value}?questionId=${parseInt(questionIdInput, 10)}&newOrder=${newNumberIndex}&currentOrder=${indexParent}`;
            Global.getDataFromUrlPost(linkApi, model).then(() => {
              window.location.reload();
            });
          }
        }
      });
    });
  },
  resetPosition: (indexParent, option) => {
    itemQuestion.forEach((item) => {
      if (option === 'up') {
        const currentIndex = parseInt(indexParent, 10) - 1;
        const newIndex = parseInt(item.dataset.index, 10);
        if (newIndex === currentIndex) {
          item.dataset.index = newIndex + 1;
          item.style.order = newIndex + 1;
        }
      } else if (option === 'down') {
        const currentIndex = parseInt(indexParent, 10) + 1;
        const newIndex = parseInt(item.dataset.index, 10);
        if (newIndex === currentIndex) {
          item.dataset.index = newIndex - 1;
          item.style.order = newIndex - 1;
        }
      }
    });
  },
  checkDataOptions: () => {
    let deleteConfirm = document.querySelectorAll('.delete-confirm');
    if (deleteConfirm) {
      deleteConfirm = Array.from(deleteConfirm);
      deleteConfirm.forEach((item) => {
        item.parentElement.addEventListener('click', (e) => {
          const table = item.parentElement.parentElement.parentElement.nextElementSibling;
          let checkbox = table.querySelectorAll('table tr td input[type=checkbox]');
          checkbox = Array.from(checkbox);
          const checkboxTrue = checkbox.filter(x => x.checked === true);
          if (checkboxTrue.length === 0) {
            e.stopPropagation();
          }
          if ((checkbox.length - checkboxTrue.length) < 2) {
            $('#btn-alert-mess').trigger('click');
            e.stopPropagation();
          }
        });
      });
    }
  },
  checkedInput: () => {
    let arrTableCurrent = document.querySelectorAll('.table-list-questions');
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
            const btnDelete = item.parentElement.previousElementSibling.children[1].children;
            if (inputChecked.length > 0) {
              btnDelete[0].classList.add('active');
            } else {
              btnDelete[0].classList.remove('active');
            }
          });
        });
      });
    }
  },
  initTruncate: () => {
    const truncateLine = document.querySelectorAll('.truncate-line');
    if (truncateLine.length > 0) {
      truncateLine.forEach((item) => {
        if (item.textContent.split(' ').length > 25) {
          item.textContent = `${ManagerRiskQuestions.truncate(item.textContent.trim(), 25)}...`;
          // const partQuestion = item.parentElement.nextElementSibling;
          // const showQuestion = partQuestion.querySelector('.show-more-question');
          // showQuestion.classList.add('show');
        }
      });
    }
  },
  truncate: (str, number) => {
    return str.split(' ').splice(0, number).join(' ');
  }
};

export default ManagerRiskQuestions;
