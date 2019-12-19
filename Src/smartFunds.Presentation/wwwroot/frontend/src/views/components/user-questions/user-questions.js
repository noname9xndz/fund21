const form = document.querySelector('form#user-quizform');
const $form = $(form);

const userQuestion = {
  initJquerySteps() {
    if ($form.length) {
      $form.steps({
        headerTag: 'h3',
        bodyTag: '.quizmain-container',
        transitionEffect: 'slideLeft',
        labels: {
          next: 'Tiếp Theo',
          previous: 'Quay lại',
          finish: 'Hoàn tất'
        },
        onStepChanging: (event, currentIndex, newIndex) => {
          // Allways allow previous action even if the current form is not valid!
          const currentStep = document.querySelector('.quizmain-container.current');
          if (currentIndex > newIndex) {
            return true;
          }

          // Next step
          if (currentIndex < newIndex) {
            // To remove error styles
            const allCurrentStepInputs = form.querySelectorAll('.quizmain-container.current input[type="radio"]');
            if (Array.from(allCurrentStepInputs).some(eachInput => !!eachInput.checked)) {
              currentStep.style.position = 'absolute';
            }
            return Array.from(allCurrentStepInputs).some(eachInput => !!eachInput.checked);
          }
        },
        onStepChanged: (event, currentIndex, priorIndex) => {
          const currentStep = document.querySelector('.quizmain-container.current');
          currentStep.style.position = 'relative';
          // debugger;
          if (userQuestion.hasOneChecked()) {
            userQuestion.addActiveBtnNext();
          } else {
            userQuestion.removeActiveBtnNext();
          }
          if (currentIndex !== 0) {
            form.classList.add('show-previous-btn');
          } else {
            form.classList.remove('show-previous-btn');
          }
          userQuestion.detectInputChecked();
        },
        onFinished: (event, currentIndex) => {
          form.submit();
        }
      });
    }
  },
  changeButtonColorOnInputClick() {
    const formQuest = document.querySelector('form#user-quizform');
    if (formQuest) {
      const allInputs = formQuest.querySelectorAll('input[type="radio"]');
      allInputs.forEach((eachInput) => {
        eachInput.addEventListener('change', () => {
          userQuestion.addActiveBtnNext();
        });
      });
    }
  },
  addActiveBtnNext() {
    form.classList.add('active-next');
  },
  removeActiveBtnNext() {
    form.classList.remove('active-next');
  },
  hasOneChecked() {
    const allInputs = form.querySelectorAll('.quizmain-container.current input[type="radio"]');
    return Array.from(allInputs).some(x => x.checked);
  },
  detectInputChecked: () => {
    const allInputs = document.querySelectorAll('.quizmain-container.current input[type="radio"]');
    const allLabels = document.querySelectorAll('.quizmain-container.current .radio-answers-container');
    if (allLabels) {
      allLabels.forEach((eachLabel) => {
        eachLabel.addEventListener('click', () => {
          allInputs.forEach((eachInput) => {
            if (eachInput.checked) {
              eachLabel.classList.add('active');
            }
            if (!eachInput.checked) {
              eachInput.parentNode.classList.remove('active');
            }
          });
        });
      });
    }
  }
};
export default userQuestion;
