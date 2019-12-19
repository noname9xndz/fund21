const requiredMess = document.querySelector('#required-mess');
const emailPattern = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
const phonePattern = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
const emailValidate = document.querySelector('input#newLetterEmailValidateMess');
const phoneValidate = document.querySelector('input#newLetterPhoneValidateMess');

const ManagerSettingContact = {
  initFunction: () => {
    ManagerSettingContact.validateFormQuestion();
  },
  validateFormQuestion: () => {
    if ($('#form-contact').length) {
      if (requiredMess) {
        $.extend($.validator.messages, {
          required: requiredMess.value
        });
        $.validator.addMethod(
          'validateEmail',
          value => emailPattern.test(value),
          emailValidate.value,
        );
        $.validator.addMethod(
          'validatePhoneNumber',
          value => phonePattern.test(value),
          phoneValidate.value,
        );
        $('#form-contact').validate({
          rules: {
            email: {
              required: true,
              validateEmail: true
            },
            phone: {
              required: true,
              validatePhoneNumber: true
            },
            contactEmail: {
              required: true,
              validateEmail: true
            }
          },

          submitHandler: () => {
            $('#form-contact').submit();
          }
        });
      }
    }
  }
};

export default ManagerSettingContact;
