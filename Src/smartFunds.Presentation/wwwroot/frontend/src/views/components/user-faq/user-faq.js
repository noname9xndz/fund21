/* eslint-disable no-param-reassign */
import Global from '../global/global';
import DatatableLanguages from '../datatables/datatables-language';

const userFAQ = {
  initAccordionsForFAQ: () => {
    const allQuestion = document.querySelectorAll('.user-faq-name');
    const allAnswers = document.querySelectorAll('.user-faq-content');
    if (allQuestion) {
      allAnswers.forEach((answer) => {
        // eslint-disable-next-line no-param-reassign
        answer.style.display = 'none';
      });
      allQuestion.forEach((question) => {
        question.removeEventListener('click', userFAQ.toggleQuestionParagraph, true);
        question.addEventListener('click', userFAQ.toggleQuestionParagraph, true);
      });
    }
  },
  toggleQuestionParagraph: (e) => {
    e.currentTarget.parentNode.classList.add('is-open');
    if (e.currentTarget.nextElementSibling.style.display === 'none') {
      e.currentTarget.nextElementSibling.style.display = 'block';
    } else {
      e.currentTarget.nextElementSibling.style.display = 'none';
      e.currentTarget.parentNode.classList.remove('is-open');
    }
  },
  searchingQuestion: () => {
    const allContentAnswers = document.querySelectorAll('.user-faq-name');
    const contentSearch = document.getElementById('faq-form-control');
    const close = document.querySelector('.input-form-control-reset');
    const btnSearch = document.querySelector('.btn-search');
    if (btnSearch && contentSearch) {
      allContentAnswers.forEach((items) => {
        function displayMatches() {
          const regex = new RegExp(contentSearch.value, 'gi');
          const response = items.innerText.replace(regex, str => `<span style='color: #F77314;'>${str}</span>`);
          items.innerHTML = response;
          contentSearch.parentNode.classList.add('is-reset');
          if (contentSearch.value.length === 0) {
            contentSearch.parentNode.classList.remove('is-reset');
          }
        }
        function unDisplayMatches() {
          const regex = new RegExp(contentSearch.value, 'gi');
          const response = items.innerText.replace(regex, str => `${str}`);
          items.innerHTML = response;
        }
        contentSearch.addEventListener('change', displayMatches);
        contentSearch.addEventListener('keyup', displayMatches);
        if (close) {
          close.addEventListener('click', () => {
            unDisplayMatches();
            setTimeout(() => {
              close.parentNode.classList.remove('is-reset');
            }, 200);
          });
        }
      });
    }
    if (contentSearch.value) {
      allContentAnswers.forEach((items) => {
        const regex = new RegExp(contentSearch.value, 'gi');
        const response = items.innerText.replace(regex, str => `<span style='color: #F77314;'>${str}</span>`);
        items.innerHTML = response;
      });
    }
  },
  initPagination: () => {
    const listQuestions = document.querySelector('#user-faq-view__table');
    if (listQuestions) {
      $(listQuestions).DataTable({
        info: false,
        searching: false,
        lengthChange: false,
        language: DatatableLanguages,
        columnDefs: [{
          targets: 1,
          orderable: false
        }],
        drawCallback: () => {
          Global.hidePagingIfOnepage(listQuestions);
          userFAQ.initAccordionsForFAQ();
        },
        createdRow: () => {
          userFAQ.searchingQuestion();
        }
      });
      // eslint-disable-next-line no-console
      Global.drawTableWithLength(listQuestions);
      Global.initPageLengthChange(listQuestions);
    }
  },
  initPagesizeOption: () => {
    if ($('.user-faq-filter-pagesize__select')) {
      $('.user-faq-filter-pagesize__select').selectmenu({
        width: 80,
        appendTo: '.user-faq-filter-pagesize',
        select: () => {
          const listQuestions = document.querySelector('#user-faq-view__table');
          const sectionElm = Global.getParentElemetWithTagName(listQuestions, 'section');
          const selectPageLength = sectionElm.querySelector('.table-filter-actions__pagesize__select');
          if (selectPageLength) {
            Global.drawTableWithLength(listQuestions, selectPageLength.value);
            Global.initPageLengthChange(listQuestions);
          }
        }
      });
    }
  }
};
export default userFAQ;
