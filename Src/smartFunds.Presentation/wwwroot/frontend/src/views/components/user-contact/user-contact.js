let isSubmitted = false;
const ContactPage = {
  focusInput: () => {
    const specialInput = $('.contact-section .form_contact .user-input--change-1');
    if (specialInput) {
      specialInput.on('focus', () => {
        $('.contact-section .form_contact .user-input--change-1 + .placeholder span').css({
          'font-weight': '500', 'font-size': '11px', 'z-index': '11', top: '8px'
        });
      });
      specialInput.on('blur', () => {
        $('.contact-section .form_contact .user-input--change-1 + .placeholder span').css({
          'font-weight': 'normal', 'font-size': '14px', 'z-index': '9', top: '17px'
        });
      });
      specialInput.on('blur', () => {
        $('.contact-section .form_contact .user-input--change-1.input-has-value + .placeholder span').css({
          'font-weight': '500', 'font-size': '11px', 'z-index': '11', top: '8px'
        });
      });
    }
  },
  loadingSubmitForm: () => {
    const submitForm = $('.submit-form-general form');
    const inputElm = $('.submit-form-general form input');
    const btnSubmit = $('.submit-form-general form .button-submit-general');
    if (submitForm.length && inputElm.length && btnSubmit.length) {
      submitForm.on('submit', () => {
        if (!isSubmitted) {
          if (inputElm.hasClass('input-validation-error')) {
            return false;
          }
          isSubmitted = true;
        } else {
          return false;
        }
      });
    }
  }
};
export default ContactPage;
