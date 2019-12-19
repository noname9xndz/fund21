const requiredMess = document.querySelector('#required-mess');

const ManagerFrequentQuestionAdd = {
  initFunction: () => {
    ManagerFrequentQuestionAdd.validateFormQuestion();
  },
  validateFormQuestion: () => {
    if ($('#form-question').length) {
      if (requiredMess) {
        $.extend($.validator.messages, {
          required: requiredMess.value
        });
        $('#form-question').validate({
          rules: {
            question: {
              required: true
            },
            answer: {
              required: true
            },
            category: {
              required: true
            }
          },

          submitHandler: () => {
          }
        });
      }
    }
  }
};

export default ManagerFrequentQuestionAdd;
