const requiredMess = document.querySelector('#required-mess');

const ManagerFrequentQuestionCategory = {
  initFunction: () => {
    ManagerFrequentQuestionCategory.validateFormCategory();
  },
  validateFormCategory: () => {
    if ($('#form-caterogy').length) {
      if (requiredMess) {
        $.extend($.validator.messages, {
          required: requiredMess.value
        });
        $('#form-caterogy').validate({
          rules: {
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

export default ManagerFrequentQuestionCategory;
